using Newtonsoft.Json.Linq;
using SWEBDB_DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwebValidate.Models
{
    public class LoginValidation
    {
        static LoginValidation lg = null;

        public static LoginValidation getInstance()
        {
            if (lg == null)
            {
                lg = new LoginValidation();
                return lg;
            }
            else
            {
                return lg;
            }
        }

        public void Login(login lg)
        {
            //string message = "OK";
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    if (!db.Users.Any(u => u.User == lg.User))
                    {
                        //message = "Usuario invalido";
                        lg.Message = "Usuario invalido";
                    }
                    else
                    {
                        if (!db.Users.Any(u => u.User == lg.User && u.Password == lg.Password))
                        {
                            //message = "Contraseña invalida";
                            lg.Message = "Contraseña invalida";
                        }
                        else
                        {
                            lg.Message = "OK";
                            //message = "OK";

                            var sUsers = db.Users
                                .Where(u => u.User == lg.User)
                                .FirstOrDefault();
                            lg.RoleId = (int)sUsers.RoleId;
                            lg.UserId = sUsers.id;

                            var sSessions = db.Sessions
                                .Where(s => s.UserId == sUsers.id && s.EndDate == null)
                                .FirstOrDefault();

                            if (sSessions != null)
                            {
                                lg.SessionId = sSessions.id;
                            }
                            else
                            {
                                Sessions session = new Sessions()
                                {
                                    UserId = sUsers.id,
                                    StartDate = DateTime.Now,
                                    EndDate = null
                                };

                                db.Sessions.Add(session);
                                db.SaveChanges();

                                lg.SessionId = session.id;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                lg.Message = "Error: " + e.Message;
            }

            //JToken tAuthentication; string servicio;
            //servicio = "https://localhost:44338/Authentication";
            //tAuthentication = CommonMethods.ConsumirServicioWeb_GET_Token(servicio, 400);
            //return message;
        }

        public void Logout(Int64 SessionId)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.ExecuteSqlCommand("UPDATE Sessions SET EndDate = GETDATE() WHERE id = " + SessionId);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}