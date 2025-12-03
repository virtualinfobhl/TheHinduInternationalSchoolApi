using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class TransDriverTbl
    {
        [Key]

        public int DriverId { get; set; }
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }



        //public int DriverId { get; set; }
        //public string? DriverName { get; set; }
        //public string? DMobileno { get; set; }
        //public string? DAddress { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
