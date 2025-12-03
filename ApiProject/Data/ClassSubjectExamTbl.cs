using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class ClassSubjectExamTbl
    {
        [Key]
        public int CSEId { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? ExamId { get; set; }
        public string? MarksType { get; set; }
        public double? MaxMarks { get; set; }
        public int? UserId { get; set; }
        public int? SchoolId { get; set; }
        public int? SessionId { get; set; }
        public System.DateTime? CreateDate { get; set; }
        public System.DateTime? UpdateDate { get; set; }
        public Nullable<bool> CSEActive { get; set; }
    }
}
