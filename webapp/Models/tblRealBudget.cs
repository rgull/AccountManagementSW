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
    
    public partial class tblRealBudget
    {
        public int id { get; set; }
        public int budgetTypeId { get; set; }
        public int categoryId { get; set; }
        public System.DateTime date { get; set; }
        public decimal budget { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> companyId { get; set; }
    
        public virtual tblBudgetType tblBudgetType { get; set; }
        public virtual tblCategory tblCategory { get; set; }
        public virtual tblCompany tblCompany { get; set; }
    }
}