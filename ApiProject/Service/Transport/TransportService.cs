using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;

namespace ApiProject.Service.Transport
{
    public class TransportService : ITransportService
    {
        private readonly ILoginUserService _loginUser;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransportService(ILoginUserService loginUser, ApplicationDbContext context, IMapper mapper)
        {
            _loginUser = loginUser;
            _context = context;
            _mapper = mapper;
        }

        // *************************** Driver Start *********************** //
        public async Task<ApiResponse<List<GetDriverList>>> GetDriverList()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var driverEntity = await _context.TransDriverTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetDriverList
                    {
                        DriverId = c.DriverId,
                        DriverName = c.Name,
                        Mobileno = c.MobileNo,
                        Address = c.Address,
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<GetDriverList>>.SuccessResponse(driverEntity, "Driver list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetDriverList>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddDriver(AddDriverreq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.TransDriverTbl.FirstOrDefaultAsync(p => p.Name == req.DriverName && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Driver name is already insert");
                }
                int DriverId = _context.TransDriverTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.DriverId) + 1;

                var driverentity = new TransDriverTbl
                {
                    DriverId = DriverId,
                    Name = req.DriverName,
                    MobileNo = req.Mobileno,
                    Address = req.Address,
                    Active = true,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.TransDriverTbl.Add(driverentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Driver  saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateDriver(UpdateDriver req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                TransDriverTbl Driver = await _context.TransDriverTbl.Where(a => a.CompanyId == SchoolId && a.Name == req.DriverName && a.DriverId != req.DriverId).FirstOrDefaultAsync();
                if (Driver != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Driver name already available.");
                }

                var result = await _context.TransDriverTbl.Where(c => c.DriverId == req.DriverId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.Name = req.DriverName;
                result.MobileNo = req.Mobileno;
                result.Address = req.Address;
                result.Userid = UserId;
                result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Driver update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangestatusDriver(int DriverId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var Driverentity = await _context.TransDriverTbl.FirstOrDefaultAsync(p => p.DriverId == DriverId && p.CompanyId == SchoolId);
                if (Driverentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Driver record not found ");
                }

                Driverentity.Active = Driverentity.Active == null ? true : !Driverentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // *************************** Vehicle Start *********************** //
        public async Task<ApiResponse<List<GetVehicleList>>> GetVehicle()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var VehicleEntity = await _context.TransBusTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetVehicleList
                    {
                        VehicleId = c.BusId,
                        VehicleNo = c.VihecleNo,
                        DriverId = c.DriverId,
                        DriverName = _context.TransDriverTbl.Where(a => a.DriverId == c.DriverId).Select(a => a.Name).FirstOrDefault(),
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<GetVehicleList>>.SuccessResponse(VehicleEntity, "Vehicle list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetVehicleList>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddVehicle(AddVehicleReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.TransBusTbl.FirstOrDefaultAsync(p => p.VihecleNo == req.VehicleNo && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Vehicle  is already insert");
                }
                int VehicleId = _context.TransBusTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.BusId) + 1;

                var Vehicleentity = new TransBusTbl
                {
                    BusId = VehicleId,
                    VihecleNo = req.VehicleNo,
                    DriverId = req.DriverId,
                    Active = true,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.TransBusTbl.Add(Vehicleentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Vehicle  saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateVehicle(UpdateVehicleModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                TransBusTbl vehicle = await _context.TransBusTbl.Where(a => a.CompanyId == SchoolId && a.VihecleNo == req.VehicleNo && a.BusId != req.VehicleId).FirstOrDefaultAsync();
                if (vehicle != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Vehicle No already available.");
                }

                var result = await _context.TransBusTbl.Where(c => c.BusId == req.VehicleId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.VihecleNo = req.VehicleNo;
                result.DriverId = req.DriverId;
                result.Userid = UserId;
                result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Vehicle update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeStatusVehicle(int VehicleId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var Vehicleentity = await _context.TransBusTbl.FirstOrDefaultAsync(p => p.BusId == VehicleId && p.CompanyId == SchoolId);
                if (Vehicleentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Vehicle record not found ");
                }

                Vehicleentity.Active = Vehicleentity.Active == null ? true : !Vehicleentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // *************************** Route Start *********************** //
        public async Task<ApiResponse<List<GetRouteList>>> GetRoute()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var RouteEntity = await _context.TransRouteTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetRouteList
                    {
                        RouteId = c.RouteId,
                        RouteName = c.Route,
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<GetRouteList>>.SuccessResponse(RouteEntity, "Route list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetRouteList>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddRoute(AddRouteReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.TransRouteTbl.FirstOrDefaultAsync(p => p.Route == req.RouteName && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Route name is already insert");
                }
                int RouteId = _context.TransRouteTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.RouteId) + 1;

                var Routeentity = new TransRouteTbl
                {
                    RouteId = RouteId,
                    Route = req.RouteName,
                    Active = true,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.TransRouteTbl.Add(Routeentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Route  saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateRoute(UpdateRouteModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                TransRouteTbl route = await _context.TransRouteTbl.Where(a => a.CompanyId == SchoolId && a.Route == req.RouteName && a.RouteId != req.RouteId).FirstOrDefaultAsync();
                if (route != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Route is already available.");
                }

                var result = await _context.TransRouteTbl.Where(c => c.RouteId == req.RouteId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.Route = req.RouteName;
                result.Userid = UserId;
                result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Route update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeStatusRoute(int RouteId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var routeentity = await _context.TransRouteTbl.FirstOrDefaultAsync(p => p.RouteId == RouteId && p.CompanyId == SchoolId);
                if (routeentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Route record not found ");
                }

                routeentity.Active = routeentity.Active == null ? true : !routeentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }


        // *************************** Stoppage Start *********************** //
        public async Task<ApiResponse<List<GetStoppageList>>> GetStoppage()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var StoppageEntity = await _context.TransStoppageTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetStoppageList
                    {
                        StoppageId = c.StoppageId,
                        Stoppage = c.Stoppage,
                        RouteId = c.RouteId,
                        RouteName = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<GetStoppageList>>.SuccessResponse(StoppageEntity, "Stoppage list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStoppageList>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddStoppage(AddStoppageReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.TransStoppageTbl.FirstOrDefaultAsync(p => p.Stoppage == req.Stoppage && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Stoppage  is already insert");
                }
                int StoppageId = _context.TransStoppageTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.StoppageId) + 1;

                var Stoppageentity = new TransStoppageTbl
                {
                    StoppageId = StoppageId,
                    Stoppage = req.Stoppage,
                    RouteId = req.RouteId,
                    Active = true,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.TransStoppageTbl.Add(Stoppageentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Stoppage  saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateStoppage(UpdateStoppageModel req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                TransStoppageTbl stop = await _context.TransStoppageTbl.Where(a => a.CompanyId == SchoolId && a.Stoppage == req.Stoppage && a.StoppageId != req.StoppageId).FirstOrDefaultAsync();
                if (stop != null)
                {
                    return ApiResponse<bool>.ErrorResponse("Stoppage name is already available.");
                }

                var result = await _context.TransStoppageTbl.Where(c => c.StoppageId == req.StoppageId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.Stoppage = req.Stoppage;
                result.RouteId = req.RouteId;
                result.Userid = UserId;
                result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Stoppage update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeStatusStoppage(int StoppageId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var Stoppageentity = await _context.TransStoppageTbl.FirstOrDefaultAsync(p => p.StoppageId == StoppageId && p.CompanyId == SchoolId);
                if (Stoppageentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Stoppage record not found ");
                }

                Stoppageentity.Active = Stoppageentity.Active == null ? true : !Stoppageentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // *************************** Transport Fee Start *********************** //
        public async Task<ApiResponse<List<GetTransportFeeModel>>> GetTransportFee()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var TransportEntity = await _context.TransportFeeTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetTransportFeeModel
                    {
                        TFId = c.TransFeeId,
                        MonthFee = c.MonthFee,
                        StoppageId = c.StopageId,
                        StoppageName = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StopageId).Select(a => a.Stoppage).FirstOrDefault(),
                        Active = c.Active,
                    }).ToListAsync();
                return ApiResponse<List<GetTransportFeeModel>>.SuccessResponse(TransportEntity, "Transport Fee list fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetTransportFeeModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddTransportFee(AddTransportFeeReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var existing = await _context.TransportFeeTbl.FirstOrDefaultAsync(p => p.StopageId == req.StoppageId && p.CompanyId == SchoolId);
                if (existing != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Transport Fee is already available");
                }
                int TransFeeId = _context.TransportFeeTbl.DefaultIfEmpty().Max(s => s == null ? 0 : s.TransFeeId) + 1;

                var TransportFeeentity = new TransportFeeTbl
                {
                    TransFeeId = TransFeeId,
                    StopageId = req.StoppageId,
                    MonthFee = req.MonthFee,
                    Active = true,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId,
                };

                _context.TransportFeeTbl.Add(TransportFeeentity);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Stoppage Fee  saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateTransportFee(UpdateTransportFee req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                //TransportFeeTbl stop = await _context.TransportFeeTbl.Where(a => a.SchoolId == SchoolId && a.MonthFee == req.MonthFee && a.TFId != req.TFId).FirstOrDefaultAsync();
                //if (stop != null)
                //{
                //    return ApiResponse<bool>.ErrorResponse("This Transport Fee  is already available.");
                //}

                var result = await _context.TransportFeeTbl.Where(c => c.TransFeeId == req.TFId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.StopageId = req.StoppageId;
                result.MonthFee = req.MonthFee;
                result.Userid = UserId;
                result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();


                ///var StoppageFee = await _context.
                var StoppageFee = _context.StuRouteAssignTbl.Where(c => c.StoppageId == req.StoppageId && c.CompanyId == SchoolId).ToList();
                if (StoppageFee != null)
                {
                    for (int i = 0; i < StoppageFee.Count; i++)
                    {
                        StoppageFee[i].TransportFee = req.MonthFee;
                        StoppageFee[i].NetTranSFee = req.MonthFee - StoppageFee[i].Discount;
                    }
                    await _context.SaveChangesAsync();
                }

                // Prepare month logic
                int currentMonth = DateTime.Now.Month;
                string currentMonthName = DateTime.Now.ToString("MMMM");

                var TransInstallment = _context.TransInstallmentTbl.Where(p => p.StoppageId == req.StoppageId && p.CompanyId == SchoolId).ToList();

                for (int i = 0; i < TransInstallment.Count; i++)
                {
                    var Tinstall = TransInstallment[i];

                    if (Tinstall.Date.HasValue)
                    {
                        int dateMonth = Tinstall.Date.Value.Month;
                        string dateMonthName = Tinstall.Date.Value.ToString("MMMM");

                        // Build list of month names between Date month and Current month
                        List<string> monthsBetween = new List<string>();
                        int monthPointer = dateMonth;

                        while (true)
                        {
                            monthPointer++;
                            if (monthPointer > 12) monthPointer = 1; // Wrap to Jan

                            if (monthPointer == currentMonth) break; // Stop when reaching current month

                            monthsBetween.Add(new DateTime(2000, monthPointer, 1).ToString("MMMM"));
                        }

                        // Skip update if:
                        // - It's the current month
                        // - Or MonthName is in the monthsBetween list
                        // - Or Date month itself
                        if (Tinstall.Date.Value.Month == currentMonth ||
                            monthsBetween.Contains(Tinstall.MonthName) ||
                            Tinstall.MonthName == dateMonthName)
                        {
                            continue;
                        }
                    }

                    // ✅ Update the record
                    //  Tinstall.StoppageId = model.StopageId;
                    Tinstall.TotalTransFee = req.MonthFee;
                    Tinstall.InstallFee = req.MonthFee;
                    // Tinstall.DueFee = model.MonthFee;
                    Tinstall.Updatedate = DateTime.Now;
                }
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Transport Fee update successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeStatusTransportFee(int TFId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var TransportFeeentity = await _context.TransportFeeTbl.FirstOrDefaultAsync(p => p.TransFeeId == TFId && p.CompanyId == SchoolId);
                if (TransportFeeentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Transport Fee record not found ");
                }

                TransportFeeentity.Active = TransportFeeentity.Active == null ? true : !TransportFeeentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // *************************** Transport Vehicle Route Assign Start *********************** //
        public async Task<ApiResponse<List<GetRouteAssignModel>>> GetRouteAssign()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var RouteAssignEntity = await _context.RouteAssignTbl.Where(c => c.CompanyId == SchoolId)
                    .GroupBy(c => new { c.RAId, c.BusId, c.Active })
                    .Select(g => new GetRouteAssignModel
                    {

                        Vehicleno = _context.TransBusTbl.Where(a => a.BusId == g.Key.BusId && a.CompanyId == SchoolId && a.Active == true).Select(a => a.VihecleNo).FirstOrDefault(),
                        Active = g.Key.Active,
                        Route = g.Select(r => new UpdateRouteModel
                        {
                            RouteId = r.RouteId,
                            RouteName = _context.TransRouteTbl.Where(a => a.RouteId == r.RouteId).Select(a => a.Route).FirstOrDefault(),
                        }).ToList()
                    }).ToListAsync();

                return ApiResponse<List<GetRouteAssignModel>>.SuccessResponse(RouteAssignEntity, "Transport Route Assign fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetRouteAssignModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddRouteAssign(AddRouteAssignRequest model)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                RouteAssignTbl route = await _context.RouteAssignTbl.Where(p => p.BusId == model.Req.VehicleId).FirstOrDefaultAsync();
                if (route != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Vehicle is  already available.");
                }
                else
                {
                    for (int i = 0; i < model.Bus.Count; i++)
                    {
                        RouteAssignTbl routeass = new RouteAssignTbl();
                        routeass.RAId = _context.RouteAssignTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.RAId) + 1;
                        routeass.BusId = model.Req.VehicleId;
                        routeass.RouteId = model.Bus[i].RouteId;
                        routeass.CreateDate = DateTime.Now;
                        routeass.UpdateDate = DateTime.Now;
                        routeass.Active = true;
                        routeass.CompanyId = SchoolId;
                        routeass.SessionId = SessionId;
                        routeass.Userid = UserId;

                        _context.RouteAssignTbl.Add(routeass);
                        await _context.SaveChangesAsync();
                    }
                    return ApiResponse<bool>.SuccessResponse(true, "Route assign saved successfully");
                }
            }
            catch (Exception ex)
            {

                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateRouteAssign(UpdateRouteAssignRequest model)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var RouteAssign = _context.RouteAssignTbl.Where(c => c.BusId == model.VehicleId && c.CompanyId == SchoolId).ToList();
                if (RouteAssign != null)
                {
                    _context.RouteAssignTbl.RemoveRange(RouteAssign);
                    _context.SaveChanges();
                }

                for (int i = 0; i < model.Bus.Count; i++)
                {
                    RouteAssignTbl routeass = new RouteAssignTbl();
                    routeass.RAId = _context.RouteAssignTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.RAId) + 1;
                    routeass.BusId = model.Req.VehicleId;
                    routeass.RouteId = model.Bus[i].RouteId;
                    routeass.CreateDate = DateTime.Now;
                    routeass.UpdateDate = DateTime.Now;
                    routeass.Active = true;
                    routeass.CompanyId = SchoolId;
                    routeass.SessionId = SessionId;
                    routeass.Userid = UserId;

                    _context.RouteAssignTbl.Add(routeass);
                    await _context.SaveChangesAsync();
                }
                return ApiResponse<bool>.SuccessResponse(true, " Update Route assign  successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }



        // *************************** Student Route Assign Fee Start *********************** //
        public async Task<ApiResponse<List<GetRouteDataModel>>> GetRouteById(int VehicleId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.RouteAssignTbl.Where(a => a.BusId == VehicleId && a.CompanyId == SchoolId)
                    .Select(a => new GetRouteDataModel
                    {

                        VehicleId = a.BusId,
                        RouteId = a.RouteId,
                        RouteName = _context.TransRouteTbl.Where(p => p.RouteId == a.RouteId).Select(a => a.Route).FirstOrDefault(),

                    }).ToListAsync();

                return ApiResponse<List<GetRouteDataModel>>.SuccessResponse(res, "Route Data fetched successfully ");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetRouteDataModel>>.ErrorResponse($"Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<List<UpdateStoppageModel>>> GetStoppageById(int RouteId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.TransStoppageTbl.Where(a => a.RouteId == RouteId && a.CompanyId == SchoolId)
                    .Select(a => new UpdateStoppageModel
                    {
                        RouteId = a.RouteId,
                        StoppageId = a.StoppageId,
                        Stoppage = a.Stoppage,
                    }).ToListAsync();

                return ApiResponse<List<UpdateStoppageModel>>.SuccessResponse(res, "Stoppage data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UpdateStoppageModel>>.ErrorResponse("Error : " + ex.Message);
            }
        }
        public async Task<ApiResponse<UpdateTransportFee>> GetTransportFeeById(int StoppageId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.TransportFeeTbl.Where(a => a.StopageId == StoppageId && a.CompanyId == SchoolId)
                    .Select(a => new UpdateTransportFee
                    {
                        TFId = a.TransFeeId,
                        StoppageId = a.StopageId,
                        StoppageName = _context.TransStoppageTbl.Where(c => c.StoppageId == StoppageId && c.CompanyId == SchoolId).Select(c => c.Stoppage).FirstOrDefault(),
                        MonthFee = a.MonthFee,

                    }).FirstOrDefaultAsync();
                return ApiResponse<UpdateTransportFee>.SuccessResponse(res, "Transport Fee data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UpdateTransportFee>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<List<GetStuRouteAssignModel>>> GetStudentRouteAssign()
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int SessionId = _loginUser.SessionId;

                var RouteAssignEntity = await _context.StuRouteAssignTbl.Where(c => c.CompanyId == SchoolId)
                    .Select(c => new GetStuRouteAssignModel
                    {
                        TSRAId = c.StuRouteAssignId,
                        StudentId = c.stu_id,
                        ClassId = c.university_id,
                        SectionId = c.SectionId,
                        VehicleId = c.BusId,
                        RouteId = c.RouteId,
                        StoppageId = c.StoppageId,
                        Studentname = _context.student_admission.Where(a => a.stu_id == c.stu_id).Select(a => a.stu_name).FirstOrDefault(),
                        Class = _context.University.Where(a => a.university_id == c.university_id).Select(a => a.university_name).FirstOrDefault(),
                        Sectionname = _context.collegeinfo.Where(a => a.collegeid == c.SessionId).Select(a => a.collegename).FirstOrDefault(),

                        Vehiclename = _context.TransBusTbl.Where(a => a.BusId == c.BusId).Select(a => a.VihecleNo).FirstOrDefault(),
                        Routename = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                        Stoppahename = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StoppageId).Select(a => a.Stoppage).FirstOrDefault(),
                        TransportFee = c.TransportFee,
                        TDiscount = c.Discount,
                        NetTransportFee = c.NetTranSFee,
                        Active = c.Active,
                        Date = c.Date,
                    }).ToListAsync();
                return ApiResponse<List<GetStuRouteAssignModel>>.SuccessResponse(RouteAssignEntity, "Student Route Assign data  fetched successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetStuRouteAssignModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        //  [HttpPost]
        public async Task<ApiResponse<bool>> AddStudentRouteAssign([FromBody] StuRouteAssignReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                // 🔹 Pehle check karo student already assigned hai ya nahi
                var sroute = await _context.StuRouteAssignTbl
                    .FirstOrDefaultAsync(p => p.stu_id == req.StudentId && p.SessionId == SessionId && p.CompanyId == SchoolId);

                if (sroute != null)
                {
                    return ApiResponse<bool>.ErrorResponse("This Student is already available.");
                }

                // 🔹 Save Route Assign
                var sturoute = new StuRouteAssignTbl
                {
                    StuRouteAssignId = _context.StuRouteAssignTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.StuRouteAssignId) + 1,
                    university_id = req.ClassId,
                    SectionId = req.SectionId,
                    stu_id = req.StudentId,
                    BusId = req.VehicleId,
                    RouteId = req.RouteId,
                    StoppageId = req.StoppageId,
                    TransportFee = req.TransportFee,
                    Discount = req.TDiscount,
                    NetTranSFee = req.NetTransportFee,
                    //  to = req.TOldDueFee ?? 0,
                    Date = req.Date,
                    //     last = 0,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Active = true,
                    CompanyId = SchoolId,
                    SessionId = SessionId,
                    Userid = UserId
                };

                _context.StuRouteAssignTbl.Add(sturoute);
                await _context.SaveChangesAsync();

                // 🔹 Month list (April–March academic year)
                string[] months = new string[]
                {
                    "January","February","March","April","May","June","July" ,"August","September","October","November","December"
                };

                // 🔹 For loop se installments save karna
                for (int i = 0; i < months.Length; i++)
                {
                    TransInstallmentTbl feeInstall = new TransInstallmentTbl
                    {
                        TInstallmentId = _context.TransInstallmentTbl.DefaultIfEmpty().Max(r => r == null ? 0 : r.TInstallmentId) + 1,
                        StuId = req.StudentId,
                        ClassId = req.ClassId,
                        InstallmentNo = (i + 1).ToString(),   // Installment number 1 se start hoga
                        MonthName = months[i],
                        TotalTransFee = (months[i] == "June") ? 0 : req.NetTransportFee,  // June = 0
                        InstallFee = (months[i] == "June") ? 0 : req.NetTransportFee,     // June = 0
                        DueFee = (months[i] == "June") ? 0 : req.NetTransportFee,         // June = 0

                        StoppageId = req.StoppageId,
                        Date = req.Date,
                        SessionId = SessionId,
                        CompanyId = SchoolId,
                        UserId = UserId,
                        Active = true,
                        ReActive = false,
                        CreateDate = DateTime.Now,
                        Updatedate = DateTime.Now
                    };

                    _context.TransInstallmentTbl.Add(feeInstall);
                    await _context.SaveChangesAsync();
                }

                return ApiResponse<bool>.SuccessResponse(true, "Student Route assign & installments saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateStudentRouteAssign(UpdateStuRouteAssignReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;

                StuRouteAssignTbl result = await _context.StuRouteAssignTbl.Where(c => c.StuRouteAssignId == req.TSRAId && c.CompanyId == SchoolId).FirstOrDefaultAsync();

                result.StuRouteAssignId = req.TSRAId;
                result.stu_id = req.StudentId;
                result.university_id = req.ClassId;
                result.SectionId = req.SectionId;
                result.BusId = req.VehicleId;
                result.RouteId = req.RouteId;
                result.StoppageId = req.StoppageId;
                result.TransportFee = req.TransportFee;
                result.Discount = req.TDiscount;
                result.NetTranSFee = req.NetTransportFee;
                result.Date = req.Date;
                result.Userid = UserId;
                result.UpdateDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // Prepare month logic
                int currentMonth = DateTime.Now.Month;
                string currentMonthName = DateTime.Now.ToString("MMMM");

                var TransInstallment = _context.TransInstallmentTbl.Where(p => p.StuId == req.StudentId && p.CompanyId == SchoolId).ToList();

                for (int i = 0; i < TransInstallment.Count; i++)
                {
                    var Tinstall = TransInstallment[i];

                    if (Tinstall.Date.HasValue)
                    {
                        int dateMonth = Tinstall.Date.Value.Month;
                        string dateMonthName = Tinstall.Date.Value.ToString("MMMM");

                        // Build list of month names between Date month and Current month
                        List<string> monthsBetween = new List<string>();
                        int monthPointer = dateMonth;

                        while (true)
                        {
                            monthPointer++;
                            if (monthPointer > 12) monthPointer = 1; // Wrap to Jan

                            if (monthPointer == currentMonth) break; // Stop when reaching current month

                            monthsBetween.Add(new DateTime(2000, monthPointer, 1).ToString("MMMM"));
                        }

                        // Skip update if:
                        // - It's the current month
                        // - Or MonthName is in the monthsBetween list
                        // - Or Date month itself
                        if (Tinstall.Date.Value.Month == currentMonth ||
                            monthsBetween.Contains(Tinstall.MonthName) ||
                            Tinstall.MonthName == dateMonthName)
                        {
                            continue;
                        }
                    }

                    // ✅ Update the record
                    Tinstall.StoppageId = req.StoppageId;
                    Tinstall.TotalTransFee = req.NetTransportFee;
                    Tinstall.InstallFee = req.NetTransportFee;
                    Tinstall.DueFee = req.NetTransportFee;
                    Tinstall.Updatedate = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Student Route Assign update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> ChangeStatusStuRouteAssignFee(int TSRAId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;

                var StuRoouteFeeentity = await _context.StuRouteAssignTbl.FirstOrDefaultAsync(p => p.StuRouteAssignId == TSRAId && p.CompanyId == SchoolId);
                if (StuRoouteFeeentity == null)
                {
                    return ApiResponse<bool>.ErrorResponse("Student Route Assign record not found ");
                }

                StuRoouteFeeentity.Active = StuRoouteFeeentity.Active == null ? true : !StuRoouteFeeentity.Active;

                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "status update successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // *************************** Add Student Transport Fee Start *********************** //
        public async Task<ApiResponse<GetTClassbySectionNdStudent>> GetTransClassBySectionNdStudent(int ClassId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                //var Sectiondatas = await _context.ClassSectionTbl.Where(a => a.ClassId == ClassId && a.SchoolId == SchoolId)
                //    .Select(a => new TSectionDataList
                //    {
                //        SectionId = a.SectionId,
                //        SectionName = _context.collegeinfo.Where(p => p.collegeid == a.SectionId && p.CompanyId == SchoolId).Select(p => p.collegename).FirstOrDefault(),
                //    }).ToListAsync();

                var Sectiondatas = await _context.collegeinfo.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new TSectionDataList
                    {
                        SectionId = a.collegeid,
                        SectionName = a.collegename,
                    }).ToListAsync();


                var StudentDatas = await _context.StuRouteAssignTbl.Where(a => a.university_id == ClassId && a.CompanyId == SchoolId)
                    .Select(a => new TStudentDataList
                    {
                        StudentId = a.stu_id,
                        StudentName = _context.student_admission.Where(p => p.stu_id == a.stu_id && p.CompanyId == SchoolId).Select(p => p.stu_name).FirstOrDefault(),
                    }).OrderBy(a => a.StudentName).ToListAsync();

                var res = new GetTClassbySectionNdStudent
                {
                    TSectionData = Sectiondatas,
                    TStudentData = StudentDatas,
                };

                return ApiResponse<GetTClassbySectionNdStudent>.SuccessResponse(res, "Fetch successfully class by section and student data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetTClassbySectionNdStudent>.ErrorResponse("Error: " + ex.Message);

            }
        }
        public async Task<ApiResponse<List<TStudentDataList>>> GetTransSectionByStudentDetail(int ClassId, int SectionId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StuRouteAssignTbl.Where(a => a.university_id == ClassId && a.SectionId == SectionId && a.CompanyId == SchoolId)
                    .Select(a => new TStudentDataList
                    {

                        StudentId = a.stu_id,
                        StudentName = _context.student_admission.Where(p => p.stu_id == a.stu_id && p.CompanyId == SchoolId).Select(p => p.stu_name).FirstOrDefault(),
                    }).OrderBy(a => a.StudentName).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<TStudentDataList>>.ErrorResponse("No Fetch section by student ");
                }


                return ApiResponse<List<TStudentDataList>>.SuccessResponse(res, "Fetch successfully section by student");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TStudentDataList>>.ErrorResponse("Error: " + ex.Message);

            }
        }
        public async Task<ApiResponse<List<TransStudentDetailModel>>> GetStudentByTransportData(int StudentId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                // ==== 1. GET Start Month FROM StuRouteAssignTbl ====
                var startDate = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId
                && c.SessionId == SessionId).Select(c => c.Date).FirstOrDefaultAsync();

                int startMonthNo = startDate?.Month ?? 1;           // route date ka month number (int)
                string startMonth = new DateTime(DateTime.Now.Year, startMonthNo, 1).ToString("MMMM");

                int currentMonthNo = DateTime.Now.Month;           // current month number (int)
                string currentMonth = new DateTime(DateTime.Now.Year, currentMonthNo, 1).ToString("MMMM");

                var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1).Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMMM")).ToList();

                var res = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId)
                    .Select(c => new TransStudentDetailModel
                    {
                        TSRAId = c.StuRouteAssignId,
                        StudentId = c.stu_id,
                        TransportFee = c.TransportFee,
                        Discount = c.Discount,
                        NetTransFee = c.NetTranSFee,
                        //  TOldDueFee = c.TOldDueFee,
                        //   LastDueFee = c.LastDueFee,
                        Date = c.Date,
                        StudentName = _context.student_admission.Where(a => a.stu_id == c.stu_id).Select(a => a.stu_name).FirstOrDefault(),
                        SRNo = _context.student_admission.Where(a => a.stu_id == c.stu_id).Select(a => a.registration_no).FirstOrDefault(),
                        Classname = _context.University.Where(a => a.university_id == c.university_id).Select(a => a.university_name).FirstOrDefault(),
                        Sectionname = _context.collegeinfo.Where(a => a.collegeid == c.SessionId).Select(a => a.collegename).FirstOrDefault(),

                        Vehicleno = _context.TransBusTbl.Where(a => a.BusId == c.BusId).Select(a => a.VihecleNo).FirstOrDefault(),
                        Routename = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                        Stoppagename = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StoppageId).Select(a => a.Stoppage).FirstOrDefault(),

                        //DueFee = _context.TransInstallmentTbl.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId
                        //&& validMonths.Contains(a.MonthName)).Select(a => a.DueFee).Sum().FirstOrDefault(),
                        DueFee = _context.TransInstallmentTbl.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId && validMonths.Contains(a.MonthName))
                        .Sum(a => a.DueFee),

                        TransInatallment = _context.TransInstallmentTbl.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId
                        && validMonths.Contains(a.MonthName)).Select(a => new TInstallmentList
                        {
                            InstallmentFee = a.InstallFee,
                            InstallmentNo = a.InstallmentNo,
                            // TTotalFee = a.TotalTransFee,
                            MonthName = a.MonthName,
                            DueFee = a.DueFee,

                        }).ToList(),

                        TransReceiptList = _context.NewTransportFeeTbl.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId)
                        .Select(a => new TransReceiptList
                        {
                            TReceiptId = a.NewPaymentId,
                            ReceiptNo = a.ReceiptNo,
                            MonthName = a.MonthName,
                            FeeType = a.FeeType,
                            TotalFee = a.NetTransFee,
                            PayFee = a.PayFee,
                            FeeDiscount = a.Paydiscount,
                        }).ToList(),

                    }).ToListAsync();

                return ApiResponse<List<TransStudentDetailModel>>.SuccessResponse(res, "Fetch successfully Transport Student data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TransStudentDetailModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }



        // *************************** Transport Fee Report Start *********************** //
        public async Task<ApiResponse<List<GetTransportDetailModel>>> TransportDetailsReport(TransportDetailReportReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StuRouteAssignTbl.Where(c => (req.ClassId == -1 ? true : c.university_id == req.ClassId)
                 && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.RouteId == -1 ? true : c.RouteId == req.RouteId)
                 && c.Active == true && c.CompanyId == SchoolId).Select(c => new GetTransportDetailModel
                 {
                     TSRAId = c.StuRouteAssignId,
                     Date = c.Date,
                     Studentname = _context.student_admission.Where(a => a.stu_id == c.stu_id).Select(a => a.stu_name).FirstOrDefault(),
                     SRNo = _context.student_admission.Where(a => a.stu_id == c.stu_id).Select(a => a.registration_no).FirstOrDefault(),
                     Class = _context.University.Where(a => a.university_id == c.university_id).Select(a => a.university_name).FirstOrDefault(),
                     Sectionname = _context.collegeinfo.Where(a => a.collegeid == c.SectionId).Select(a => a.collegename).FirstOrDefault(),

                     Vehiclename = _context.TransBusTbl.Where(a => a.BusId == c.BusId).Select(a => a.VihecleNo).FirstOrDefault(),
                     Routename = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                     Stoppagename = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StoppageId).Select(a => a.Stoppage).FirstOrDefault(),

                     //StudentDetail = _context.studentTbl.Where(a => a.StudentId == c.StudentId && a.SchoolId == SchoolId)
                     //.Select(a => new StudentDetailModel
                     //{
                     //    Studentname = a.stu_name,
                     //    SRNo = a.SRNo,
                     //    Fathername = a.father_name,
                     //    Mothername = a.mother_name,
                     //    MobileNo = a.father_mobile,

                     //}).ToList(),

                 }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<GetTransportDetailModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<GetTransportDetailModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {

                return ApiResponse<List<GetTransportDetailModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }
        public async Task<ApiResponse<List<TransportFeereportModel>>> TransportFeeReport(TransportFeeReportReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                //// ==== 1. GET Start Month FROM StuRouteAssignTbl ====
                //var startDate = await _context.StuRouteAssignTbl.Where(c => c.stu_id == req.StudentId && c.CompanyId == SchoolId
                //&& c.SessionId == SessionId).Select(c => c.Date).FirstOrDefaultAsync();

                //int startMonthNo = startDate?.Month ?? 1;           // route date ka month number (int)
                //string startMonth = new DateTime(DateTime.Now.Year, startMonthNo, 1).ToString("MMMM");

                //int currentMonthNo = DateTime.Now.Month;           // current month number (int)
                //string currentMonth = new DateTime(DateTime.Now.Year, currentMonthNo, 1).ToString("MMMM");

                //var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1).Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMMM")).ToList();



                var res = await _context.StuRouteAssignTbl.Where(c => (req.ClassId == -1 ? true : c.university_id == req.ClassId)
                 && (req.SectionId == -1 ? true : c.SectionId == req.SectionId) && (req.StudentId == -1 ? true : c.stu_id == req.StudentId)
                 && (req.VehicleId == -1 ? true : c.BusId == req.VehicleId) && c.Active == true && c.CompanyId == SchoolId)
                    .Select(c => new TransportFeereportModel
                    {
                        TSRAId = c.StuRouteAssignId,
                        StudentId = c.stu_id,
                        StudentName = _context.student_admission.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId).Select(a => a.stu_name).FirstOrDefault(),
                        SRNo = _context.student_admission.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId).Select(a => a.registration_no).FirstOrDefault(),
                        Classname = _context.University.Where(a => a.university_id == c.university_id).Select(a => a.university_name).FirstOrDefault(),
                        Sectionname = _context.collegeinfo.Where(a => a.collegeid == c.SessionId).Select(a => a.collegename).FirstOrDefault(),

                        Vehicleno = _context.TransBusTbl.Where(a => a.BusId == c.BusId).Select(a => a.VihecleNo).FirstOrDefault(),
                        Routename = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                        Stoppagename = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StoppageId).Select(a => a.Stoppage).FirstOrDefault(),
                        TransportFee = c.TransportFee,
                        Discount = c.Discount,
                        NetTransFee = c.NetTranSFee,
                        //TOldDueFee = c.TOldDueFee,
                        Date = c.Date,

                        TransportReceipt = _context.NewTransportFeeTbl.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId)
                        .Select(a => new TraansportReceiptList
                        {
                            PayFee = a.PayFee,
                            FeeDiscount = a.Paydiscount,
                            FeeType = a.FeeType,
                        }).ToList(),

                        TransInatallment = _context.TransInstallmentTbl.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId
                       /* && validMonths.Contains(a.MonthName)*/).Select(a => new TInstallmentList
                                                                {
                                                                    InstallmentFee = a.InstallFee,
                                                                    InstallmentNo = a.InstallmentNo,
                                                                    DueFee = a.DueFee,
                                                                    MonthName = a.MonthName,

                                                                }).ToList(),

                    }).ToListAsync();

                if (res == null || !res.Any())
                {
                    return ApiResponse<List<TransportFeereportModel>>.ErrorResponse("No student fee found data");
                }

                return ApiResponse<List<TransportFeereportModel>>.SuccessResponse(res, "Fetch student fee data successfully");
            }
            catch (Exception ex)
            {

                return ApiResponse<List<TransportFeereportModel>>.ErrorResponse("Something went wrong: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddStudentTransportFee(StudentTransportFeeReq req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            {
                try
                {
                    int SchoolId = _loginUser.SchoolId;
                    int UserId = _loginUser.UserId;
                    int SessionId = _loginUser.SessionId;

                    if (req.PayFee > 0)
                    {
                        NewTransportFeeTbl Receipt = new NewTransportFeeTbl();

                        // Safe TReceiptId calculation
                        Receipt.NewPaymentId = (_context.NewTransportFeeTbl.Any() ? _context.NewTransportFeeTbl.Max(r => r.NewPaymentId) : 0) + 1;
                        institute GetInstituteCodeName = _context.institute.Where(i => i.institute_id == SchoolId).FirstOrDefault();
                        string threeLetters = GetInstituteCodeName.instituteCode.Substring(0, 3).ToUpper();
                        // Generate ReceiptNo
                        var lastReceipt = _context.NewTransportFeeTbl.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.NewPaymentId).FirstOrDefault();
                        string ReceiptCode = "";
                        var Id = 0;
                        if (lastReceipt != null)
                        {
                            var Receipts = lastReceipt.ReceiptNo.Split('/');
                            ReceiptCode = Receipts[1];

                            Id = int.Parse(ReceiptCode);
                            Id++;
                        }
                        else
                        {
                            Id = 1;
                        }
                        ReceiptCode = threeLetters + "/" + Id;
                       
                        int NewOrderNo = 1;

                        var LastOrderNo = _context.NewTransportFeeTbl.Where(s => s.CompanyId == SchoolId && s.SessionId == SessionId).Select(s => s.OrderNo).ToList()
                            .Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).OrderByDescending(x => x).FirstOrDefault();

                        if (LastOrderNo > 0)
                        {
                            NewOrderNo = LastOrderNo + 1;
                        }

                        // Assign properties
                        Receipt.StuRouteAssignId = req.TSRAId;
                        Receipt.ReceiptNo = ReceiptCode;
                        Receipt.stu_id = req.StudentId;
                        Receipt.university_id = req.ClassId;
                        Receipt.SectionId = req.SectionId;

                        Receipt.BusId = req.VehicleId;
                        Receipt.RouteId = req.RouteId;
                        Receipt.StoppageId = req.StoppageId;

                        Receipt.TransFee = _context.StuRouteAssignTbl.Where(a => a.StuRouteAssignId == req.TSRAId && a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => a.TransportFee).FirstOrDefault();
                        Receipt.Discount = _context.StuRouteAssignTbl.Where(a => a.StuRouteAssignId == req.TSRAId && a.CompanyId == SchoolId && a.SessionId == SessionId).Select(a => a.Discount).FirstOrDefault();
                        Receipt.StoppageId = req.StoppageId;
                        Receipt.MonthType = req.MonthType;
                        Receipt.MonthName = req.MonthName;
                        Receipt.NetTransFee = req.TotalFee;
                        Receipt.Paydiscount = req.FeeDiscount;
                        Receipt.PayFee = req.PayFee;
                        Receipt.DueFee = req.TotalFee - (req.FeeDiscount + req.PayFee);
                        Receipt.Cash = req.Cash;
                        Receipt.UPI = req.UPI;
                        Receipt.Date = req.PaymentDate;
                        Receipt.PaymentMode = req.PaymentMode;
                        Receipt.Remark = req.Remark;
                        Receipt.FeeType = "TransportFee";
                        Receipt.OrderNo = NewOrderNo.ToString();
                        Receipt.OrderStatus = "Succcessfully";
                        Receipt.TransactionId = "";
                        Receipt.ReceiptType = "Offline";
                        Receipt.Active = true;
                        Receipt.Date = DateTime.Now;
                        Receipt.CreateDate = DateTime.Now;
                        Receipt.UpdateDate = DateTime.Now;
                        Receipt.CompanyId = SchoolId;
                        Receipt.Userid = UserId;
                        Receipt.SessionId = SessionId;

                        _context.NewTransportFeeTbl.Add(Receipt);
                        await _context.SaveChangesAsync();

                        var sturouteasign = _context.StuRouteAssignTbl.Where(s => s.stu_id == req.StudentId && s.SessionId == SessionId && s.CompanyId == SchoolId).FirstOrDefault();
                        sturouteasign.TTransportFee = (sturouteasign.TTransportFee ?? 0) + req.TotalFee;
                        sturouteasign.TDueFee = Receipt.DueFee;
                        sturouteasign.TPayDiscount = (sturouteasign.TPayDiscount ?? 0) + req.FeeDiscount;
                        sturouteasign.TPayFee = (sturouteasign.TPayFee ?? 0) + req.PayFee;

                        await _context.SaveChangesAsync();

                        // ev.MonthName को ',' से विभाजित करके महीनों की सूची प्राप्त करें
                        var months = req.MonthName.Split(',').Select(m => m.Trim()).ToList();

                        // प्रत्येक महीने के लिए संबंधित TransInstallmentTbl प्रविष्टियों को प्राप्त करें
                        var installments = _context.TransInstallmentTbl.Where(u => u.StuId == req.StudentId && u.ClassId == req.ClassId && u.SessionId == SessionId &&
                         u.CompanyId == SchoolId && months.Contains(u.MonthName)).ToList();

                        for (int i = 0; i < installments.Count; i++)
                        {
                            installments[i].ReActive = true;
                        }
                        await _context.SaveChangesAsync();

                        // ev.PayFee को double में कन्वर्ट करें, यदि null है तो 0.0 मान लें
                        double remainingPay = (req.PayFee ?? 0) + (req.FeeDiscount ?? 0);

                        // सभी सक्रिय इंस्टॉलमेंट्स को प्राप्त करें, जिन्हें पहले सक्रिय किया गया है
                        var activeInstallments = _context.TransInstallmentTbl.Where(u => u.StuId == req.StudentId && u.ClassId == req.ClassId && u.SessionId == SessionId &&
                            u.CompanyId == SchoolId && u.ReActive == true).OrderBy(u => u.MonthName).ToList();

                        // प्रत्येक इंस्टॉलमेंट के लिए DueFee को घटाएं जब तक कि remainingPay समाप्त न हो जाए
                        for (int i = 0; i < activeInstallments.Count && remainingPay > 0; i++)
                        {
                            var installment = activeInstallments[i];
                            if (installment.DueFee.HasValue && installment.DueFee.Value > 0)
                            {
                                double dueFee = installment.DueFee.Value;
                                if (remainingPay >= dueFee)
                                {
                                    remainingPay -= dueFee;
                                    installment.DueFee = 0;
                                }
                                else
                                {
                                    installment.DueFee = dueFee - remainingPay;
                                    remainingPay = 0;
                                }
                                await _context.SaveChangesAsync();

                            }
                        }
                        await transaction.CommitAsync();
                        return ApiResponse<bool>.SuccessResponse(true, "Student Route assign & installments saved successfully");
                    }

                    // Agar PayFee <= 0 hu
                    return ApiResponse<bool>.ErrorResponse("Invalid request: PayFee must be greater than 0");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
                }
            }
        }

        public async Task<ApiResponse<List<GetTransportFeeDetailModel>>> GetTransportFeeDetails(int StudentId)

        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                // ==== 1. GET Start Month FROM StuRouteAssignTbl ====
                var startDate = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId
                && c.SessionId == SessionId).Select(c => c.Date).FirstOrDefaultAsync();

                int startMonthNo = startDate?.Month ?? 1;           // route date ka month number (int)
                string startMonth = new DateTime(DateTime.Now.Year, startMonthNo, 1).ToString("MMMM");

                int currentMonthNo = DateTime.Now.Month;           // current month number (int)
                string currentMonth = new DateTime(DateTime.Now.Year, currentMonthNo, 1).ToString("MMMM");

                var validMonths = Enumerable.Range(startMonthNo, currentMonthNo - startMonthNo + 1).Select(m => new DateTime(DateTime.Now.Year, m, 1).ToString("MMMM")).ToList();

                var res = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId)
                    .Select(c => new GetTransportFeeDetailModel
                    {

                        StudentName = _context.StudentRenewView.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId)
                        .Select(a => new StudentDetailModel
                        {
                            Studentname = a.stu_name,
                            SRNo = a.registration_no,
                            Fathername = a.father_name,
                            Mothername = a.mother_name,
                            MobileNo = a.mother_mobile,
                            RollNo = a.RollNo,
                        }).ToList(),

                        TransportFee = c.TransportFee,
                        Discount = c.Discount,

                        Classname = _context.University.Where(a => a.university_id == c.university_id).Select(a => a.university_name).FirstOrDefault(),
                        Sectionname = _context.collegeinfo.Where(a => a.collegeid == c.SectionId).Select(a => a.collegename).FirstOrDefault(),
                        Vehicleno = _context.TransBusTbl.Where(a => a.BusId == c.BusId).Select(a => a.VihecleNo).FirstOrDefault(),
                        Routename = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                        Stoppagename = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StoppageId).Select(a => a.Stoppage).FirstOrDefault(),

                        TransInatallment = _context.TransInstallmentTbl.Where(a => a.StuId == c.stu_id && a.CompanyId == SchoolId && validMonths.Contains(a.MonthName))
                        .Select(a => new TInstallmentList
                        {
                            InstallmentFee = a.InstallFee,
                            InstallmentNo = a.InstallmentNo,
                            DueFee = a.DueFee,
                            MonthName = a.MonthName,

                        }).ToList(),

                        TransportReceiptlist = _context.NewTransportFeeTbl.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId)
                        .Select(a => new TraansportReceiptListMoldel
                        {
                            TReceiptId = a.NewPaymentId,
                            ReceiptNo = a.ReceiptNo,
                            MonthName = a.MonthName,
                            FeeType = a.FeeType,
                            TotalFee = a.NetTransFee,
                            PayFee = a.PayFee,
                            FeeDiscount = a.Discount,
                            PaymentDate = a.Date,
                            Remark = a.Remark,
                        }).Distinct().ToList(),


                        //  NetTransFee = c.NetTransportFee,
                        //  TOldDueFee = c.TOldDueFee,


                    }).ToListAsync();

                return ApiResponse<List<GetTransportFeeDetailModel>>.SuccessResponse(res, "Fetch successfully Transport fee details data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetTransportFeeDetailModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }


        public async Task<ApiResponse<List<GetTransPaidoldFeeModel>>> GetTransportPaidOldFee(int StudentId)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                var res = await _context.StuRouteAssignTbl.Where(c => c.stu_id == StudentId && c.CompanyId == SchoolId)
                    .Select(c => new GetTransPaidoldFeeModel
                    {

                        TSRAId = c.StuRouteAssignId,
                        StudentId = c.stu_id,
                        ClassId = c.university_id,
                        SectionId = c.SectionId,
                        VehicleId = c.BusId,
                        RouteId = c.RouteId,
                        StoppageId = c.StoppageId,
                        Studentname = _context.student_admission.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId).Select(a => a.stu_name).FirstOrDefault(),
                        SRNo = _context.student_admission.Where(a => a.stu_id == c.stu_id && a.CompanyId == SchoolId).Select(a => a.registration_no).FirstOrDefault(),
                        Classname = _context.University.Where(a => a.university_id == c.university_id).Select(a => a.university_name).FirstOrDefault(),
                        Sectionname = _context.collegeinfo.Where(a => a.collegeid == c.SessionId).Select(a => a.collegename).FirstOrDefault(),

                        Vehicleno = _context.TransBusTbl.Where(a => a.BusId == c.BusId).Select(a => a.VihecleNo).FirstOrDefault(),
                        Routename = _context.TransRouteTbl.Where(a => a.RouteId == c.RouteId).Select(a => a.Route).FirstOrDefault(),
                        Stoppagename = _context.TransStoppageTbl.Where(a => a.StoppageId == c.StoppageId).Select(a => a.Stoppage).FirstOrDefault(),
                        PaidOldfee = _context.NewTransportFeeTbl.Where(a => a.stu_id == c.stu_id && a.FeeType == "TransportOldFee")
                        .Select(a => new TraansportReceiptList
                        {
                            PayFee = a.PayFee,
                            FeeDiscount = a.Discount,
                            FeeType = a.FeeType,
                        }).ToList(),
                        TransportFee = c.TransportFee,
                        Discount = c.Discount,
                        //   NetTransFee = c.NetTransportFee,
                        //  TOldDueFee = c.TOldDueFee,

                    }).ToListAsync();

                return ApiResponse<List<GetTransPaidoldFeeModel>>.SuccessResponse(res, "Fetch successfully Transport Paid Old Fee data ");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetTransPaidoldFeeModel>>.ErrorResponse("Error: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateTransportOldFee(UpdateOldFeeReq req)
        {
            try
            {
                int SchoolId = _loginUser.SchoolId;
                int UserId = _loginUser.UserId;
                int SessionId = _loginUser.SessionId;

                if (req.PayFee > 0)
                {
                    NewTransportFeeTbl Receipt = new NewTransportFeeTbl();

                    // Safe TReceiptId calculation
                    Receipt.NewPaymentId = (_context.NewTransportFeeTbl.Any() ? _context.NewTransportFeeTbl.Max(r => r.NewPaymentId) : 0) + 1;

                    // Generate ReceiptNo
                    var existingReceipt = _context.NewTransportFeeTbl.Where(s => s.CompanyId == SchoolId).OrderByDescending(s => s.NewPaymentId).FirstOrDefault();

                    if (existingReceipt == null || !int.TryParse(existingReceipt.ReceiptNo, out int lastNo))
                    {
                        Receipt.ReceiptNo = "1";
                    }
                    else
                    {
                        Receipt.ReceiptNo = (lastNo + 1).ToString();
                    }

                    // Assign properties
                    Receipt.StuRouteAssignId = req.TSRAId;
                    Receipt.stu_id = req.StudentId;
                    Receipt.university_id = req.ClassId;
                    Receipt.SectionId = req.SectionId;
                    Receipt.BusId = req.VehicleId;
                    Receipt.RouteId = req.RouteId;
                    Receipt.StoppageId = req.StoppageId;
                    Receipt.Discount = req.FeeDiscount;
                    Receipt.PayFee = req.PayFee;
                    Receipt.Cash = req.Cash;
                    Receipt.UPI = req.UPI;
                    Receipt.Date = req.PaymentDate;
                    Receipt.PaymentMode = req.PaymentMode;
                    Receipt.Remark = req.Remark;
                    Receipt.FeeType = "TransportOldFee";
                    Receipt.Active = true;
                    Receipt.Date = DateTime.Now;
                    Receipt.CreateDate = DateTime.Now;
                    Receipt.UpdateDate = DateTime.Now;
                    Receipt.CompanyId = SchoolId;
                    Receipt.Userid = UserId;
                    Receipt.SessionId = SessionId;

                    _context.NewTransportFeeTbl.Add(Receipt);
                    await _context.SaveChangesAsync();


                    //var sturouteasign = _context.StuRouteAssignTbl.FirstOrDefault(s => s.StudentId == req.StudentId && s.SessionId == SessionId && s.SchoolId == SchoolId);
                    //sturouteasign.LastDueFee = req.DueFee;
                    //await _context.SaveChangesAsync();

                    return ApiResponse<bool>.SuccessResponse(true, "Student Transport  Pay Old Fee saved successfully");
                }

                // Agar PayFee <= 0 hua
                return ApiResponse<bool>.ErrorResponse("Invalid request: PayFee must be greater than 0");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse("Error : " + ex.Message);
            }
        }

        #region GetEmployee


        #endregion GetEmployee

    }
}
