using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Areas.Admin.Models
{
    public class BudgetTypeModel
    {
    }
    public class AddBudgetType
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public DateTime createDate { get; set; }
         public Boolean isACTIVE { get; set; }
    }
    public class EditBudgetType
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public DateTime createDate { get; set; }
        public Boolean isACTIVE { get; set; }
    }
}