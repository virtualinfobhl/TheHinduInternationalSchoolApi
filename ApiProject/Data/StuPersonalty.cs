using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class StuPersonalty
    {
        [Key]
        public int PersonaltyId { get; set; }
        public Nullable<int> StuId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? Discipline { get; set; }
        public string? Concentration { get; set; }
        public string? Intiative { get; set; }
        public string? Independently { get; set; }
        public string? Direction { get; set; }
        public string? Cleanliness { get; set; }
        public string? Etiquette { get; set; }
        public string? OtherPro { get; set; }
        public string? Passionate { get; set; }
        public string? Confident { get; set; }
        public string? Responsible { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> BranchId { get; set; }




        //public int PersonaltyId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> SectionId { get; set; }
        //public string? Discipline { get; set; }
        //public string? Concentration { get; set; }
        //public string? Intiative { get; set; }
        //public string? Independently { get; set; }
        //public string? Direction { get; set; }
        //public string? Cleanliness { get; set; }
        //public string? Etiquette { get; set; }
        //public string? OtherPro { get; set; }
        //public string? Passionate { get; set; }
        //public string? Confident { get; set; }
        //public string? Responsible { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> Userid { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<System.DateTime> Createdate { get; set; }
        //public Nullable<System.DateTime> Updatedate { get; set; }

    }
}
