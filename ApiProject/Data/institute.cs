using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class institute
    {
        [Key]

        public int institute_id { get; set; }
        public string institute_name { get; set; }
        public string? regist_num { get; set; }
        public string? regist_date { get; set; }
        public string? address { get; set; }
        public string? landline_num { get; set; }
        public string? mob_num { get; set; }
        public string? alternatemob_num { get; set; }
        public string? state_name { get; set; }
        public string? district_name { get; set; }
        public string? city_name { get; set; }
        public string? servicetax_num { get; set; }
        public string? tin_num { get; set; }
        public string? pan_num { get; set; }
        public string? pincode { get; set; }
        public string? email { get; set; }
        public string? weburl { get; set; }
        public string? logo_img { get; set; }
        public string? institute_img { get; set; }
        public string? instituteCode { get; set; }
        public Nullable<int> SessionId { get; set; }




        //public int SchoolId { get; set; }
        //public string SchoolName { get; set; }
        //public string? OwnerName { get; set; }
        //public string SchoolCode { get; set; }
        //public string? RGTNO { get; set; } 
        //public Nullable<System.DateTime> RGSTDate { get; set; }
        //public Nullable<bool> CompanyActive { get; set; }
        //public string? Address { get; set; }
        //public string? LandlineNum { get; set; }
        //public string? MobileNo1 { get; set; }
        //public string? MobileNo2 { get; set; }
        //public string? StateName { get; set; }
        //public string? DistrictName { get; set; }
        //public string? CityName { get; set; }
        //public string? PinCode { get; set; }
        //public string? Email { get; set; }
        //public string? WebUrl { get; set; }
        //public string? LogoImg { get; set; }
        //public string? Rlogo { get; set; }
        //public Nullable<System.DateTime> JoiningDate { get; set; }
        //public Nullable<System.DateTime> ExpireDate { get; set; }
        //public System.DateTime CreateDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
        //public Nullable<bool> Active { get; set; }
    }
}
