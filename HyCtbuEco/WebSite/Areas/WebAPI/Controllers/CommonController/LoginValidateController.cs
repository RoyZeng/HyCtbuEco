using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TMS.Common;
using System.Web.SessionState;
using HyCtbuEco.Admin;
using HyCtbuEco.Entities;
using HyCtbuEco.Guest;
using HyCtbuEco.Models;


namespace WebSite.Areas.WebApi.Controllers
{

    public class LoginValidateController : BaseController
    {
        private SysUserDAO suDao = new SysUserDAO();  //SysUserDAO对象
        private SysRoleDAO srDao = new SysRoleDAO();  //SysRoleDAO对象

        private SysUserRoleDAO sysUserRoleDao = new SysUserRoleDAO();  //SysUserRoleDAO对象

        /// <summary>
        /// 退出系统
        /// </summary>
        public void LogoutSysUser()
        {
            //Session.RemoveAll();
            SysUserInfo.ClearSession();


        }




        /// <summary>
        /// 访客退出系统
        /// </summary>
        public void LogoutGuestUser() 
        {

            //清除session
            GuestUserInfo.ClearSession();
        
        }


        #region 系统用户登录验证
        public JsonResult SysUser(string UserName, string PassWord, string valide)
        {
            //默认主页
            string DefaultURL = "/SysManager/Index#/SysManager/Home";

            string newURL = "";
            int result = -3;  //无法接受的用户名和密码
            //用户名存在非法字符
            if (!checkPara(UserName))
            {
                result = -1;
            }
            else
            {
                UserName = Utitil.SqlFilter(UserName);//过滤防注入

                if ((!string.IsNullOrEmpty(UserName)) && (!string.IsNullOrEmpty(PassWord)) && (!string.IsNullOrEmpty(valide)))
                {
                    //验证码错误
                    if (Session["ValidateCode"].ToString() != valide)
                    {
                        result = -2;
                    }
                    else
                    {


                        PassWord = Utitil.MD5(PassWord);//加密

                        if (ValidateSysUser(UserName, PassWord, out newURL))
                        {

                            result = 1;//登陆成功
                        }
                        else
                        {
                            result = 0;  //错误的用户名或者密码
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(newURL))
            {
                DefaultURL = newURL;
            }
            return Json(new
            {
                rs = result,
                mainURL = DefaultURL
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 系统用户的验证
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password">MD5口令</param>
        /// <returns></returns>
        private bool ValidateSysUser(string UserName, string Password, out string MainURL)
        {
            string WhereClause = "SUserName ='" + UserName + "' and  SPassword='" + Password + "' ";//只许系统管理员登录
            int count = 0;
            IList<TbSysUser> tmpItems = suDao.GetByPageDataBase(0, 1, "ID ASC", WhereClause, out count);
            if (tmpItems.Count > 0)
            {
                TbSysUser item = tmpItems[0];//取第一个用户

                MainURL = item.SMainURL;//默认主页

                Session["UserID"] = item.Id;//用户id,
                Session["UserName"] = item.SUserName;//用户名
                Session["TrueName"] = item.STrueName;//用户真名
                Session["PartID"] = item.IPart;//用户所在部门ID;
                if (!string.IsNullOrEmpty(item.SImg))
                {
                    Session["SysHeader"] = item.SImg;
                }
                else
                {
                    Session["SysHeader"] = "/img/avatars/sunny.png";///img/avatars/sunny.png，使用默认图片
                }
                //(2)根据id，取用户的角色

                IList<TbSysUserRole> tmpUserRoles = new SysUserRoleDAO().findByIUserId(item.Id);
                string strRoleName = "", strRoleID = "", strPowerList = "";//角色列表，权限列表
                IList<string> powerList = new List<string>();
                foreach (var itemR in tmpUserRoles)
                {
                    TbSysRole tmpRole = srDao.GetByID((int)itemR.IRoleId);

                    if (tmpRole != null)
                    {
                        if (!string.IsNullOrEmpty(tmpRole.SPower))
                        {

                            strPowerList += tmpRole.SPower;//保存该用户的所有权限
                        }

                        strRoleName += tmpRole.SRoleName + ",";
                        strRoleID += itemR.IRoleId + ",";//保存该用户的所有角色
                    }


                }

                //由于多个角色的权限表可能相同，所以需要合并

                strPowerList = Utitil.clearRsplit(strPowerList, ',');//清除无效的,号

                Session["RoleName"] = strRoleName;
                Session["RoleID"] = strRoleID;
                Session["PowerList"] = strPowerList;


                //得到登录IP与最后登录时间
                item.SCIP = Utitil.getIP();
                item.DLastLoginDate = DateTime.Now;
                //更新信息
                suDao.Update(item);
                return true;

            }
            else
            {

                LogWriter.Default.WriteWarning("试图登录错误警告,ip:" + Utitil.getIP() + ",用户名" + UserName + "密码" + Password);
                MainURL = "";
                return false;
            }

        }

        #endregion




        #region 访客登录验证
        /// <summary>
        /// 访客登录验证
        /// </summary>
        /// <param name="UserName">访客用户名</param>
        /// <param name="PassWord">登录密码</param>
        /// <param name="ValidCode">验证码</param>
        /// <returns></returns>
        public JsonResult GuestUser(string UserName, string PassWord, string ValidCode)
        {

            int result = -3;  //无法接受的用户名或者密码
            if ((!string.IsNullOrEmpty(UserName)) && (!string.IsNullOrEmpty(PassWord)) && (!string.IsNullOrEmpty(ValidCode)))
            {
                PassWord = Utitil.MD5(PassWord);//对密码加密


                if (Session["GuestValidateCode"].ToString() != ValidCode) 
                {
                    result = -2;  //验证码错误
                }

                if (ValidateGuest(UserName, PassWord))
                {
                    result = 1;//登陆成功
                }
                else
                {
                    result = 0;  //错误的用户名或密码
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        // <summary>
        /// 访客登录验证
        /// </summary>
        /// <param name="UserName">登录用户名</param>
        /// <param name="PassWord">登录密码</param>
        /// <returns></returns>
        public static bool ValidateGuest(string UserName, string PassWord)
        {
            GuestsDAO _GuestsDao = new GuestsDAO();  //GuestsDAO对象
            HttpSessionState Session = System.Web.HttpContext.Current.Session;

            string WhereClause = "SGuestName ='" + UserName + "' and  SPassword='" + PassWord + "' ";

            IList<TbGuests> list = _GuestsDao.GetByPage(WhereClause, 0, int.MaxValue);
            if (list.Count > 0)
            {
                Session["GuestID"] = list[0].Id;
                return true;
            }

            return false;
        }

        #endregion



        /// <summary>
        /// 检查参数中是否有非法字符'和"
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        private bool checkPara(string paras)
        {
            if (paras.IndexOf("'") >= 0 || paras.IndexOf("\"") >= 0)
            {
                return false;
            }
            return true;
        }
        #region 操作cookie



        ///// <summary>
        ///// 删除指定名称的cookie
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public bool DelCookie(string name)
        //{
        //    //删除cookie
        //    HttpCookie cookie = new HttpCookie(name);
        //    cookie.Expires = System.DateTime.Now.AddDays(-4);
        //    Response.Cookies.Add(cookie);

        //    //清空session
        //    SysUserInfo.ClearSession();
        //    return true;
        //}

        #endregion

    }
}
