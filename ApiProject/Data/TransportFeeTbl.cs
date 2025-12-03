using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class TransportFeeTbl
    {
        [Key]

        public int TransFeeId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<int> StopageId { get; set; }
        public Nullable<double> MonthFee { get; set; }
        public Nullable<double> SixMonthFee { get; set; }
        public Nullable<double> SixDiscount { get; set; }
        public Nullable<double> SixMonthTotal { get; set; }
        public Nullable<double> YearlyFee { get; set; }
        public Nullable<double> YearlyDiscount { get; set; }
        public Nullable<double> YearlyTotal { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> BranchId { get; set; }


        //public int TFId { get; set; }
        //public Nullable<int> StoppageId { get; set; }
        //public Nullable<double> MonthFee { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
