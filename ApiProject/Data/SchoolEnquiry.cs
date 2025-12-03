using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class SchoolEnquiry
    {
        [Key]
        public int EnquiryID { get; set; }
        public string SchoolName { get; set; }
        public string DirectorName { get; set; }
        public string DirectorMobileNo { get; set; }
        public string SchoolMobileNo { get; set; }
        public string Email { get; set; }
        public string LandlineNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string LogoPath { get; set; }
        public string WebsiteURL { get; set; }
        public Nullable<int> EstablishmentYear { get; set; }
        public string BoardName { get; set; }
        public string AffiliationNumber { get; set; }
        public string SchoolType { get; set; }
        public string Medium { get; set; }
        public string PrincipalName { get; set; }
        public string PrincipalMobileNo { get; set; }
        public Nullable<int> TotalStudents { get; set; }
        public Nullable<int> TotalTeachers { get; set; }
        public string TransportAvailable { get; set; }
        public string HostelAvailable { get; set; }
        public string FacebookPage { get; set; }
        public string InstagramPage { get; set; }
        public string LocationLink { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<bool> DeleteE { get; set; }
        public Nullable<bool> Complete { get; set; }
    }
}
