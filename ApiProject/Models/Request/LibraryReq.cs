using ApiProject.Data;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ApiProject.Models.Request
{
    public class AddBookReq
    {
        public string? BookTitle { get; set; }
        public string? BookNumber { get; set; }
        public string? Publisher { get; set; }
        public string? Author { get; set; }
        public string? Subject { get; set; }
        public Nullable<long> TotalQuantity { get; set; }
        public Nullable<double> BookPrice { get; set; }
        public string? Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class UpdateBookReq
    {
        public int? BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookNumber { get; set; }
        public string? Publisher { get; set; }
        public string? Author { get; set; }
        public string? Subject { get; set; }
        public Nullable<long> TotalQuantity { get; set; }
        public Nullable<double> BookPrice { get; set; }
        public string? Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class AddcardNoReq
    {
        public string? LibraryCardNo { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public int? Emp_Id { get; set; }
        public string? MemberType { get; set; }
    }

    public class LibrartReportReq
    {
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public int? Emp_Id { get; set; }
        public string? MemberType { get; set; }
    }

    public class IssueLibraryBookReq
    {
        public Nullable<int> LibraryId { get; set; }
        public Nullable<int> BookId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public Nullable<long> IssueQuantity { get; set; }
        public Nullable<System.DateTime> DueReturnDate { get; set; }
    }

    public class UpdateReturndateReq
    {
        public Nullable<int> BIssueId { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
    }




}