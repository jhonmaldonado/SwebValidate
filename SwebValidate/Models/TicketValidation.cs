using PARKDB_DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using FDICODB_DataAccess.Models;
using SWEBDB_DataAccess.Models;

namespace SwebValidate.Models
{
    public class TicketValidation
    {
        static TicketValidation tv = null;

        public static TicketValidation getInstance()
        {
            if (tv == null)
            {
                tv = new TicketValidation();
                return tv;
            }
            else
            {
                return tv;
            }
        }

        public TicketInfo BookValidation(TicketInfo TI)
        {
            int KartenNr = int.Parse(TI.TicketId.ToString());
            DateTime fchConsulta = DateTime.Now.AddDays(-1);
            Boolean TicketExist = false;
            using (var db = new PARK_DBEntities())
            {
                db.Database.Connection.Open();

                if (db.SDKARTEBEWEGUNG.Any(sdkb => sdkb.KarteKartenNr == KartenNr && sdkb.Ztpkt > fchConsulta))
                {
                    TicketExist = true;
                }
                else
                {
                    TI.ValidationApplied = false;
                    TI.Message = "Favor revisar # tiquete, no se encuentra entrada asociada.";
                }
            }

            if (TicketExist)
            {
                //using (var db = new FDICO_DBEntities())
                //{
                //    db.Database.Connection.Open();

                //    if (!db.ValidationsST.Any(v => v.ticketId == TI.TicketId && v.isApplied == false && v.dateTx > fchConsulta))
                //    {
                //        db.ValidationsST.Add(new ValidationsST
                //        {
                //            dateTx = DateTime.Now,
                //            ticketId = TI.TicketId,
                //            providerNo = TI.validationId,
                //            deviceNo = 0,
                //            isApplied = false
                //        });
                //        db.SaveChanges();
                //    }

                //    TI.ValidationApplied = true;
                //    TI.Message = "Validación registrada.";
                //}

                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    
                    if(!db.TicketValidations.Any(tv=>tv.TicketId == TI.TicketId && tv.IsApplied==false && tv.Timestamp > fchConsulta))
                    {
                        db.TicketValidations.Add(new SWEBDB_DataAccess.Models.TicketValidations
                        {
                            Timestamp = DateTime.Now,
                            TicketId = TI.TicketId,
                            ValidationId = TI.validationId,
                            UserId = TI.UserId,
                            IsValid = true,
                            IsApplied = false
                        });
                        db.SaveChanges();
                    }

                    TI.ValidationApplied = true;
                    TI.Message = "Validación registrada.";
                }
            }
            
            return TI;
        }

        public IEnumerable<Validations> GetValidations()
        {
            IEnumerable<Validations> Val = null;
            try
            {
                using (var db = new PARK_DBEntities())
                {
                    db.Database.Connection.Open();

                    Val = db.VERG
                        .Select(v => new Validations { validationId = v.VergNr, validation = v.Bez })
                        .ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Val;
        }

        public IEnumerable<Validations> GetUserValidations(Int64 UserId)
        {
            IEnumerable<Validations> Val = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    var uv = db.UserValidations
                        .Where(u => u.UserId == UserId && u.IsValid == true)
                        .ToList();

                    using (var dbParkDB = new PARK_DBEntities())
                    {
                        dbParkDB.Database.Connection.Open();

                        var Verg = dbParkDB.VERG
                            .Select(v => new Validations { validationId = v.VergNr, validation = v.Bez })
                                .ToList();

                        Val = (from uv1 in uv
                               join v1 in Verg on uv1.ValidationId equals v1.validationId
                               select new Validations
                               {
                                   validationId = uv1.ValidationId,
                                   validation = v1.validation
                               }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Val;
        }

        public List<TicketValidations> TracingTicketValidations(DateTime timeStamp)
        {
            List<TicketValidations> traceTV = new List<TicketValidations>();
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    DateTime endDate = timeStamp.AddDays(1);
                    var TicketVal = db.TicketValidations.Include("Users").Where(tv => tv.Timestamp >= timeStamp && tv.Timestamp < endDate).ToList();

                    using (var dbParkDB = new PARK_DBEntities())
                    {
                        dbParkDB.Database.Connection.Open();

                        var Verg = dbParkDB.VERG
                            .Select(v => new Validations { validationId = v.VergNr, validation = v.Bez })
                                .ToList();

                        foreach(SWEBDB_DataAccess.Models.TicketValidations ticket in TicketVal)
                        {
                            traceTV.Add(new TicketValidations
                            {
                                id = ticket.id,
                                Timestamp = (DateTime)ticket.Timestamp,
                                TicketId = ticket.TicketId,
                                ValidationId = ticket.ValidationId,
                                Validation = Verg.Where(v => v.validationId == ticket.ValidationId).Select(v => v.validation).First(),
                                UserId = ticket.UserId,
                                //User = ticket.Users.User,
                                IsValid = ticket.IsValid?"Si":"No",
                                IsApplied = ticket.IsApplied ? "Si" : "No"
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return traceTV;
        }
    }
}