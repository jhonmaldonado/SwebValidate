using SwebValidate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SwebValidate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "sweb Validate";

            return RedirectToAction("Login", "Home");
            //return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "sweb Validate";
            ViewBag.Check = false;
            login lg = new login();
            return View(lg);
        }

        [HttpPost]
        public ActionResult Login(login lg)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(lg.User, false);

                //LP.PlateNo = LP.PlateNo.ToUpper();
                //TI = TicketValidation.getInstance().BookValidation(TI);
                LoginValidation.getInstance().Login(lg);

                if (lg.Message == "OK")
                {
                    TempData["SessionId"] = lg.SessionId;
                    TempData["User"] = lg.User;
                    TempData["UserId"] = lg.UserId;
                    TempData["RoleId"] = lg.RoleId;

                    return RedirectToAction("GetTicketInfo", "TicketValidation");
                }
                else
                {
                    TempData["SessionId"] = null; TempData["User"] = null; TempData["UserId"] = null;
                    TempData["msg"] = "<script>alert('" + lg.Message + "');</script>";
                }
            }
            //ViewBag.Check = TI.ValidationApplied;
            return View(lg);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            LoginValidation.getInstance().Logout(Int64.Parse(TempData["SessionId"].ToString()));
            TempData["SessionId"] = null; TempData["User"] = null; TempData["UserId"] = null;

            return RedirectToAction("Login", "Home");
        }
    }
}
