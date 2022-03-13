using ScholarPortal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScholarPortal.Filters
{
    public class ErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception.InnerException;
            string errmsg = filterContext.Exception.Message;
            string stacktrace = filterContext.Exception.StackTrace;

            CustomExchange ce = new CustomExchange();
            ce.ErrorLog(errmsg, ex, stacktrace);

            filterContext.ExceptionHandled = true;
            //var model = new HandleErrorInfo(filterContext.Exception, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString());
            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "Error",
            //    ViewData = new ViewDataDictionary(model)
            //};
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = "error",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new ContentResult() { Content = "error" };
            }

            return;
        }
    }
}