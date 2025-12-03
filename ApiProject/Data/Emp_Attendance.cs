using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class Emp_Attendance
    {
        [Key]
        public int EAId { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public Nullable<int> Emp_Code { get; set; }
        public string? Emp_Name { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int EAId { get; set; }
        //public Nullable<int> Emp_Id { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<System.TimeSpan> Time { get; set; }
        //public string? Status { get; set; }
        //public string? Note { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> UserId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> branch_id { get; set; }
    }
}
