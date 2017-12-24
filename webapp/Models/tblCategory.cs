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
    
    public partial class tblCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCategory()
        {
            this.tblcompanyCategories = new HashSet<tblcompanyCategory>();
            this.tblExpectedBudgetLines = new HashSet<tblExpectedBudgetLine>();
            this.tblRealBudgets = new HashSet<tblRealBudget>();
            this.tblKeyPointBudgetLines = new HashSet<tblKeyPointBudgetLine>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public int budgetTypeId { get; set; }
        public int parentId { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<bool> isActive { get; set; }
    
        public virtual tblBudgetType tblBudgetType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblcompanyCategory> tblcompanyCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblExpectedBudgetLine> tblExpectedBudgetLines { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblRealBudget> tblRealBudgets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblKeyPointBudgetLine> tblKeyPointBudgetLines { get; set; }
    }
}
