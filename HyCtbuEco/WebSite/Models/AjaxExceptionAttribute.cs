using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HyCtbuEco.Models
{
    #region AjaxExceptionAttribute
    /// <summary>
    /// Ajax Exception Handle Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AjaxExceptionAttribute : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            filterContext.Result = AjaxError(filterContext.Exception.Message, filterContext);

            //Let the system know that the exception has been handled
            filterContext.ExceptionHandled = true;
        }

        /// <summary>
        /// Ajaxes the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>JsonResult</returns>
        protected JsonResult AjaxError(string message, ExceptionContext filterContext)
        {
            //If message is null or empty, then fill with generic message
            if (String.IsNullOrEmpty(message))
                message = "出现错误！请刷新本页面";

            //Set the response status code to 500
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //Needed for IIS7.0
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            return new JsonResult
            {
                //can extend more properties 
                Data = new AjaxExceptionModel() { ErrorMessage = message },
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }
    }
    #endregion

    /// <summary>
    /// AjaxExceptionModel
    /// </summary>
    public class AjaxExceptionModel
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}