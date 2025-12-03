namespace ApiProject.Models.Response
{
    public class LoginRes
    {
        public string? Token { get; set; }
      //  public string? SchoolCode { get; set; }
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public string? Pincode { get; set; }
        public string? MobileNo1 { get; set; }
        public string? MobileNo2 { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }        
        public string? UserType { get; set; }
        public string? StartSession { get; set; }
        public string? EndSession { get; set; }
        public string? StartSessionYear { get; set; }
        public string? EndSessionYear { get; set; }
        public string? ResponseCode { get; set; } // "-1", "-2", "1" etc
        public DateTime? ExpiryDate { get; set; }
        //---->>
        public int? ParentId { get; set; }
        public string? ParentName { get; set; }
        public string? ParentMobile { get; set; }
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Class { get; set; }
        public string? SrNo { get; set; }
    }
}
