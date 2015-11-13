using HyCtbuEco.Admin;
using HyCtbuEco.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace HyCtbuEco.Models
{
    public  class SysUserInfo
    {

        /// <summary>
        /// 获取当前管理员的角色名
        /// </summary>
        /// <returns></returns>
        public static string GetSysUserRole()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["RoleName"] == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(tempSession["RoleName"]);
            }

        }
        /// <summary>
        /// 获取当前管理员的角色id
        /// </summary>
        /// <returns></returns>
        public static string GetSysUserRoleID()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["RoleID"] == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(tempSession["RoleID"]);
            }

        }

        /// <summary>
        /// 获取当前管理员的角色权限列表
        /// </summary>
        /// <returns></returns>
        public static string GetSysUserPowerList()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["PowerList"] == null)
            {
                return "";// "1,2,4,5,6,42,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,26,27,28,30,31,32,33,35,36,37,38,43,44,49,50,51,52,53,45,46,47,48,39,40";// "";
            }
            else
            {
                return Convert.ToString(tempSession["PowerList"]);
            }

        }



        /// <summary>
        /// 获取当前登录管理员ID
        /// </summary>
        /// <returns></returns>
        public static int GetUserID()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["UserID"] == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(tempSession["UserID"]);
            }

        }

        /// <summary>
        /// 获取当前管理员的用户名
        /// </summary>
        /// <returns></returns>
        public static string GetSysUserName()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["UserName"] == null)
            {
                return "admin";
            }
            else
            {                
                return Convert.ToString(tempSession["UserName"]);
            }

        }


        /// <summary>
        /// 获取当前管理员的用户名
        /// </summary>
        /// <returns></returns>
        public static void SetSysUserName(string username)
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            tempSession["UserName"] = username;
        }

        /// <summary>
        /// 获取当前管理员的真实姓名
        /// </summary>
        /// <returns></returns>
        public static string GetSysUserTureName()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["TrueName"] == null)
            {
                return "admin";
            }
            else
            {               
                return Convert.ToString(tempSession["TrueName"]);
            }

        }

        /// <summary>
        /// 清除Session
        /// </summary>
        public static void ClearSession()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            tempSession.Clear();
        }




        /// <summary>
        /// 获取当前管理员的部门号
        /// </summary>
        /// <returns></returns>
        public static int GetPartID()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
           
           if (tempSession["PartID"] == null)
           {
               //TOMOdify By TMS

               return -1;//-1注意需要修改
           }
           else
           {
               return Convert.ToInt32(tempSession["PartID"]);
           }
        }



        /// <summary>
        /// 重读获取当前管理员的部门号,用于此用户失败的情况
        /// </summary>
        /// <returns></returns>
        public static int ReloadPartID()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            int curUser = GetUserID();
            if (curUser > 0)
            {
                TbSysUser userItem = new SysUserDAO().GetByID(curUser);
                tempSession["PartID"] = userItem.IPart;

                return Convert.ToInt32(tempSession["PartID"]);

            }
            return -1;
           
        }



        /// <summary>
        /// 设置指标数组
        /// </summary>
        /// <returns></returns>
        public static void SetIndiactorArray(int[] indiactorArray)
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            tempSession["indiactorArray"] = indiactorArray;
        }

        /// <summary>
        /// 获取指标数组
        /// </summary>
        /// <returns></returns>
        public static int [] GetIndiactorArray()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["indiactorArray"] == null)
            {
                return null;
            }
            else
            {
                return tempSession["indiactorArray"] as int[];
            }

        }



        /// <summary>
        /// 设置地区数组
        /// </summary>
        /// <returns></returns>
        public static void SetAreaArray(int[] areaArray)
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            tempSession["areaArray"] = areaArray;
        }

        /// <summary>
        /// 获取获取数组
        /// </summary>
        /// <returns></returns>
        public static int[] GetAreaArray()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["areaArray"] == null)
            {
                return null;
            }
            else
            {
                return tempSession["areaArray"] as int[];
            }

        }



        /// <summary>
        /// 设置时间数组
        /// </summary>
        /// <returns></returns>
        public static void SetTimeArray(int[] timeArray)
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            tempSession["timeArray"] = timeArray;
        }

        /// <summary>
        /// 获取时间数组
        /// </summary>
        /// <returns></returns>
        public static int[] GetTimeArray()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["timeArray"] == null)
            {
                return null;
            }
            else
            {
                return tempSession["timeArray"] as int[];
            }

        }



    }
}