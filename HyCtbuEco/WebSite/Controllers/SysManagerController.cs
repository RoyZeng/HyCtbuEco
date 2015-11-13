using HyCtbuEco.Admin;
using TMS.Common;
using HyCtbuEco.Entities;
using HyCtbuEco.Models;
using HyCtbuEco.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HyCtbuEco2015.Controllers
{

    [UserAuthorizeSys]
    public class SysManagerController : Controller
    {
      
        public ActionResult Index()
        {
            ViewBag.SUserName = SysUserInfo.GetSysUserName();  //当前用户的用户名
            if (Session["SysHeader"] != null)
            {
                ViewBag.ImageURL = Session["SysHeader"];
            }
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        //菜单
        public ActionResult Menu()
        {
            TMS.Common.ComTreeNode root = new TMS.Common.ComTreeNode();
            IList<TbSysPagePower> spLists = new SysPagePowerDAO().finByIds(SysUserInfo.GetSysUserPowerList());


            if (spLists.Count > 0)
            {
                //接下来进行节点组装


                root.NodeName = "根结点";
                Dictionary<int, TMS.Common.ComTreeNode> nodeslist = new Dictionary<int, TMS.Common.ComTreeNode>();
                foreach (var itemPower in spLists)
                {
                    TMS.Common.ComTreeNode myNode = new TMS.Common.ComTreeNode();
                    TMS.Common.ComTreeNode tempNode;
                    myNode.NodeName = itemPower.SMenuName;

                    myNode.Id = itemPower.Id;
                    myNode.Url = itemPower.SMenuURL;
                    myNode.Tip = itemPower.SPicUrl;

                    nodeslist.Add(itemPower.Id, myNode);

                    if (Convert.ToInt32(itemPower.IParentId) != 0)
                    {
                        tempNode = (TMS.Common.ComTreeNode)nodeslist[(int)itemPower.IParentId];

                        tempNode.Childrens.Add(myNode);
                    }
                    else
                    {
                        root.Childrens.Add(myNode);
                    }

                }

            }

            ViewBag.MenuTree = root;
            return View();
        }


        /// <summary>
        /// 系统用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            List<TbSysPart> PartList = new SysPartDAO().GetByPage("1=1" , 0, int.MaxValue).ToList();
            IList<TbSysRole> list = new SysRoleBaseDAO().GetAll();
            ViewBag.roleList = list;
            ViewBag.partList = PartList;
            return View();
        }


        /// <summary>
        /// 部门管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Parts()
        {

            return View();
        }


        /// <summary>
        ///页面管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Pages()
        {

            return View();
        }


        /// <summary>
        ///角色管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Role()
        {

            return View();
        }



        /// <summary>
        //日志浏览
        /// </summary>
        /// <returns></returns>
        public ActionResult Log()
        {

            return View();
        }


        /// <summary>
        /// 通用SQL语句管理
        /// </summary>
        /// <returns></returns>
        public ActionResult CommonSQLMangement()
        {
            ViewBag.UserID = 4;  
            return View();
        }
        /// <summary>
        /// 通用数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult CommonDataImport()
        {

            return View();
        }




        /// <summary>
        /// 数据导出
        /// </summary>
        /// <returns></returns>
        public ActionResult CommonDataExport()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult SysChangePassWord()
        {
            return View();
        }






      


    }
}
