using SwebValidate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwebValidate.Controllers
{
    public class ValidationPlansController : Controller
    {
        // GET: ValidationPlans
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ValidationPlans()
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

            TempData["vpModal"] = "true";
            GetViewBagSource();

            ValidationPlans vp = new ValidationPlans();
            vp.Validations = TicketValidation.getInstance().GetValidations();
            return View("ValidationPlans", "_AdminLayout", vp);
        }

        private void GetViewBagSource()
        {
            TempData.Keep("SessionId"); TempData.Keep("User"); TempData.Keep("UserId"); TempData.Keep("RoleId");

            ViewBag.ValidationPlans = ValidationPlansModel.getInstance().GetValidationPlans();
            ViewBag.Validations = TicketValidation.getInstance().GetValidations();
        }

        // GET: ValidationPlans/Create
        public ActionResult Create()
        {
            TempData["vpModal"] = "false";

            ValidationPlans vp = new ValidationPlans();
            vp.Validations = TicketValidation.getInstance().GetValidations();

            GetViewBagSource();
            return View("ValidationPlans", "_AdminLayout", vp);
        }

        // POST: ValidationPlans/Create
        [HttpPost]
        public ActionResult Create(ValidationPlans vp)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    ValidationPlansModel.getInstance().Create(vp);
                }
                TempData["vpModal"] = ModelState.IsValid.ToString().ToLower();

                vp.Validations = TicketValidation.getInstance().GetValidations();

                GetViewBagSource();
                return View("ValidationPlans", "_AdminLayout", vp);
            }
            catch
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: ValidationPlans/Edit/5
        public ActionResult Edit(int id)
        {
            TempData["vpModal"] = "false";
            ValidationPlans vp = ValidationPlansModel.getInstance().GetValidationPlan(id);
            vp.Validations = TicketValidation.getInstance().GetValidations();

            GetViewBagSource();
            return View("ValidationPlans", "_AdminLayout", vp);
        }

        // POST: ValidationPlans/Edit/5
        [HttpPost]
        public ActionResult Edit(ValidationPlans vp)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    ValidationPlansModel.getInstance().Update(vp);
                }
                TempData["vpModal"] = ModelState.IsValid.ToString().ToLower();

                vp.Validations = TicketValidation.getInstance().GetValidations();

                GetViewBagSource();
                return View("ValidationPlans", "_AdminLayout", vp);
            }
            catch
            {
                return View();
            }
        }

        // GET: ValidationPlans/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ValidationPlans/Delete/5
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
