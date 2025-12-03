using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class TransBusTbl
    {
        [Key]

        public int BusId { get; set; }
        public Nullable<int> DriverId { get; set; }
        public string? VihecleNo { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int VehicleId { get; set; }
        //public string VehicleNo { get; set; }
        //public Nullable<int> DriverId { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
