using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class fee_installment
    {
        [Key]

        public int Id { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> stu_id { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<System.DateTime> paid_date { get; set; }
        public Nullable<int> IntallmentID { get; set; }
        public Nullable<double> total_fee { get; set; }
        public Nullable<double> due_fee { get; set; }
        public string? Installment { get; set; }
        public Nullable<double> FAmount { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }


        //public int FIId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<double> SInsAmount { get; set; }
        //public string? Installment { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> UserId { get; set; }
        //public System.DateTime? CreateDate { get; set; }
        //public System.DateTime? UpdateDate { get; set; }
    }
}
