using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class EventCertificate
    {
        [Key]
        public int EvId { get; set; }
        public Nullable<int> EventId { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? CertificateStatus { get; set; }
        public string? RankType { get; set; }
        public string? Description { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> BranchId { get; set; }

        //public int ECId { get; set; }
        //public Nullable<int> EventId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> EmployeId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> SectionId { get; set; }
        //public string? Status { get; set; }
        //public string? RankType { get; set; }
        //public string? Description { get; set; }
        //public Nullable<System.DateTime> IssueDate { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> Userid { get; set; }
    }
}
