using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SWEBDB_DataAccess.Models;
using SwebValidate.Models;

namespace SwebValidate.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserValidations()
        {
            if (TempData.ContainsKey("SessionId"))
            {
                if (string.IsNullOrEmpty(TempData["SessionId"].ToString()))
                {
                    return RedirectToAction("Login", "Home");
                }
                TempData.Keep("SessionId"); TempData.Keep("User"); TempData.Keep("UserId"); TempData.Keep("RoleId");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["userModal"] = "true"; TempData["UserId"] = 0; TempData["trValidation"] = "false"; TempData.Keep("trValidation");
            TempData["validationModal"] = "true";

            GetViewBagSource(0);
            Users u = new Users();
            u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
            return View("UserValidations", "_AdminLayout", u);
        }

        //[HttpPost]
        //public ActionResult UserValidations()
        //{
        //    return View();
        //}

        // GET: UsersModule/Create
        public ActionResult Create()
        {
            TempData["userModal"] = "false"; TempData["UserId"] = 0; TempData.Keep("trValidation");// TempData["trValidation"] = "false";
            TempData["validationModal"] = "true";

            Users u = new Users();
            u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));

            GetViewBagSource(0);
            return View("UserValidations", "_AdminLayout", u);
        }

        // POST: UsersModule/Create
        [HttpPost]
        public ActionResult Create(Users u)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    //TempData["userModal"] = "true";
                    UserManagement.getInstance().Create(u);
                }
                //else
                //{
                //    TempData["userModal"] = "false";
                //}
                TempData["userModal"] = ModelState.IsValid.ToString().ToLower(); TempData["UserId"] = 0;
                TempData.Keep("trValidation"); TempData["validationModal"] = "true";

                GetViewBagSource(0);
                u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
                return View("UserValidations", "_AdminLayout", u);
            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: UsersModule/Edit/5
        public ActionResult Edit(int id)
        {
            Users u = UserManagement.getInstance().GetUser(id);// new Users();
            TempData["userModal"] = "false"; TempData["UserId"] = id; TempData.Keep("UserId"); ViewBag.UserId = id;
            TempData.Keep("trValidation"); TempData["validationModal"] = "true";
            
            GetViewBagSource(Int64.Parse(TempData["UserId"].ToString()));
            u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
            return View("UserValidations", "_AdminLayout", u);
        }

        // POST: UsersModule/Edit/5
        [HttpPost]
        public ActionResult Edit(Users u)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    UserManagement.getInstance().Update(u);
                }
                TempData["userModal"] = ModelState.IsValid.ToString().ToLower(); TempData["UserId"] = u.id; TempData.Keep("UserId"); ViewBag.UserId = u.id;
                TempData.Keep("trValidation"); TempData["validationModal"] = "true";

                GetViewBagSource(Int64.Parse(TempData["UserId"].ToString()));

                u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
                return View("UserValidations", "_AdminLayout", u);
            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Select(int id)
        {
            try
            {
                Users u = UserManagement.getInstance().GetUser(id);
                TempData["userModal"] = "true"; TempData["UserId"] = u.id; TempData.Keep("UserId");
                ViewBag.UserId = u.id; TempData["validationModal"] = "true";

                GetViewBagSource(Int64.Parse(TempData["UserId"].ToString()));

                //u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
                
                if (ViewBag.UserValidations.Count >= u.Roles.Where(r => r.id == u.RoleId).Select(r => r.AllowedValidations).FirstOrDefault())
                {
                    TempData["trValidation"] = "false";

                    ViewBag.Validations = new Validations[] { new Validations() };
                }
                else
                {
                    TempData["trValidation"] = "true";

                    //Validation Associate - B2B - Minute Plan
                    if (u.RoleId == 4)
                    {
                        ViewBag.Validations = UserManagement.getInstance().GetValidations();
                    }

                }
                TempData.Keep("trValidation");

                return View("UserValidations", "_AdminLayout", u);
            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult CreateValidation(Int16 ValidationId)
        {
            TempData.Keep("UserId");
            UserManagement.getInstance().CreateValidation(Int64.Parse(TempData["UserId"].ToString()), ValidationId);
            TempData["userModal"] = "true"; TempData["validationModal"] = "true";

            GetViewBagSource(Int64.Parse(TempData["UserId"].ToString()));

            List<Users> users = ViewBag.Users;
            int av = users.Where(v => v.id == Int64.Parse(TempData["UserId"].ToString())).Select(v => v.AllowedValidations).FirstOrDefault();
            if (ViewBag.UserValidations.Count >= av)
            {
                TempData["trValidation"] = "false";
            }
            else
            {
                TempData["trValidation"] = "true";
            }
            TempData.Keep("trValidation");

            Users u = new Users();
            u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
            
            return View("UserValidations", "_AdminLayout", u);
        }

        public ActionResult UpdateValidation(int id)
        {
            TempData["validationModal"] = "false";

            TempData["userModal"] = "true"; TempData.Keep("UserId"); TempData.Keep("trValidation");// TempData["trValidation"] = "false";
            ViewBag.UserId = TempData["UserId"];
            GetViewBagSource(Int64.Parse(TempData["UserId"].ToString()));

            ViewBag.UserValidation = UserManagement.getInstance().GetUserValidation(id);

            //Users u = new Users();
            Users u = UserManagement.getInstance().GetUser(int.Parse(TempData["UserId"].ToString()));
            u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
            if (u.RoleId == 4)
            {
                ViewBag.Validations = UserManagement.getInstance().GetValidations();
            }
            return View("UserValidations", "_AdminLayout", u);
        }

        [HttpPost]
        public ActionResult UpdateValidation(UserValidations uv)
        {
            TempData["validationModal"] = "true";
            UserManagement.getInstance().UpdateUserValidation(uv);

            TempData["userModal"] = "true"; TempData.Keep("UserId"); TempData.Keep("trValidation");// TempData["trValidation"] = "false";
            ViewBag.UserId = TempData["UserId"];
            GetViewBagSource(Int64.Parse(TempData["UserId"].ToString()));
            Users u = new Users();
            u.Roles = UserManagement.getInstance().GetRoles(int.Parse(TempData["RoleId"].ToString()));
            return View("UserValidations", "_AdminLayout", u);
        }

        private void GetViewBagSource(Int64 UserId)
        {
            TempData.Keep("SessionId"); TempData.Keep("User"); TempData.Keep("UserId"); TempData.Keep("RoleId");

            ViewBag.Users = UserManagement.getInstance().GetUsers(int.Parse(TempData["RoleId"].ToString()));
            ViewBag.UserValidations = UserManagement.getInstance().GetUserValidations(UserId);
            ViewBag.Validations = TicketValidation.getInstance().GetValidations();
            ViewBag.UserValidation = UserManagement.getInstance().GetUserValidation(0);
        }
    }
}