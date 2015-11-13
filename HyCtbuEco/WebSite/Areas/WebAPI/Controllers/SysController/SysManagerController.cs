using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HyCtbuEco.Entities;
using HyCtbuEco.Admin;

using TMS.Common;

using HyCtbuEco.Models;
using HyCtbuEco.Services;


namespace WebSite.Areas.WebApi.Controllers
{

    [UserAuthorizeSys]
    public class SysManagerController : BaseController
    {
        
        private SysUserDAO suDao = new SysUserDAO();
        private SysRoleDAO srDao = new SysRoleDAO();
        private SysPartDAO syspartDAO = new SysPartDAO();
        private SysUserRoleDAO sysUserRoleDao = new SysUserRoleDAO();


        #region 验证用户名唯一
        /// <summary>
        /// 验证用户名唯一
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public JsonResult ValidateUserName(string UserName)
        {
            string whereClause = "SUserName='" + UserName + "'";
            IList<TbSysUser> userList = new SysUserDAO().GetByPage(whereClause, 0, int.MaxValue);
            if (userList.Count > 0)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        } 
        #endregion


        #region 从user和part中获取数据
        /// <summary>
        /// 从user和part中获取数据
        /// </summary>
        /// <param name="firstResult"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderBy"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public JsonResult GetByPageUserAndUserRole(int firstResult, int pagesize, string orderBy, string condition)
        {
            int count = 0;
            condition = HttpUtility.UrlDecode(condition);//对条件解码
            List<UserAndUserRole> Data = new List<UserAndUserRole>();//user加上部门名称的模型集合
            //获取当前页的user；
            IList<TbSysUser> userList = suDao.GetByPageDataBase(firstResult, pagesize, "ID", condition, out count).ToList();
            foreach (TbSysUser item in userList)
            {
                UserAndUserRole tmpChildren = new UserAndUserRole();
                string SPartName = "";
                //为每个user加上部门名称
                if (!string.IsNullOrEmpty(item.IPart.ToString()))
                {
                    int partId = (int)item.IPart;
                    if (partId != 0)
                    {
                        SPartName = syspartDAO.GetByID(partId).SPartName;
                        tmpChildren.SPartName = SPartName;
                    }
                    else
                        tmpChildren.SPartName = "";
                }
                ClassValueCopier.Copy(tmpChildren, item);

                Data.Add(tmpChildren);
            }

            return Json(new
            {
                DataCount = count,
                Data = Data,
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region 同时对user和userRole进行添加
        /// <summary>
        /// 同时对user和userRole进行添加
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="RoleIds"></param>
        /// <returns></returns>
        public int AddUserAndUserRole(TbSysUser User, string RoleIds)
        {

            try
            {
                string SRoleNames = "";  //用于保存拥有的角色
                string[] IRoleIds = RoleIds.Split(new char[] { ',' });//拆分角色Id；
                TbSysUserRole UserRole = new TbSysUserRole();//userRole添加的对象
                foreach (var ID in IRoleIds)
                {
                    UserRole.IRoleId = int.Parse(ID);
                    SRoleNames = srDao.GetByID(int.Parse(ID)).SRoleName + "," + SRoleNames;
                }
                //清除多余的分隔符','
                SRoleNames = SRoleNames.Substring(0, SRoleNames.Length - 1);

                User.SRoleName = SRoleNames;
                User.SPassword = Utitil.MD5(User.SPassword);   //对用户的密码在客户端进行MD5加密
                User.SOtherPassword = Utitil.MD5(User.SOtherPassword); 


                User.DCreateDate = DateTime.Now;//创建时间
                User.DLastLoginDate = DateTime.Now;//默认最后登录时间为创建时间

                suDao.Insert(User);//插入    

                UserRole.IUserId = User.Id;
                foreach (string id in IRoleIds)
                {
                    UserRole.IRoleId = int.Parse(id);
                    sysUserRoleDao.Insert(UserRole);
                }
                return 0;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        #endregion





        #region 同时对两张表进行删除，user和userRole;
        /// <summary>
        /// 同时对两张表进行删除，user和userRole;
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteUserAndUserRole(string ids)
        {
            try
            {
                int count = 0;
                string[] SIds = ids.Split(new char[] { ',' });

                foreach (string SId in SIds)
                {
                    int IId = int.Parse(SId);//User的id;
                    //查找目标user拥有角色
                    List<TbSysUserRole> list = sysUserRoleDao.GetByPage("IUserid =" + IId, 0, 20, "ID", "DESC", out count).ToList();

                    foreach (var i in list)
                    {
                        sysUserRoleDao.Del(i.Id);//逐个角色删除
                    }

                    suDao.Del(IId);//删除user
                }
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        #endregion



        #region 获取需要更新的用户角色
        /// <summary>
        /// 获取需要更新的用户角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetUserAndUserRole(int id)
        {
            TbSysUser user = suDao.GetByID(id);//获取当前用户


            //修正把密码发送到客户端的问题
            TbSysUser tempUser = new TbSysUser();  //新建一个对象
            ClassValueCopier.Copy(tempUser, user);
            tempUser.SPassword = "";  //把系统后台用户的密码置空

            UserAndUserRole item = new UserAndUserRole();
            if (!string.IsNullOrEmpty(item.IPart.ToString()))
            {
                int partId = (int)item.IPart;
                TbSysPart tempPart = syspartDAO.GetByID(partId);
                if (tempPart != null)
                {
                    item.SPartName = tempPart.SPartName;
                }

            }
            ClassValueCopier.Copy(item, tempUser);
            return Json(item, JsonRequestBehavior.AllowGet);
        } 
        #endregion


        #region 更新用户角色
        /// <summary>
        /// 更新用户角色
        /// </summary>
        /// <param name="item"></param>
        /// <param name="RoleIds"></param>
        /// <returns></returns>
        public int PutUserAndUserRole(TbSysUser item, string RoleIds)
        {
            try
            {
                TbSysUser tempuser = new SysUserDAO().GetByID(item.Id);
                if (tempuser != null)
                {
                    item.DCreateDate = tempuser.DCreateDate;    //更新时不更新用户的创建时间
                }

                //在对密码进行加密前先验证客户端传过来的密码是否非空
                if (!String.IsNullOrEmpty(item.SPassword))
                {
                    item.SPassword = Utitil.MD5(item.SPassword);  //对密码进行md5加密

                    //使用了更新密码功能
                    LogWriter.Default.WriteWarning("使用了管理员更新密码功能,ip:" + Utitil.getIP()
                        + ",更改的用户名" + item.SUserName + ",当前用户id:" + SysUserInfo.GetUserID());

                }
                else 
                {
                    item.SPassword = tempuser.SPassword;  //在未更改密码的情况下，使用原密码
                }



                if (!String.IsNullOrEmpty(item.SOtherPassword))
                {
                    item.SOtherPassword = Utitil.MD5(item.SOtherPassword);  //对密码进行md5加密

                    //使用了更新密码功能
                    LogWriter.Default.WriteWarning("使用了管理员更新第二密码功能,ip:" + Utitil.getIP()
                        + ",更改的用户名" + item.SUserName + ",当前用户id:" + SysUserInfo.GetUserID());

                }
                else
                {
                    item.SOtherPassword = tempuser.SOtherPassword;  //在未更改密码的情况下，使用原密码
                }

                if (!string.IsNullOrEmpty(RoleIds))
                {
                    string[] IRoleIds;
                    string SRoleName = "";
                    IRoleIds = RoleIds.Split(new char[] { ',' });//拆分角色Id；
                    TbSysUserRole UserRole = new TbSysUserRole();//userRole添加的对象
                    UserRole.IUserId = item.Id;
                    IList<TbSysUserRole> OldUserRole = sysUserRoleDao.GetByPage("IUserId =" + item.Id, 0, int.MaxValue);//找到以前拥有的所有角色
                    if (OldUserRole.Count > 0)
                    {
                        sysUserRoleDao.Del(OldUserRole);  //通过对象集的方法删除
                    }

                    //添加user新的角色
                    foreach (string id in IRoleIds)
                    {
                        UserRole.IRoleId = int.Parse(id);
                        SRoleName = srDao.GetByID(int.Parse(id)).SRoleName + "," + SRoleName;//拼装角色字符串
                        if (sysUserRoleDao.Insert(UserRole) == 0)
                            return 0;//添加失败
                    }
                    SRoleName = SRoleName.Substring(0, SRoleName.Length - 1);
                    item.SRoleName = SRoleName;
                }
                suDao.Update(item);
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        } 
        #endregion





        /// <summary>
        /// 验证当前密码是否与原密码一致
        /// </summary>
        /// <returns></returns>
        public JsonResult SysCheckPassword(string Password) 
        {
            int result = 0;  
            if (!string.IsNullOrEmpty(Password))
            {
                Password = Utitil.MD5(Password);  //把密码进行MD5加密
                TbSysUser item = new SysUserDAO().GetByID(SysUserInfo.GetUserID());
                if (item != null)
                {
                    if (Password == item.SPassword)
                    {
                        result = 1;
                    }
                }
                
            }
            return Json(result, JsonRequestBehavior.AllowGet);
            
        }


        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public JsonResult SysUpdatePassword(string Password,String OtherPassword="")
        {
            TbSysUser item = new SysUserDAO().GetByID(SysUserInfo.GetUserID());
            if (item != null)
            {
                Password = Utitil.MD5(Password);
                item.SPassword = Password;
                

                //使用了更新密码功能
                  LogWriter.Default.WriteWarning("使用了更新密码功能,ip:" + Utitil.getIP() + ",用户名" + item.SUserName );

                  if (OtherPassword!= "")
                  {
                     
                      item.SPassword = Utitil.MD5(OtherPassword);

                  }

                  suDao.Update(item);

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }



        //查询q,返回page_limit,取用户呢称
        public JsonResult QueryByUserTrueName(string q, int page_limit)
        {
            IList<TbSysUser> tmpList = suDao.GetByWhere("STrueName like '%" + q.Trim() + "%'", "ID", 10);
            return Json(tmpList, JsonRequestBehavior.AllowGet);
        }

    }
}
