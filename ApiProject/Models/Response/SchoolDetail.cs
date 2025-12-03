namespace ApiProject.Models.Response
{
    public class SchoolDetail
    {
        public string schoolname { get; set; }
        public string ownername { get; set; }
        public string schoolcode { get; set; }
        public string rgtno { get; set; }
       // public Nullable<System.DateTime> rgstdate { get; set; }
        public string rgstdate { get; set; }

        public Nullable<bool> companyactive { get; set; }
        public string address { get; set; }
        public string landlinenum { get; set; }
        public string mobileno1 { get; set; }
        public string mobileno2 { get; set; }
        public string statename { get; set; }
        public string districtname { get; set; }
        public string cityname { get; set; }
        public string pincode { get; set; }
        public string email { get; set; }
        public string weburl { get; set; }
        public string logoimg { get; set; }
        public string rlogo { get; set; }
        public Nullable<System.DateTime> joiningdate { get; set; }
        public Nullable<System.DateTime> expiredate { get; set; }
        public Nullable<bool> active { get; set; }
    }
}
