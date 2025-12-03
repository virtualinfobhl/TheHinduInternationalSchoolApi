using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string? EventName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }

        //public int EventId { get; set; }
        //public string? Eventname { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
