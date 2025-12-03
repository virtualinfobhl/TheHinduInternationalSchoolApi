using ApiProject.Data;
using System.Drawing.Printing;
using System.Security.Cryptography.Pkcs;

namespace ApiProject.Models.Response
{

    public class GetDriverList
    {
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? Mobileno { get; set; }
        public string? Address { get; set; }
        public bool? Active { get; set; }
    }

    public class GetDriver
    {
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
    }

    public class UpdateDriver
    {
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? Mobileno { get; set; }
        public string? Address { get; set; }
    }

    public class GetVehicleList
    {
        public int VehicleId { get; set; }
        public string VehicleNo { get; set; }
        public Nullable<int> DriverId { get; set; }
        public string DriverName { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class UpdateVehicleModel
    {
        public int? VehicleId { get; set; }
        public string? VehicleNo { get; set; }
        public int? DriverId { get; set; }
    }

    public class GetRouteList
    {
        public int? RouteId { get; set; }
        public string? RouteName { get; set; }
        public bool? Active { get; set; }
    }

    public class UpdateRouteModel
    {
        public int? RouteId { get; set; }
        public string? RouteName { get; set; }
    }

    public class GetStoppageList
    {
        public int StoppageId { get; set; }
        public string? Stoppage { get; set; }
        public int? RouteId { get; set; }
        public string? RouteName { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class UpdateStoppageModel
    {
        public int? RouteId { get; set; }
        public int? StoppageId { get; set; }
        public string? Stoppage { get; set; }
    }

    public class GetTransportFeeModel
    {
        public int? TFId { get; set; }
        public Nullable<int> StoppageId { get; set; }
        public string? StoppageName { get; set; }
        public Nullable<double> MonthFee { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class UpdateTransportFee
    {
        public int? TFId { get; set; }
        public Nullable<int> StoppageId { get; set; }
        public Nullable<double> MonthFee { get; set; }
    }

    public class GetRouteAssignModel
    {
        public int? RAId { get; set; }
        public string? Vehicleno { get; set; }
        // public int? RouteId { get; set; }
        public Nullable<bool> Active { get; set; }
        public List<UpdateRouteModel> Route { get; set; }
    }

    public class GetRouteDataModel
    {
        public int? VehicleId { get; set; }
        public int? RouteId { get; set; }
        public string? RouteName { get; set; }
    }

    public class GetStuRouteAssignModel
    {
        public int TSRAId { get; set; }
        public int? StudentId { get; set; }
        public string? Studentname { get; set; }
        public int? ClassId { get; set; }
        public string? Class { get; set; }
        public int? SectionId { get; set; }
        public string? Sectionname { get; set; }
        public int? VehicleId { get; set; }
        public string? Vehiclename { get; set; }
        public int? RouteId { get; set; }
        public string? Routename { get; set; }
        public int? StoppageId { get; set; }
        public string? Stoppahename { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> TDiscount { get; set; }
        public Nullable<double> NetTransportFee { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Active { get; set; }
        //public int? StudentId { get; set; }
        //public int? ClassId { get; set; }
        //public int? SectionId { get; set; }
        //public int? VehicleId { get; set; }
        //public int? RouteId { get; set; }
        //public int? StoppageId { get; set; }
    }


    public class GetTransportDetailModel
    {
        public int? TSRAId { get; set; }
        public string? Studentname { get; set; }
        public string? SRNo { get; set; }
        public string? Class { get; set; }
        public string? Sectionname { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string? Vehiclename { get; set; }
        public string? Routename { get; set; }
        public string? Stoppagename { get; set; }

    }
    public class StudentDetailModel
    {
        public string? Studentname { get; set; }
        public string? SRNo { get; set; }
        public string? Fathername { get; set; }
        public string? Mothername { get; set; }
        public string? MobileNo { get; set; }
        public string? RollNo { get; set; }


    }

    public class TSectionDataList
    {
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }

    }

    public class TStudentDataList
    {
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? SRNo { get; set; }
    }

    public class GetTClassbySectionNdStudent
    {
        public List<TSectionDataList> TSectionData { get; set; }
        public List<TStudentDataList> TStudentData { get; set; }
    }

    public class TInstallmentList
    {
        public string? InstallmentNo { get; set; }
        //  public Nullable<double> TTotalFee { get; set; }
        public string? MonthName { get; set; }
        public Nullable<double> InstallmentFee { get; set; }
        public Nullable<double> DueFee { get; set; }
    }

    public class TransReceiptList
    {
        public int? TReceiptId { get; set; }
        public string? ReceiptNo { get; set; }
        public string? MonthName { get; set; }
        public string? FeeType { get; set; }
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> PayFee { get; set; }
        public Nullable<double> FeeDiscount { get; set; }
    }

    public class TransStudentDetailModel
    {
        public int? TSRAId { get; set; }
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? SRNo { get; set; }
        public string? Classname { get; set; }
        public string? Sectionname { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string? Vehicleno { get; set; }
        public string? Routename { get; set; }
        public string? Stoppagename { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> NetTransFee { get; set; }
        public Nullable<double> TOldDueFee { get; set; }
        public Nullable<double> LastDueFee { get; set; }
        public Nullable<double> DueFee { get; set; }

        public List<TInstallmentList> TransInatallment { get; set; }
        public List<TransReceiptList> TransReceiptList { get; set; }

    }

    public class TraansportReceiptList
    {
        public Nullable<double> PayFee { get; set; }
        public Nullable<double> FeeDiscount { get; set; }
        public string? FeeType { get; set; }
    }

    public class TransportFeereportModel
    {
        public int? TSRAId { get; set; }
        public int? StudentId { get; set; }
        public string? SRNo { get; set; }
        public string? StudentName { get; set; }
        public string? Classname { get; set; }
        public string? Sectionname { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string? Vehicleno { get; set; }
        public string? Routename { get; set; }
        public string? Stoppagename { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> NetTransFee { get; set; }
        public Nullable<double> TOldDueFee { get; set; }
        public List<TraansportReceiptList> TransportReceipt { get; set; }
        public List<TInstallmentList> TransInatallment { get; set; }

    }

    public class TraansportReceiptListMoldel
    {
        public int TReceiptId { get; set; }
        public string? ReceiptNo { get; set; }
        public string? MonthName { get; set; }
        public string? FeeType { get; set; }
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> FeeDiscount { get; set; }
        public Nullable<double> PayFee { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? Remark { get; set; }

    }


    public class GetTransportFeeDetailModel
    {
        public List<StudentDetailModel> StudentName { get; set; }
        public string? Classname { get; set; }
        public string? Sectionname { get; set; }
        public string? Vehicleno { get; set; }
        public string? Routename { get; set; }
        public string? Stoppagename { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> Discount { get; set; }
        public List<TInstallmentList> TransInatallment { get; set; }
        public List<TraansportReceiptListMoldel> TransportReceiptlist { get; set; }
        public Nullable<double> NetTransFee { get; set; }
        public Nullable<double> TOldDueFee { get; set; }
    }

    public class GetTransPaidoldFeeModel
    {
        public int? TSRAId { get; set; }
        public int? StudentId { get; set; }
        public string? Studentname { get; set; }
        public string? SRNo { get; set; }
        public int? ClassId { get; set; }
        public string? Classname { get; set; }
        public int? SectionId { get; set; }
        public string? Sectionname { get; set; }
        public int? VehicleId { get; set; }
        public string? Vehicleno { get; set; }
        public int? RouteId { get; set; }
        public string? Routename { get; set; }
        public int? StoppageId { get; set; }
        public string? Stoppagename { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> NetTransFee { get; set; }
        public Nullable<double> TOldDueFee { get; set; }
        //  public Nullable<double> PaidOldfee { get; set; }
        public List<TraansportReceiptList> PaidOldfee { get; set; }
    }



}
