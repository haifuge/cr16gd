using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RailBiding.Common
{
    public class MenuFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (filterContext.HttpContext.Session["MenuList"] != null)
            {
                filterContext.Controller.ViewBag.MainMenuList = filterContext.HttpContext.Session["MenuList"];
            }
        }
    }

    public class VerifyLoginFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session["UserId"] == null)
                filterContext.Result = new RedirectResult("/Login");
            else
            {
                filterContext.Controller.ViewBag.UserName = filterContext.HttpContext.Session["UserName"];
                filterContext.Controller.ViewBag.UserDepartment = filterContext.HttpContext.Session["UserDepartment"];
            }
        }
    }

    public class VerifyMobileLoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session["UserId"] == null)
                filterContext.Result = new RedirectResult("/MobileLogin");
            else
            {
                filterContext.Controller.ViewBag.UserName = filterContext.HttpContext.Session["UserName"];
                filterContext.Controller.ViewBag.UserDepartment = filterContext.HttpContext.Session["UserDepartment"];
            }
        }
    }

    public class ActiveMenuFilter:ActionFilterAttribute
    {
        public string MenuName { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (MenuName == null || MenuName == "")
                MenuName = "ItemC";
            filterContext.Controller.ViewBag.ActiveMenu = MenuName;
        }
    }
}