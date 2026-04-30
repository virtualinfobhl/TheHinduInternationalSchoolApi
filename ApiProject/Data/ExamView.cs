
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class ExamView
    {
        [Key]
        public int subject_id { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> college_id { get; set; }
        public Nullable<int> course_id { get; set; }
        public string? subject_name { get; set; }
        public string? theory_marks { get; set; }
        public string? tma_marks { get; set; }
        public string? practical_marks { get; set; }
        public string? total_marks { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<long> first_test { get; set; }
        public Nullable<long> second_test { get; set; }
        public Nullable<long> third_test { get; set; }
        public Nullable<long> half_yearly { get; set; }
        public Nullable<long> yearly { get; set; }
        public Nullable<long> Written { get; set; }
        public Nullable<long> Oral { get; set; }
        public Nullable<long> Pratical { get; set; }
        public Nullable<long> TestTotal { get; set; }
        public string? TestGrade { get; set; }
        public string? TestType { get; set; }
        public string? MarksType { get; set; }
        public string? MGrade { get; set; }
        public Nullable<int> stu_id { get; set; }
        public Nullable<long> fourth_test { get; set; }
        public Nullable<long> Quarterly { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> TestTypeId { get; set; }
        public Nullable<long> MaxMarks { get; set; }
        public Nullable<int> Userid { get; set; }
        public string? Marks_Type { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}