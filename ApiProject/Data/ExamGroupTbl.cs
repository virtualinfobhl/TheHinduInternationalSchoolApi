using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class ExamGroupTbl
    {
        [Key]
        public int ExamGroupId { get; set; }
        public string ExamGroupName { get; set; }
        public Nullable<bool> EGActive { get; set; }
        public int UserId { get; set; }
        public int SchoolId { get; set; }
        public int SessionId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
