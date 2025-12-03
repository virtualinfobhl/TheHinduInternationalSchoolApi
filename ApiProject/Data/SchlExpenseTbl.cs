using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class SchlExpenseTbl
    {
        [Key]
        public int SchlExpenseId { get; set; }
        public Nullable<int> ExpenseId { get; set; }
        public string? RefNo { get; set; }
        public string? Name { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<double> Amount { get; set; }
        public string? AttachDocument { get; set; }
        public string? Description { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }

        //public int SEId { get; set; }
        //public int? ExpensesId { get; set; }
        //public string? RerNo { get; set; }
        //public string? InvoiceNo { get; set; }
        //public Nullable<System.DateTime> InvoiceDate { get; set; }
        //public Nullable<double> Amount { get; set; }
        //public string? Description { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public int? SessionId { get; set; }
        //public int? SchoolId { get; set; }
        //public int? Userid { get; set; }
    }
}
