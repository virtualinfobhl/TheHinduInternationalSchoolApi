using ApiProject.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;


namespace ApiProject.Models.Request
{
    public class ExpensesReq
    {
        public string? ExpenseName { get; set; }

    }

    public class SchoolExpensesReq
    {
        public int? ExpensesId { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Nullable<double> Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
    }

    public class UpdateschlExpenses
    {
        public int SEId { get; set; }
        public int? ExpensesId { get; set; }
        public string? InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<double> Amount { get; set; }
        public string? Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class GetSchoolExpensesListReq
    {
        public int? ExpensesId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

}
