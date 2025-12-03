using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class M_FeeDetail
    {
        [Key]

        public int FDId { get; set; }
        public string? ReceiptNo { get; set; }
        public Nullable<int> stu_id { get; set; }
        public Nullable<int> ClassId { get; set; }
        public string? stu_code { get; set; }
        public string? Status { get; set; }
        public Nullable<int> NoOfIns { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> PayFees { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public Nullable<double> AdmissionFees { get; set; }
        public Nullable<double> ExamFees { get; set; }
        public Nullable<double> Tutionfee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> FeeTotal { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> NetDueFees { get; set; }
        public Nullable<double> DueFees { get; set; }
        public Nullable<double> TotalFees { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public string? Remark { get; set; }
        public Nullable<double> Cash { get; set; }
        public Nullable<double> Upi { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> RTS { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderNo { get; set; }
        public string? TransactionId { get; set; }
        public string? ReceiptType { get; set; }


        //public int FRId { get; set; }
        //public string? ReceiptNo { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public string? Status { get; set; }
        //public string? FeeType { get; set; }
        //public Nullable<double> PayFees { get; set; }
        //public Nullable<System.DateTime> PaymentDate { get; set; }
        //public string? PaymentMode { get; set; }
        //public string? Remark { get; set; }
        //public Nullable<double> Cash { get; set; }
        //public Nullable<double> Upi { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> UserId { get; set; }
        //public System.DateTime? CreateDate { get; set; }
        //public System.DateTime? UpdateDate { get; set; }
    }
}
