using TMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HyCtbuEco2015.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult SysRootUser()
        {
            return View();
        }


        public ActionResult GetValidateCode()
        {

            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }


        public ActionResult Version()
        {
            return View();
        }




        /// <summary>
        /// 访客登录验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGuestValidateCode()
        {

            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            Session["GuestValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }



        /// <summary>
        /// 访客登陆
        /// </summary>
        /// <returns></returns>
        public ActionResult Guester() 
        {

            return View();

        }


    }
}
