using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class TransInstallmentTbl
    {
        [Key]

        public int TInstallmentId { get; set; }
        public Nullable<int> StuId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public string? InstallmentNo { get; set; }
        public Nullable<double> TotalTransFee { get; set; }
        public Nullable<double> InstallFee { get; set; }
        public Nullable<double> DueFee { get; set; }
        public string? MonthName { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<bool> ReActive { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> Updatedate { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> BramchId { get; set; }
        public Nullable<int> BusId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<int> StoppageId { get; set; }

        //public int TIId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> StoppageId { get; set; }
        //public string? InstallmentNo { get; set; }
        //public Nullable<double> TTotalFee { get; set; }
        //public Nullable<double> InstallmentFee { get; set; }
        //public string? MonthName { get; set; }
        //public Nullable<bool> ReActive { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
