using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwebValidate.Models
{
    public class TicketInfo
    {
        [Required(ErrorMessage = "Favor escanear o digitar número de tiquete")]
        [StringLength(100, ErrorMessage = "El número de {0} debe tener mínimo {2} caracteres.", MinimumLength = 6)]
        [Range(0, int.MaxValue, ErrorMessage = "Por favor ingresar solo números")]
        [Display(Name = "Tiquete")]
        public string TicketId { get; set; }

        public string Message { get; set; } = "";
        public Int16 validationId { get; set; }
        public Boolean ValidationApplied { get; set; }
        public Int64 UserId { get; set; }

        public IEnumerable<Validations> Validations { get; set; } //= TicketValidation.getInstance().GetValidations();

        public IEnumerable<HttpPostedFileBase> file { get; set; }
    }

    public class Validations
    {
        public Int16 validationId { get; set; }
        public string validation { get; set; }
    }

    public class login
    {
        public Int64 SessionId { get; set; }

        [Required(ErrorMessage = "Favor digitar Usuario")]
        [StringLength(50, ErrorMessage = "El {0} debe tener mínimo {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Usuario")]
        public string User { get; set; }

        [Required(ErrorMessage = "Favor digitar Contraseña")]
        [StringLength(50, ErrorMessage = "La {0} debe tener mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public string Message { get; set; } = "";

        public Int64 UserId { get; set; }

        public int RoleId { get; set; }
    }

    public class Users
    {
        public Int64 id { get; set; }

        [Required(ErrorMessage = "Favor digitar Usuario")]
        [StringLength(50, ErrorMessage = "El {0} debe tener mínimo {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Usuario")]
        public string User { get; set; } = "";

        [Required(ErrorMessage = "Favor digitar Contraseña")]
        [StringLength(50, ErrorMessage = "La {0} debe tener mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = "";

        public int RoleId { get; set; }

        public IEnumerable<Roles> Roles { get; set; }

        public Int32 AllowedValidations { get; set; }
    }

    public class Roles
    {
        public Int64 id { get; set; }

        [Required(ErrorMessage = "Favor digitar Rol")]
        [StringLength(50, ErrorMessage = "El {0} debe tener mínimo {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Rol")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Favor digitar Validaciones Permitidas")]
        //[StringLength(100, ErrorMessage = "El número de {0} debe tener mínimo {2} caracter.", MinimumLength = 1)]
        //[Range(0, int.MaxValue, ErrorMessage = "Por favor ingresar solo números")]
        //[Display(Name = "Validaciones Permitidas")]
        public Int32 AllowedValidations { get; set; }
    }

    public class UserValidations
    {
        public Int64 id { get; set; }
        public Int64 UserId { get; set; }
        public Int16 ValidationId { get; set; }
        public string Validation { get; set; }
        public Boolean IsValid { get; set; }

        //public List<Validations> Validations { get; set; }
    }

    public class ValidationPlans
    {
        public Int64 id { get; set; }
        public Int16 ValidationId { get; set; }
        public string Validation { get; set; }

        [Required(ErrorMessage = "Favor digitar Cliente")]
        [StringLength(50, ErrorMessage = "El {0} debe tener mínimo {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Cliente")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Favor indicar minutos del plan.")]
        public Int32 MinutePlan { get; set; }

        public Boolean IsValid { get; set; } = true;
        public string Remarks { get; set; }

        public IEnumerable<Validations> Validations { get; set; }
    }

    public class TicketValidations
    {
        public Int64 id { get; set; }
        public DateTime Timestamp { get; set; }
        public string TicketId { get; set; }
        public Int16 ValidationId { get; set; }
        public string Validation { get; set; }
        public Int64 UserId { get; set; }
        public string User { get; set; }
        public string IsValid { get; set; }
        public string IsApplied { get; set; }
    }
}