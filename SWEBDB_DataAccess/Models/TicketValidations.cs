//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SWEBDB_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TicketValidations
    {
        public long id { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public string TicketId { get; set; }
        public short ValidationId { get; set; }
        public long UserId { get; set; }
        public bool IsValid { get; set; }
        public bool IsApplied { get; set; }
    }
}
