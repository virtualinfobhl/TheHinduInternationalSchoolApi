using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{

    public class ExpenseTbl
    {
        [Key]

        public int ExpenseId { get; set; }
        public string? ExpenseName { get; set; }
        public string? Description { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int ExpensesId { get; set; }
        //public string? ExpenseName { get; set; }
        //public string? Description { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public int? SessionId { get; set; }
        //public int? SchoolId { get; set; }
        //public int? Userid { get; set; }
    }
}
