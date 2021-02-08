using SWEBDB_DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwebValidate.Models
{
    public class RolesMng
    {
        static RolesMng rm = null;

        public static RolesMng getInstance()
        {
            if (rm == null)
            {
                rm = new RolesMng();
                return rm;
            }
            else
            {
                return rm;
            }
        }

        public IEnumerable<SWEBDB_DataAccess.Models.Roles> GetRoles()
        {
            IEnumerable<SWEBDB_DataAccess.Models.Roles> roles = null;
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    roles = new List<SWEBDB_DataAccess.Models.Roles>();
                    roles = db.Roles.ToList();
                }
            }
            catch (Exception e)
            {

            }
            return roles;
        }

        public Roles GetRole(int id)
        {
            Roles role = new Roles();
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    var dbRole = db.Roles.Where(u => u.id == id).FirstOrDefault();
                    role.id = dbRole.id;
                    role.Role = dbRole.Role;
                    role.AllowedValidations = (int)dbRole.AllowedValidations;
                }
            }
            catch (Exception e)
            {

            }
            return role;
        }

        public void Create(Roles r)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();
                    db.Roles.Add(new SWEBDB_DataAccess.Models.Roles
                    {
                        Role = r.Role,
                        AllowedValidations = r.AllowedValidations
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public void Update(Roles r)
        {
            try
            {
                using (var db = new SWEBDB_Entities())
                {
                    db.Database.Connection.Open();

                    var dbRole = db.Roles.Where(v => v.id == r.id).FirstOrDefault();
                    dbRole.Role = r.Role;
                    dbRole.AllowedValidations = r.AllowedValidations;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}