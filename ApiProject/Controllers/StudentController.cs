using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.SchoolFees;
using ApiProject.Service.Student;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System.Text.Json;

//using Newtonsoft.Json;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]

    [Authorize]

    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        //  public object JsonConvert { get; private set; }

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // student details
        [HttpGet("getclass")]
        public async Task<IActionResult> getclass()
        {
            try
            {
                var res = await _studentService.GetClass();

                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetClassByFeendSection")]
        public async Task<IActionResult> GetClassByFeendSection(int ClassId)
        {
            try
            {
                var res = await _studentService.GetClassByFeendSection(ClassId);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetParentsByMobileNo")]
        public async Task<IActionResult> GetParentsByMobileNo(string Mobileno)
        {
            try
            {
                var res = await _studentService.GetParentsByMobileNo(Mobileno);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("addstuquickadmission")]
        public async Task<IActionResult> addstuquickadmission(quickadmissionmodel request)
        {
            try
            {
                var res = await _studentService.AddStuQuickadmission(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("AddStudentAdmission")]
        public async Task<IActionResult> AddStudentAdmission(AddStudentReqModel request)
        {
            try
            {
                var res = await _studentService.AddStudentAdmissionAsync(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("updatestudentdata")]
        public async Task<IActionResult> updatestudentdata([FromBody] StudentUpdateReqModel request)
        {
            try
            {
                var res = await _studentService.updatestudentdata(request);
                return Ok(res);

            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        //[HttpPost("UpdateStuinstallment")]
        //public async Task<IActionResult> UpdateStuinstallment(FeeInstallmentReqMOdel request)
        //{
        //    try
        //    {

        //        var res = await _studentService.UpdateStuinstallment(request);
        //        return Ok(res);

        //    }
        //    catch (Exception ex)
        //    {
        //        var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
        //        return BadRequest(response);
        //    }
        //}


        // For updating student data (JSON only)
        //[HttpPost("updatestudentdata")]
        //public async Task<IActionResult> updatestudentdata([FromBody] StudentUpdateReqModel request)
        //{
        //    try
        //    {
        //        var res = await _studentService.updatestudentdata(request);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
        //        return BadRequest(response);
        //    }
        //}


        [HttpPost("studentexcelupload")]
        public async Task<IActionResult> studentexcelupload([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Invalid file.");

                var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(file.FileName));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var (validList, errorList) = await ReadAndValidateExcel(path);

                ApiResponse<bool> result = null;
                if (validList.Any())
                {

                    result = await _studentService.studentexcelupload(validList);

                }
                var data = new
                {
                    success = result,
                    total = validList.Count + errorList.Count,
                    vaild = validList.Count,
                    failed = errorList.Count,
                    errors = errorList
                };
                var response = ApiResponse<object>.SuccessResponse(data);

                return Ok(response);

            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);
            }
        }

        private async Task<(List<StudentExcelUploadListReq> valid, List<ExcelErrorRow> errors)> ReadAndValidateExcel(string path)
        {
            var validStudents = new List<StudentExcelUploadListReq>();
            var errorList = new List<ExcelErrorRow>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets.First();

            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                var student = new StudentExcelUploadListReq
                {
                    SRNo = sheet.Cells[row, 1].Text.Trim(),
                    ClassName = sheet.Cells[row, 2].Text.Trim(),
                    SectionName = sheet.Cells[row, 3].Text.Trim(),
                    stu_name = sheet.Cells[row, 4].Text.Trim(),
                    father_name = sheet.Cells[row, 5].Text.Trim(),
                    mother_name = sheet.Cells[row, 6].Text.Trim(),
                    father_mobile = sheet.Cells[row, 7].Text.Trim(),
                    DOB = DateTime.TryParse(sheet.Cells[row, 8].Text, out var dob) ? dob : DateTime.Now.Date,
                    Address = sheet.Cells[row, 9].Text.Trim(),
                    RTE = sheet.Cells[row, 10].Text.Trim(),
                    AdmissionPayfee = string.IsNullOrWhiteSpace(sheet.Cells[row, 11].Text) ? 0 : Convert.ToDouble(sheet.Cells[row, 11].Text),
                    Discount = string.IsNullOrWhiteSpace(sheet.Cells[row, 12].Text) ? 0 : Convert.ToDouble(sheet.Cells[row, 12].Text),
                    OldDuefees = string.IsNullOrWhiteSpace(sheet.Cells[row, 13].Text) ? 0 : Convert.ToDouble(sheet.Cells[row, 13].Text),
                    PaymentDate = string.IsNullOrWhiteSpace(sheet.Cells[row, 14].Text) ? DateTime.Now.Date : Convert.ToDateTime(sheet.Cells[row, 14].Text),
                    PaymentMode = string.IsNullOrWhiteSpace(sheet.Cells[row, 15].Text.Trim()) ? "" : sheet.Cells[row, 15].Text.Trim(),
                };

                var errors = StudentValidator.Validate(student);

                if (errors.Any())
                {
                    errorList.Add(new ExcelErrorRow
                    {
                        RowNumber = row,
                        Errors = errors
                    });
                }
                else
                {
                    validStudents.Add(student);
                }
            }

            return (validStudents, errorList);
        }

        //  Student Bulk Edit
        [HttpPost("ShowStudentBulkEdit")]
        public async Task<IActionResult> ShowStudentBulkEdit(BulkStudentReq request)
        {
            try
            {
                var res = await _studentService.ShowStudentBulkEdit(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("updateBulkStudent")]
        public async Task<IActionResult> updateBulkStudent(List<studentRollNoAttendaceReq> request)
        {
            try
            {
                var res = await _studentService.UpdateBulkStudentAsync(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        //  student fee discount 
        [HttpPost("ShowStudentFeeDiscont")]
        public async Task<IActionResult> ShowStudentFeeDiscont(BulkStudentReq request)
        {
            try
            {
                var res = await _studentService.ShowStudentFeeDiscont(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("AddStudentDiscountFee")]
        public async Task<IActionResult> AddStudentDiscountFee(List<studentDiscountfeeReq> request)
        {
            try
            {
                var res = await _studentService.AddStudentDiscountFee(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        // student presonality
        [HttpPost("ShowStudentPersonality")]
        public async Task<IActionResult> ShowStudentPersonality(BulkStudentReq request)
        {
            try
            {
                var res = await _studentService.ShowStudentPersonality(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("addstudentPersonal")]
        public async Task<IActionResult> AddStudentPersonal(List<stuPersonalModelReq> req)
        {
            try
            {
                var res = await _studentService.AddStudentPersonalAsync(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }


        // Exam marks data 
        [HttpGet("GetClassbySectionNdSubject")]
        public async Task<IActionResult> GetClassbySectionNdSubject(int ClassId)
        {
            try
            {
                var res = await _studentService.GetClassbySectionNdSubject(ClassId);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("GetMarksType")]
        public async Task<IActionResult> GetMarksType(int SubjectId)
        {
            try
            {
                var res = await _studentService.GetMarksType(SubjectId);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("getTestMarksDetails")]
        public async Task<IActionResult> getTestMarksDetails(studentexamMarksReq request)
        {
            try
            {
                var res = await _studentService.ShowTestMarksDetails(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("updatestudentMarks")]
        public async Task<IActionResult> updatestudentMarks(UpdateStudentMarksRequest request)
        {
            try
            {
                var res = await _studentService.UpdateStudentMarks(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("MarksExcelUpload")]
        public async Task<IActionResult> MarksExcelUpload(examMarksmodel request)
        {
            try
            {
                var res = await _studentService.ExcelStudentMarks(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }




        // ************ Event Code 
        [HttpGet("GetEventList")]
        public async Task<IActionResult> GetEventList()
        {
            try
            {
                var res = await _studentService.GetEventList();
                return Ok(res);

            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);

            }
        }

        [HttpPost("GetEventDataById")]
        public async Task<IActionResult> GetEventDataById(int EventId)
        {
            try
            {
                var res = await _studentService.GetEventDataByIdAsync(EventId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("AddEventCertificate")]
        public async Task<IActionResult> AddEventCertificate(EventCartificateModelReq req)
        {
            try
            {
                var res = await _studentService.AddEventCertificateAsync(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);

            }
        }

        [HttpGet("GetClassbySectionStudent")]
        public async Task<IActionResult> GetClassbySectionStudent(int ClassId)
        {
            try
            {
                var res = await _studentService.GetClassBySectionNdStudent(ClassId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);

            }
        }

        [HttpGet("GetSectionbyStudentDetails")]
        public async Task<IActionResult> GetSectionbyStudentDetails(int ClassId, int SectionId)
        {
            try
            {
                var res = await _studentService.GetSectionByStudentDetail(ClassId, SectionId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);

            }
        }

        [HttpGet("GetStudentDetailsById")]
        public async Task<IActionResult> GetStudentDetailsById(int StudentId)
        {
            try
            {
                var res = await _studentService.GetStudentDetailById(StudentId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("GetStudentDueFeeTC")]
        public async Task<IActionResult> GetStudentDueFeeTC(int StudentId)
        {
            try
            {
                var res = await _studentService.GetStudentDueFeeTC(StudentId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("GenerateTC")]
        public async Task<IActionResult> GenerateTC(GetStudentTCDropoutReq req)
        {
            try
            {
                var res = await _studentService.GenerateTC(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);

            }
        }
        [HttpPost("StudentDropout")]
        public async Task<IActionResult> StudentDropout(GetStudentTCDropoutReq req)
        {
            try
            {
                var res = await _studentService.StudentDropout(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);

            }
        }









        [HttpGet("GetClassSubject")]
        public async Task<IActionResult> GetClassSubject(int ClassId)
        {
            try
            {
                var res = await _studentService.GetClassSubjectAsync(ClassId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("GetClassExamSubject")]
        public async Task<IActionResult> GetClassExamSubject(ClassExamMarksModelreq request)
        {
            try
            {
                var res = await _studentService.GetClassExamSubjectAsync(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Error: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("getclassbyfee")]
        public async Task<IActionResult> getclassbyfee(int classid)
        {
            try
            {
                var res = await _studentService.GetClassByFee(classid);

                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("getclassbysection")]
        public async Task<IActionResult> getclassbysection(int classid)
        {
            try
            {
                var res = await _studentService.GetClassBySection(classid);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("getSectionbystudent")]
        public async Task<IActionResult> getSectionbystudent(int sectionid)
        {
            try
            {
                var res = await _studentService.GetSectionbyStudent(sectionid);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getstudentlist")]
        public async Task<IActionResult> getstudentlist(getstudentlistReq request)
        {
            try
            {
                var res = await _studentService.GetStudentList(request);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("GetQuickStudentList")]
        public async Task<IActionResult> GetQuickStudentList(getstudentlistReq req)
        {
            try
            {
                var res = await _studentService.GetQuickStudentList(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("GetClassByFeeInstallment")]
        public async Task<IActionResult> GetClassByFeeInstallment(int ClassId)
        {
            try
            {
                var res = await _studentService.GetClassByFeeInsAsync(ClassId);

                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("getstudentdatabyid")]
        public async Task<IActionResult> getstudentdatabyid(int studentid)
        {
            try
            {
                var res = await _studentService.getstudentdatabyid(studentid);

                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }





    }
}
