using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class GradeInfo
    {

        [Key]
        public int grade_id { get; set; }
        public string? grade_name { get; set; }
        public string? Percent_Upto { get; set; }
        public string? Percent_From { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int GradeId { get; set; }
        //public string GradeName { get; set; }
        //public string Percent_Upto { get; set; }
        //public string Percent_From { get; set; }
        //public Nullable<int> UserId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
    }
}
