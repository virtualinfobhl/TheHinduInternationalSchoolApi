using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public partial class InstallmentTbl
    {
        [Key]

        public int InstallmentId { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> fee_id { get; set; }
        public Nullable<double> total { get; set; }
        public string? Installmentno { get; set; }
        public string? Installment { get; set; }
        public Nullable<double> FeeAmount { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> branch_id { get; set; }

        //public int InstallmentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<double> InsAmount { get; set; }
        //public string? Installment { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> UserId { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
        //public Nullable<int> Installmentno { get; set; }
    }
}
