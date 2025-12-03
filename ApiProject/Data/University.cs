using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class University
    {
        [Key]

        public int university_id { get; set; }
        public string? university_name { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int ClassId { get; set; }
        //public string ClassName { get; set; }
        //public int ClassPriority { get; set; }
        //public Nullable<bool> CActive { get; set; }
        //public int UserId { get; set; }
        //public int SchoolId { get; set; }
        //public int SessionId { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
    }
}
