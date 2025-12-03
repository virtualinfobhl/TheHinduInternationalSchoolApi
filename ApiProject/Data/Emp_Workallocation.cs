using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class Emp_Workallocation
    {
        [Key]
        public int Id { get; set; }
        public string? Emp_Code { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public Nullable<int> UniversityId { get; set; }
        public Nullable<int> college_id { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }

        //public int EWorkId { get; set; }
        //public Nullable<int> Emp_Id { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> SectionId { get; set; }
        //public Nullable<int> SubjectId { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> UserId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> branch_id { get; set; }
    }
}
