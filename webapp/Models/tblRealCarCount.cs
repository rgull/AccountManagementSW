//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartAdminMvc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblRealCarCount
    {
        public int id { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> carCounts { get; set; }
        public Nullable<int> compnayId { get; set; }
    
        public virtual tblCompany tblCompany { get; set; }
    }
}
