using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace HyCtbuEco.Models
{
    public class GuestUserInfo
    {


        /// <summary>
        /// 获取当前登录访客的ID
        /// </summary>
        /// <returns></returns>
        public static int GetGuestID()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            if (tempSession["GuestID"] == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(tempSession["GuestID"]);
            }

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
        public static int[] GetIndiactorArray()
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



        /// <summary>
        /// 清除Session
        /// </summary>
        public static void ClearSession()
        {
            HttpSessionState tempSession = System.Web.HttpContext.Current.Session;
            tempSession.Clear();
        }


    }
}