using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class RouteAssignTbl
    {
        [Key]

        public int RAId { get; set; }
        public Nullable<int> BusId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public string? RouteName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int RAId { get; set; }
        //public Nullable<int> VehicleId { get; set; }
        //public Nullable<int> RouteId { get; set; }
        //public string? RouteName { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
