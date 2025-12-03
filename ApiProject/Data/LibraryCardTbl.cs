using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class LibraryCardTbl
    {
        [Key]
        public int LibraryId { get; set; }
        public string? LibraryCardNo { get; set; }
        public Nullable<int> MemberId { get; set; }
        public Nullable<int> stu_id { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public string? MemberType { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> BranchId { get; set; }

        //public int LibraryId { get; set; }
        //public string? LibraryCardNo { get; set; }
        //public Nullable<int> MemberId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<int> SectionId { get; set; }
        //public Nullable<int> Emp_Id { get; set; }
        //public string? MemberType { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> Userid { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public Nullable<int> BranchId { get; set; }
    }
}
