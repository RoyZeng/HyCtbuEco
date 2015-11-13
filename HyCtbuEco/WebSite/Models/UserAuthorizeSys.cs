using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HyCtbuEco.Services;
using System.Web.Security;

using System.Web.Mvc;
using System.Net;
using HyCtbuEco.Models;
namespace HyCtbuEco.Services
{

    //http://www.cnblogs.com/JustRun1983/p/3377652.html
    //http://www.cnblogs.com/JustRun1983/p/3279139.html

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UserAuthorizeSys : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SysUserInfo.GetUserID() < 1)
            {
                //未认证
                var redirectUrl = "/Login/SysRootUser?RedirectPath=" + filterContext.HttpContext.Request.Url;


                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //非ajax
                    filterContext.Result = new RedirectResult(redirectUrl);
                }
                else
                {
                    //ajax操作
                    //直接返回403，然后客户端判断，并添加SessionTimeout头
                    filterContext.HttpContext.Response.AddHeader("SessionTimeout", "true");
                    filterContext.Result = new HttpStatusCodeResult(403, "您没有该权限，请登录！");
                    
                };

                return;


            }
        }

    }
}