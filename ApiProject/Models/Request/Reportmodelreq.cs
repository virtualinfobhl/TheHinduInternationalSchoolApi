using ApiProject.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ApiProject.Models.Request
{

    public class GetStudentFeeListReqModel
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? Studentid { get; set; }
        public string? Srno { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetStudentFeeReqModel
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? Studentid { get; set; }
        public string? Srno { get; set; }
    }

    public class GetClasswiseInstallmentListReq
    {
        public int? Classid { get; set; }
        public int? SectionId { get; set; }
    }

    public class GetTestExamReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? TestType { get; set; }
    }

    public class DailyCollectionReportReq
    {
        public Nullable<System.DateTime> FormDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
    }
}
