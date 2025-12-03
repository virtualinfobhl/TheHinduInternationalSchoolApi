using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public  class collegeinfo
    {

        [Key]

        public int collegeid { get; set; }
        public string? collegename { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int SectionId { get; set; }
        //public string SectionName { get; set; }
        //public Nullable<int> SectionPriority { get; set; }
        //public Nullable<bool> active { get; set; }
        //public Nullable<int> UserId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
    }
}
