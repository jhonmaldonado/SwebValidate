using PARKDB_DataAccess.Models;
using SWEBDB_DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwebValidate.Models
{
    public class UserManagement
    {
        static UserManagement uv = null;

        public static UserManagement getInstance()
        {
            if (uv == null)
            {
                uv = new UserManagement();
                return uv;
            }
            else
            {
                return uv;
            }
        }

        //public List<SWEBDB_DataAccess.Models.Users> GetUsers(int RoleId)
        //{
        //    List<SWEBDB_DataAccess.Models.Users> users = null;
        //    try
        //    {
        //        using (var db = new SWEBDB_Entities())
        //        {
        //            db.Database.Connection.Open();

        //            //var users1 = db.Users.Include("Roles").ToList();
        //            users = new List<SWEBDB_DataAccess.Models.Users>();
        //            users = db.Users.Include("Roles").Where(u => u.RoleId >= RoleId).ToList();

        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return users;
        //}

        public List<Users> GetUsers(int RoleId)
        {
            List<Users> users = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    //var users1 = db.Users.Include("Roles").ToList();
                    users = new List<Users>();

                    var users1 = db.Users.Include("Roles").Where(u => u.RoleId >= RoleId).ToList();
                    //users = users1.ToList();

                    //IEnumerable<SWEBDB_DataAccess.Models.Users> users2 = db.Users.Include("Roles").Where(u => u.RoleId >= RoleId).ToList();
                    foreach(SWEBDB_DataAccess.Models.Users u in users1)
                    {
                        users.Add(new Users
                        {
                            id = u.id,
                            User = u.User,
                            Password = u.Password,
                            RoleId = (int)u.RoleId,
                            Roles = new List<Roles> { new Roles { id = u.Roles.id, Role = u.Roles.Role } },
                            AllowedValidations = (int)u.Roles.AllowedValidations
                        });
                    }
                    //users = users2.ToList();
                }
            }
            catch (Exception e)
            {

            }
            return users;
        }

        public Users GetUser(int id)
        {
            Users user = new Users();
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    var dbUser = db.Users.Include("Roles").Where(u => u.id == id).FirstOrDefault();
                    user.id = dbUser.id;
                    user.User = dbUser.User;
                    user.Password = dbUser.Password;
                    user.RoleId = (int)dbUser.RoleId;
                    user.Roles = new List<Roles> { new Roles { id = dbUser.Roles.id, Role = dbUser.Roles.Role, AllowedValidations = (int)dbUser.Roles.AllowedValidations } };
                }
            }
            catch (Exception e)
            {

            }
            return user;
        }

        public List<Roles> GetRoles(int RoleId)
        {
            List<Roles> roles = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    roles = new List<Roles>();
                    var lists = db.Roles.Where(u => u.id >= RoleId).ToList();

                    foreach (SWEBDB_DataAccess.Models.Roles r in lists)
                    {
                        roles.Add(new Roles
                        {
                            id = r.id,
                            Role = r.Role,
                            AllowedValidations = (int)r.AllowedValidations
                        });
                    }
                }
            }
            catch (Exception e)
            {

            }
            return roles;
        }

        public void Create(Users u)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    db.Users.Add(new SWEBDB_DataAccess.Models.Users
                    {
                        User = u.User,
                        Password = u.Password,
                        RoleId = u.RoleId
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public void Update(Users u)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    var dbUser = db.Users.Where(v => v.id == u.id).FirstOrDefault();
                    dbUser.User = u.User;
                    dbUser.Password = u.Password;
                    dbUser.RoleId = u.RoleId;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public List<UserValidations> GetUserValidations(Int64 UserId)
        {
            List<UserValidations> userValidations = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    userValidations = new List<UserValidations>();
                    var lists = db.UserValidations.Where(u => u.UserId == UserId && u.IsValid == true).ToList();

                    using (var dbParkDB = new PARK_DBEntities())
                    {
                        dbParkDB.Database.Connection.Open();

                        var Verg = dbParkDB.VERG
                            .Select(v => new Validations { validationId = v.VergNr, validation = v.Bez })
                                .ToList();

                        foreach (SWEBDB_DataAccess.Models.UserValidations r in lists)
                        {
                            userValidations.Add(new UserValidations
                            {
                                id = r.id,
                                UserId = r.UserId,
                                ValidationId = r.ValidationId,
                                Validation = Verg.Where(v => v.validationId == r.ValidationId).Select(v => v.validation).First(),
                                IsValid = r.IsValid
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return userValidations;
        }

        public void CreateValidation(Int64 UserId, Int16 ValidationId)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    db.UserValidations.Add(new SWEBDB_DataAccess.Models.UserValidations
                    {
                        UserId = UserId,
                        ValidationId = ValidationId,
                        IsValid = true
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public UserValidations GetUserValidation(int id)
        {
            UserValidations userV = new UserValidations();
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    var dbUser = db.UserValidations.Where(u => u.id == id).FirstOrDefault();
                    if (dbUser != null)
                    {
                        userV.id = dbUser.id;
                        userV.UserId = dbUser.UserId;
                        userV.ValidationId = dbUser.ValidationId;
                        userV.IsValid = dbUser.IsValid;
                    }
                }
            }
            catch (Exception e)
            {

            }
            return userV;
        }

        public void UpdateUserValidation(UserValidations uv)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    var dbUserV = db.UserValidations.Where(v => v.id == uv.id).FirstOrDefault();
                    dbUserV.ValidationId = uv.ValidationId;
                    dbUserV.IsValid = uv.IsValid;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public IEnumerable<Validations> GetValidations()
        {
            IEnumerable<Validations> Val = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    Val = db.ValidationPlans
                        .Select(v => new Validations { validationId = v.ValidationId, validation = v.Customer })
                        .ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Val;
        }
    }
}