using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class BookIssueTbl
    {
        [Key]
        public int IssueId { get; set; }
        public Nullable<int> BookId { get; set; }
        public string? BookTitle { get; set; }
        public Nullable<int> LibraryId { get; set; }
        public Nullable<int> stu_id { get; set; }
        public Nullable<int> Emp_Id { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<System.DateTime> DueReturnDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> BranchId { get; set; }

        //public int BIssueId { get; set; }
        //public Nullable<int> BookId { get; set; }
        //public Nullable<int> LibraryId { get; set; }
        //public Nullable<int> StudentId { get; set; }
        //public Nullable<int> Emp_Id { get; set; }
        //public Nullable<System.DateTime> IssueDate { get; set; }
        //public Nullable<long> IssueQuantity { get; set; }
        //public Nullable<System.DateTime> DueReturnDate { get; set; }
        //public Nullable<System.DateTime> ReturnDate { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> Userid { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> BranchId { get; set; }
        //public Nullable<int> SessionId { get; set; }
    }
}
