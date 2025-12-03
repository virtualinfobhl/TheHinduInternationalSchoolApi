using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class UserInformation
    {
        [Key]

        public int id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Mobileno { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public string? UserRole { get; set; }




        //public int UserId { get; set; }
        //public string? Name { get; set; }
        //public string? Address { get; set; }
        //public string? MobileNo { get; set; }
        //public Nullable<System.DateTime> JoiningDate { get; set; }
        //public Nullable<bool> UserActive { get; set; }
        //public Nullable<bool> MasterActive { get; set; }
        //public string? UserName { get; set; }
        //public string? Password { get; set; }
        //public string? Status { get; set; }
        //public int? SchoolId { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
    }
}
