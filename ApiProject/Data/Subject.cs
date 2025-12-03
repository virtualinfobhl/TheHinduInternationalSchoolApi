using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class Subject
    {
        [Key]

        public int subject_id { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> college_id { get; set; }
        public Nullable<int> course_id { get; set; }
        public string? subject_name { get; set; }
        public Nullable<int> Priority { get; set; }
        public string? theory_marks { get; set; }
        public string? tma_marks { get; set; }
        public string? practical_marks { get; set; }
        public string? Marks_Type { get; set; }
        public string? total_marks { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<long> Quarterly { get; set; }
        public Nullable<long> first_test { get; set; }
        public Nullable<long> second_test { get; set; }
        public Nullable<long> third_test { get; set; }
        public Nullable<long> fourth_test { get; set; }
        public Nullable<long> half_yearly { get; set; }
        public Nullable<long> yearly { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int SubjectId { get; set; }
        //public string? SubjectName { get; set; }
        //public Nullable<bool> SbjActive { get; set; }
        //public int? UserId { get; set; }
        //public int? SchoolId { get; set; }
        //public int? SessionId { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
        //public Nullable<int> SubjectPriority { get; set; }
    }
}
