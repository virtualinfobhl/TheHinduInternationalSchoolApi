using ApiProject.Data;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ApiProject.Models.Request
{
    public class Getparentsreq
    {
        public string? Token { get; set; }
        public int? ParentId { get; set; }
        public int? StudentId { get; set; }

    }

    public class AddStudentinstallReq
    {
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? sectionId { get; set; }
        public Nullable<double> PaidFee { get; set; }
        public string? Remark { get; set; }
        //  public Nullable<double> Duefee { get; set; }
        //  public Nullable<System.DateTime> PaymentDate { get; set; }

    }

    public class AddTransportMonthFeeReq
    {
        public int? StuRouteAssignId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? sectionId { get; set; }
        public int? VehicleId { get; set; }
        public int? RouteId { get; set; }
        public int? StoppageId { get; set; }
        public string? MonthName { get; set; }
        public Nullable<double> PayFee { get; set; }
        //     public Nullable<System.DateTime> Date { get; set; }
        //public Nullable<double> Discount { get; set; }
        //public Nullable<double> SpclDiscount { get; set; }
        //    public Nullable<double> PayDiscount { get; set; }
        //  public Nullable<double> Duefee { get; set; }
        //    public Nullable<System.DateTime> PaymentDate { get; set; }
        //    public string? Remark { get; set; }
    }
}
