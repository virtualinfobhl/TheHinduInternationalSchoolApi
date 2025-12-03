using ApiProject.Data;
using System.Drawing.Printing;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Pkcs;

namespace ApiProject.Models.Response
{
    public class getBooks
    {
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookNumber { get; set; }
        public string? Publisher { get; set; }
        public string? Author { get; set; }
        public string? Subject { get; set; }
        public Nullable<long> TotalQuantity { get; set; }
        public Nullable<double> BookPrice { get; set; }
        public string? Description { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class GetLibraryStudentModel
    {
        public int? MemberId { get; set; }
        public string? CardNo { get; set; }
        public int? StudentId { get; set; }
        public string? SRNo { get; set; }
        public string? stu_name { get; set; }
        public string? father_name { get; set; }
        public string? mother_name { get; set; }
        public string? father_mobile { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }
    }

    public class GetlibrarayEmployeeModel
    {
        public int? MemberId { get; set; }
        public string? CardNo { get; set; }
        public int? Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public string? DOB { get; set; }
        public string? Gendar { get; set; }
        public string? Mobileno { get; set; }
        public string? EmailId { get; set; }
    }

    public class GetBookReportReq
    {
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookNumber { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string? Publisher { get; set; }
        public string? Author { get; set; }
        public string? Subject { get; set; }
        public Nullable<long> TotalQuantity { get; set; }
        public Nullable<double> BookPrice { get; set; }
        public string? Description { get; set; }

    }

    public class GetStuDetail
    {
        public int? StudentId { get; set; }
        public string? SRNo { get; set; }
        public string? stu_name { get; set; }
        public string? Fatername { get; set; }
        public string? StuMobileno { get; set; }
        public string? Gender { get; set; }
    }

    public class GetempDetails
    {
        public int? Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public string? Fatername { get; set; }
        public string? EmpMobileno { get; set; }
        public string? Gender { get; set; }
        public string? EmailId { get; set; }

    }

    public class GetMemberListModel
    {
        public int? MemberId { get; set; }
        public string? CardNo { get; set; }
        public string? MemberType { get; set; }
        public List<GetStuDetail> Student { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public List<GetempDetails> Employee { get; set; }
    }

    public class LibraryIssudatemodel
    {
        public string? BookTitle { get; set; }
        public string? BookNumber { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> DueReturnDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }

    }

    public class GetLibraryReportModel
    {
        public string? CardNo { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public List<GetStuDetail> Student { get; set; }
        public List<GetempDetails> Employee { get; set; }
        public List<LibraryIssudatemodel> Librarydate { get; set; }

    }

    public class getMenmberProfileModel
    {
        public int? MemberId { get; set; }
        public string? CardNo { get; set; }
        public string? MemberType { get; set; }
        public List<GetStuDetail> Student { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public List<GetempDetails> Employee { get; set; }
        public List<LibraryIssudatemodel> Bookdata { get; set; }
    }

    public class GetAuthormodel
    {
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? Author { get; set; }
    }


}
