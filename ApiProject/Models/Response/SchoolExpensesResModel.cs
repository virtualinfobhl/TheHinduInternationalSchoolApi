using ApiProject.Data;

namespace ApiProject.Models.Response
{

    public class GetExpenses
    {
        public int? ExpensesId { get; set; }
        public string? ExpenseName { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class UpdateExpensesModel
    {
        public int? ExpensesId { get; set; }
        public string? ExpenseName { get; set; }
    }

    public class GetSchoolExpenses
    {
        public int SEId { get; set; }
        public int? ExpensesId { get; set; }
        public string? ExpensesName { get; set; }
        public string? RerNo { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<double> Amount { get; set; }
        public string? Description { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }


}
