using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class EmployeeBankDetailTbl
    {
        [Key]
        public int BankId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string? AHName { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> SchoolId { get; set; }
        public Nullable<int> SessionId { get; set; }
    }
}
