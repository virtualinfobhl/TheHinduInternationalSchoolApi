using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class Student_Attendance
    {
        [Key]
        public int SAId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int SAId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public string Status { get; set; }
        //public string Note { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<System.TimeSpan> Time { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> Userid { get; set; }
        //public Nullable<int> SchoolId { get; set; }

    }
}
