using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class NewTransportFeeTbl
    {
        [Key]
        public int NewPaymentId { get; set; }
        public Nullable<int> StuFeeId { get; set; }
        public Nullable<int> StuRouteAssignId { get; set; }
        public Nullable<int> stu_id { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? ReceiptNo { get; set; }
        public Nullable<int> BusId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<int> StoppageId { get; set; }
        public Nullable<double> TransFee { get; set; }
        public Nullable<double> Cash { get; set; }
        public Nullable<double> UPI { get; set; }
        public Nullable<double> Paydiscount { get; set; }
        public Nullable<double> PayFee { get; set; }
        public Nullable<double> DueFee { get; set; }
        public Nullable<double> OldDueFee { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> SpclDiscount { get; set; }
        public Nullable<double> NetTransFee { get; set; }
        public string? MonthType { get; set; }
        public string? MonthName { get; set; }
        public string? PaymentMode { get; set; }
        public string? FeeType { get; set; }
        public string? Remark { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> Userid { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderNo { get; set; }
        public string? TransactionId { get; set; }
        public string? ReceiptType { get; set; }


        //public int TReceiptId { get; set; }
        //public string? ReceiptNo { get; set; }
        //public Nullable<int> TSRAId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> SectionId { get; set; }
        //public Nullable<int> VehicleId { get; set; }
        //public Nullable<int> RouteId { get; set; }
        //public Nullable<int> StoppageId { get; set; }
        //public string? MonthType { get; set; }
        //public string? MonthName { get; set; }
        //public string? FeeType { get; set; }
        //public Nullable<double> TotalFee { get; set; }
        //public Nullable<double> PayFee { get; set; }
        //public Nullable<double> FeeDiscount { get; set; }
        //public Nullable<double> Cash { get; set; }
        //public Nullable<double> UPI { get; set; }
        //public string? PaymentMode { get; set; }
        //public Nullable<System.DateTime> PaymentDate { get; set; }
        //public string? Remark { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
