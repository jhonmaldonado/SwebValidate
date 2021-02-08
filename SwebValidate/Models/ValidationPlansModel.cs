using PARKDB_DataAccess.Models;
using SWEBDB_DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwebValidate.Models
{
    public class ValidationPlansModel
    {
        static ValidationPlansModel vp = null;

        public static ValidationPlansModel getInstance()
        {
            if (vp == null)
            {
                vp = new ValidationPlansModel();
                return vp;
            }
            else
            {
                return vp;
            }
        }

        public List<ValidationPlans> GetValidationPlans()
        {
            List<ValidationPlans> vp = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    
                    vp = new List<ValidationPlans>();

                    var ValPlan = db.ValidationPlans.ToList();

                    using (var dbParkDB = new PARK_DBEntities())
                    {
                        dbParkDB.Database.Connection.Open();

                        var Verg = dbParkDB.VERG
                            .Select(v => new Validations { validationId = v.VergNr, validation = v.Bez })
                                .ToList();

                        foreach (SWEBDB_DataAccess.Models.ValidationPlans vps in ValPlan)
                        {
                            vp.Add(new ValidationPlans
                            {
                                id = vps.id,
                                ValidationId = vps.ValidationId,
                                Customer = vps.Customer,
                                MinutePlan = vps.MinutePlan,
                                IsValid = vps.IsValid,
                                Remarks = vps.Remarks,
                                Validation = Verg.Where(v => v.validationId == vps.ValidationId).Select(v => v.validation).First()
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return vp;
        }

        public void Create(ValidationPlans vp)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    db.ValidationPlans.Add(new SWEBDB_DataAccess.Models.ValidationPlans
                    {
                        ValidationId = vp.ValidationId,
                        Customer = vp.Customer,
                        MinutePlan = vp.MinutePlan,
                        Remarks = vp.Remarks,
                        IsValid = vp.IsValid
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public ValidationPlans GetValidationPlan(Int64 id)
        {
            ValidationPlans vp = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    vp = new ValidationPlans();

                    var ValPlan = db.ValidationPlans.Where(u => u.id == id).FirstOrDefault();

                    vp.id = ValPlan.id;
                    vp.ValidationId = ValPlan.ValidationId;
                    vp.Customer = ValPlan.Customer;
                    vp.MinutePlan = ValPlan.MinutePlan;
                    vp.IsValid = ValPlan.IsValid;
                    vp.Remarks = ValPlan.Remarks;
                }
            }
            catch (Exception e)
            {

            }
            return vp;
        }

        public void Update(ValidationPlans vp)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    var dbVP = db.ValidationPlans.Where(v => v.id == vp.id).FirstOrDefault();
                    dbVP.ValidationId = vp.ValidationId;
                    dbVP.Customer = vp.Customer;
                    dbVP.MinutePlan = vp.MinutePlan;
                    dbVP.IsValid = vp.IsValid;
                    dbVP.Remarks = vp.Remarks;

                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}