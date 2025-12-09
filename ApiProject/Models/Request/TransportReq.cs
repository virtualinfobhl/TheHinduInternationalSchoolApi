using ApiProject.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ApiProject.Models.Request
{
    public class AddDriverreq
    {
        public string? DriverName { get; set; }
        public string? Mobileno { get; set; }
        public string? Address { get; set; }
    }

    public class AddVehicleReq
    {
        public int? DriverId { get; set; }
        public string? VehicleNo { get; set; }
    }

    public class AddRouteReq
    {
        public string? RouteName { get; set; }
    }

    public class AddStoppageReq
    {
        public string? Stoppage { get; set; }
        public int? RouteId { get; set; }
    }
    public class AddTransportFeeReq
    {
        public Nullable<double> MonthFee { get; set; }
        public int? StoppageId { get; set; }

    }
    public class AddRouteAssignReq
    {
        public int? VehicleId { get; set; }
    }

    public class RouteReq
    {
        public int RouteId { get; set; }
    }

    public class AddRouteAssignRequest
    {
        public AddRouteAssignReq Req { get; set; }
        public List<RouteReq> Bus { get; set; }
    }

    public class UpdateRouteAssignRequest
    {
        public int? VehicleId { get; set; }
        public AddRouteAssignReq Req { get; set; }
        public List<RouteReq> Bus { get; set; }
    }

    public class StuRouteAssignReq
    {
        //  public int TSRAId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? VehicleId { get; set; }
        public int? RouteId { get; set; }
        public int? StoppageId { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> TDiscount { get; set; }
        public Nullable<double> NetTransportFee { get; set; }
        public Nullable<double> TOldDueFee { get; set; }
        public Nullable<double> LastDueFee { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        //  public Nullable<bool> Active { get; set; }
        // public List<getTransportinstallment> TInstallment { get; set; }
    }


    public class getTransportinstallment
    {
        public int TIId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> StoppageId { get; set; }
        public string? InstallmentNo { get; set; }
        public Nullable<double> TTotalFee { get; set; }
        public Nullable<double> InstallmentFee { get; set; }
        public string? MonthName { get; set; }
        public Nullable<bool> ReActive { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }

    public class UpdateStuRouteAssignReq
    {
        public int TSRAId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? VehicleId { get; set; }
        public int? RouteId { get; set; }
        public int? StoppageId { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> TDiscount { get; set; }
        public Nullable<double> NetTransportFee { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class TransportDetailReportReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? RouteId { get; set; }
    }
    public class TransportFeeReportReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? StudentId { get; set; }
        public int? VehicleId { get; set; }
    }

    public class StudentTransportFeeReq
    {

        //  public int TReceiptId { get; set; }
        //   public string? ReceiptNo { get; set; }
        public Nullable<int> TSRAId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<int> StoppageId { get; set; }
        public string? MonthType { get; set; }
        public string? MonthName { get; set; }
        //   public string? FeeType { get; set; }
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> PayFee { get; set; }
        public Nullable<double> FeeDiscount { get; set; }
        public Nullable<double> Cash { get; set; }
        public Nullable<double> UPI { get; set; }
        public string? PaymentMode { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? Remark { get; set; }
        //  public Nullable<System.DateTime> Date { get; set; }
        //  public Nullable<bool> Active { get; set; }
      //  public Nullable<double> DueFee { get; set; }
    }

    public class UpdateOldFeeReq
    {
        public Nullable<int> TSRAId { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> VehicleId { get; set; }
        public Nullable<int> RouteId { get; set; }
        public Nullable<int> StoppageId { get; set; }
        public Nullable<double> FeeDiscount { get; set; }
        public Nullable<double> PayFee { get; set; }
        public Nullable<double> Cash { get; set; }
        public Nullable<double> UPI { get; set; }
      //  public string? FeeType { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public string? Remark { get; set; }
    }


}
