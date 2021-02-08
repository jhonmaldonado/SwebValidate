using SwebValidate.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SwebValidate.Controllers
{
    public class TicketValidationController : Controller
    {
        // GET: TicketValidation
        public ActionResult GetTicketInfo()
        {
            if (TempData.ContainsKey("SessionId"))
            {
                if (string.IsNullOrEmpty(TempData["SessionId"].ToString()))
                {
                    return RedirectToAction("Login", "Home");
                }
                KeepTempData();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.Title = "Ticket Validation";
            ViewBag.Check = false;

            TicketInfo TI = new TicketInfo();
            if(TempData["RoleId"].ToString() == "1")
            {
                TI.Validations = TicketValidation.getInstance().GetValidations();
            }
            else
            {
                TI.Validations = TicketValidation.getInstance().GetUserValidations(Int64.Parse(TempData["UserId"].ToString()));
            }
             //(IEnumerable<Validations>)UserManagement.getInstance().GetUserValidations(Int64.Parse(TempData["UserId"].ToString()));

            if (TempData["RoleId"].ToString() == "1" || TempData["RoleId"].ToString() == "2")
            {
                return View("GetTicketInfo", "_AdminLayout", TI);
            }

            return View(TI);
        }

        [HttpPost]
        public ActionResult GetTicketInfo(TicketInfo TI)
        {
            if (ModelState.IsValid)
            {
                if (TI.validationId == 0)
                {
                    TI.Message = "Favor seleccionar validación.";
                }
                else
                {
                    TI.UserId = Int64.Parse(TempData["UserId"].ToString());
                    TI = TicketValidation.getInstance().BookValidation(TI);
                }
            }
            else
            {
                TI.Message = "Favor escanear o digitar número de tiquete.";
            }
            
            ViewBag.Check = TI.ValidationApplied;
            TempData["msg"] = "<script>alert('" + TI.Message + "');</script>";
            
            if (TempData["RoleId"].ToString() == "1")
            {
                TI.Validations = TicketValidation.getInstance().GetValidations();
            }
            else
            {
                TI.Validations = TicketValidation.getInstance().GetUserValidations(Int64.Parse(TempData["UserId"].ToString()));
            }

            KeepTempData();

            if (TempData["RoleId"].ToString() == "1" || TempData["RoleId"].ToString() == "2")
            {
                return View("GetTicketInfo", "_AdminLayout", TI);
            }

            return View(TI);
        }

        public string BarcodeScan()
        {//HttpPostedFileBase file
            string Barcode = "No se pudo leer código de barras, por favor digite 6 últimos digitos del tiquete de parqueadero.";
            try
            {
                string path = "";
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/Content/Barcode"), fileName);
                    file.SaveAs(path);
                }
            }
            catch (Exception ex)
            {

            }
            return Barcode;
        }

        public ActionResult TracingTicketValidations()
        {
            if (TempData.ContainsKey("SessionId"))
            {
                if (string.IsNullOrEmpty(TempData["SessionId"].ToString()))
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            KeepTempData();

            TempData["fchConsulta"] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (Request.Params["value"] != null)
            {
                TempData["fchConsulta"] = Request.Params["value"];
            }
            ViewBag.ticketValidations = TicketValidation.getInstance().TracingTicketValidations(DateTime.Parse(TempData["fchConsulta"].ToString()));


            return View("TracingTicketValidations", "_AdminLayout");
        }

        private void KeepTempData()
        {
            TempData.Keep("SessionId"); TempData.Keep("User"); TempData.Keep("UserId"); TempData.Keep("RoleId");
        }
    }
}