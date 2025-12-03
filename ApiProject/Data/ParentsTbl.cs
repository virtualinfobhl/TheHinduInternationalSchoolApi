using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class ParentsTbl
    {
        [Key]
        public int ParentsId { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianMobileNo { get; set; }
        public string? FatherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? FatherOccupation { get; set; }
        public Nullable<double> FatherIncome { get; set; }
        public string? MotherName { get; set; }
        public string? MotherMobileNo { get; set; }
        public string? MotherOccupation { get; set; }
        public Nullable<double> MotherIncome { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
