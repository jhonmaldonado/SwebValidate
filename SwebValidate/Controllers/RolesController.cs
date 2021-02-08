using SwebValidate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwebValidate.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult prmRoles()
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
            TempData["roleModal"] = "true";
            GetViewBagSource();

            Roles r = new Roles();
            return View("prmRoles", "_AdminLayout", r);
        }

        private void GetViewBagSource()
        {
            TempData.Keep("SessionId"); TempData.Keep("User"); TempData.Keep("UserId"); TempData.Keep("RoleId");

            ViewBag.Roles = RolesMng.getInstance().GetRoles();
        }

        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            TempData["roleModal"] = "false";

            Roles r = new Roles();

            GetViewBagSource();
            return View("prmRoles", "_AdminLayout", r);
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(Roles r)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    //TempData["userModal"] = "true";
                    RolesMng.getInstance().Create(r);
                }

                TempData["roleModal"] = ModelState.IsValid.ToString().ToLower();
                GetViewBagSource();
                return View("prmRoles", "_AdminLayout", r);
            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {
            Roles r = RolesMng.getInstance().GetRole(id);// new Users();
            TempData["roleModal"] = "false";

            GetViewBagSource();
            return View("prmRoles", "_AdminLayout", r);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(Roles r)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    RolesMng.getInstance().Update(r);
                }
                TempData["roleModal"] = ModelState.IsValid.ToString().ToLower();

                GetViewBagSource();
                return View("prmRoles", "_AdminLayout", r);
            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
