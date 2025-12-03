using System.ComponentModel.DataAnnotations;

namespace ThisApiProject.Data
{
    public class EmployeeSalary
    {
        [Key]

        public int id { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public Nullable<int> Month { get; set; }
        public string? Year { get; set; }
        public string? TotalPresent { get; set; }
        public string? TotalAbsent { get; set; }
        public string? TotalLeave { get; set; }
        public string? TotalHoliday { get; set; }
        public string? TotalHalfDay { get; set; }
        public string? TotalDays { get; set; }
        public string? BasicSalary { get; set; }
        public string? Allowance { get; set; }
        public string? TotalSalary { get; set; }
        public string? ESI { get; set; }
        public string? PF { get; set; }
        public string? TDS { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public Nullable<bool> Active { get; set; }
        public int? Userid { get; set; }
        public string? BankName { get; set; }
        public string? ChequeNo { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string? ReceiptNo { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<double> PayAdvAmount { get; set; }
        public Nullable<double> PayAmount { get; set; }
    }
}
