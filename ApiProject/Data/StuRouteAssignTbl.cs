using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class StuRouteAssignTbl
    {
        [Key]
        public int StuRouteAssignId { get; set; }
        public Nullable<int> stu_id { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> BusId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<int> StoppageId { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> NetTranSFee { get; set; }
        public Nullable<double> TTransportFee { get; set; }
        public Nullable<double> TPayFee { get; set; }
        public Nullable<double> TDueFee { get; set; }
        public Nullable<double> TPayDiscount { get; set; }
        public Nullable<double> OldPayFee { get; set; }
        public Nullable<double> OldDueFee { get; set; }
        public Nullable<double> OldTransDueFee { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> BranchId { get; set; }


        //public int TSRAId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> SectionId { get; set; }
        //public Nullable<int> VehicleId { get; set; }
        //public Nullable<int> RouteId { get; set; }
        //public Nullable<int> StoppageId { get; set; }
        //public Nullable<double> TransportFee { get; set; }
        //public Nullable<double> TDiscount { get; set; }
        //public Nullable<double> NetTransportFee { get; set; }
        //public Nullable<double> TOldDueFee { get; set; }
        //public Nullable<double> LastDueFee { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
