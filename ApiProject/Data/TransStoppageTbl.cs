
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class TransStoppageTbl
    {
        [Key]

        public int StoppageId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public string? Stoppage { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> BranchId { get; set; }


        //public int StoppageId { get; set; }
        //public string? Stoppage { get; set; }
        //public int? RouteId { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
