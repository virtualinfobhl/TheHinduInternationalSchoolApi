using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ApiProject.Service.Transport
{
    public interface ITransportService
    {
        // *************************** Driver Start *********************** //
        Task<ApiResponse<List<GetDriverList>>> GetDriverList();
        Task<ApiResponse<bool>> AddDriver(AddDriverreq req);
        Task<ApiResponse<bool>> UpdateDriver(UpdateDriver req);
        Task<ApiResponse<bool>> ChangestatusDriver(int DriverId);


        // *************************** Vehicle Start *********************** //
        Task<ApiResponse<List<GetVehicleList>>> GetVehicle();
        Task<ApiResponse<bool>> AddVehicle(AddVehicleReq req);
        Task<ApiResponse<bool>> UpdateVehicle(UpdateVehicleModel req);
        Task<ApiResponse<bool>> ChangeStatusVehicle(int VehicleId);


        // *************************** Route Start *********************** //
        Task<ApiResponse<List<GetRouteList>>> GetRoute();
        Task<ApiResponse<bool>> AddRoute(AddRouteReq req);
        Task<ApiResponse<bool>> UpdateRoute(UpdateRouteModel req);
        Task<ApiResponse<bool>> ChangeStatusRoute(int RouteId);


        // *************************** Stoppage Start *********************** //
        Task<ApiResponse<List<GetStoppageList>>> GetStoppage();
        Task<ApiResponse<bool>> AddStoppage(AddStoppageReq req);
        Task<ApiResponse<bool>> UpdateStoppage(UpdateStoppageModel req);
        Task<ApiResponse<bool>> ChangeStatusStoppage(int StoppageId);


        // *************************** Transport Fee Start *********************** //
        Task<ApiResponse<List<GetTransportFeeModel>>> GetTransportFee();
        Task<ApiResponse<bool>> AddTransportFee(AddTransportFeeReq req);
        Task<ApiResponse<bool>> UpdateTransportFee(UpdateTransportFee req);
        Task<ApiResponse<bool>> ChangeStatusTransportFee(int TFId);


        // *************************** Transport Vehicle Route Assign Start *********************** //
        Task<ApiResponse<List<GetRouteAssignModel>>> GetRouteAssign();
        Task<ApiResponse<bool>> AddRouteAssign(AddRouteAssignRequest model);
        Task<ApiResponse<bool>> UpdateRouteAssign(UpdateRouteAssignRequest model);

        // *************************** Transport Vehicle Route Assign Start *********************** //
        Task<ApiResponse<List<GetRouteDataModel>>> GetRouteById(int VehicleId);
        Task<ApiResponse<List<UpdateStoppageModel>>> GetStoppageById(int RouteId);
        Task<ApiResponse<UpdateTransportFee>> GetTransportFeeById(int StoppageId);
        Task<ApiResponse<List<GetStuRouteAssignModel>>> GetStudentRouteAssign();
        Task<ApiResponse<bool>> AddStudentRouteAssign(StuRouteAssignReq req);
        Task<ApiResponse<bool>> UpdateStuRouteAssign(UpdateStuRouteAssignReq req);
        Task<ApiResponse<bool>> ChangeStatusStuRouteAssignFee(int TSRAId);


        // *************************** Add Student Transport Fee Start *********************** //

        Task<ApiResponse<GetTClassbySectionNdStudent>> GetTransClassBySectionNdStudent(int ClassId);
        Task<ApiResponse<List<TStudentDataList>>> GetTransSectionByStudentDetail(int ClassId, int SectionId);
        Task<ApiResponse<List<TransStudentDetailModel>>> GetStudentByTransportData(int StudentId);

        Task<ApiResponse<bool>> AddStudentTransportFee(StudentTransportFeeReq req);


        #region Transport Fee Report Start
        // *************************** Transport Fee Report Start *********************** //
        Task<ApiResponse<List<GetTransportDetailModel>>> TransportDetailsReport(TransportDetailReportReq Req);
        Task<ApiResponse<List<TransportFeereportModel>>> TransportFeeReport(TransportFeeReportReq Req);
        Task<ApiResponse<List<GetTransportFeeDetailModel>>> GetTransportFeeDetails(int StudentId);
        Task<ApiResponse<List<GetTransPaidoldFeeModel>>> GetTransportPaidOldFee(int StudentId);
        Task<ApiResponse<bool>> UpdateTransportOldFee (UpdateOldFeeReq req);

        #endregion Transport Fee Report Start

    }
}
