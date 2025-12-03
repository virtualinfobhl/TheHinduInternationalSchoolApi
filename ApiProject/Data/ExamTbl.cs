using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class ExamTbl
    {
        [Key]

        //public int TestExamId { get; set; }
        //public Nullable<int> university_id { get; set; }
        //public Nullable<int> stu_id { get; set; }
        //public Nullable<int> subject_id { get; set; }
        //public Nullable<int> EmpId { get; set; }
        //public Nullable<long> Written { get; set; }
        //public Nullable<long> Oral { get; set; }
        //public Nullable<long> Pratical { get; set; }
        //public Nullable<long> Total { get; set; }
        //public string? Grade { get; set; }
        //public string? TestType { get; set; }
        //public string? MarksType { get; set; }
        //public string? MGrade { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<int> CompanyId { get; set; }
        //public Nullable<int> Userid { get; set; }
        //public Nullable<int> Branch_id { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> TestTypeId { get; set; }
        //public Nullable<long> MaxMarks { get; set; }


        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public Nullable<int> ExamGId { get; set; }
        public Nullable<bool> EActive { get; set; }
        public int? UserId { get; set; }
        public int? SchoolId { get; set; }
        public int? SessionId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public Nullable<int> ExamPriority { get; set; }
    }
}
