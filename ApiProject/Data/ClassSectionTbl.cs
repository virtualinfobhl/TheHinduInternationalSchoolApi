using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class ClassSectionTbl
    {
        [Key]
        public int CSId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> SchoolId { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
