using ApiProject.Data;
using ApiProject.Models;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using ApiProject.Service.Current;
using ApiProject.Service.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SchoolController : BaseController
    {


        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        // *********************** School Informaction  ***************************** //
        #region School Insformaction

        [HttpGet("getschooldetails")]
        public async Task<IActionResult> getschooldetails()
        {
            try
            {
                var res = await _schoolService.SchoolDetail();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("getState")]
        public async Task<IActionResult> GetState()
        {
            try
            {
                var res = await _schoolService.GetState();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("getDistrict")]
        public async Task<IActionResult> GetDistrict()
        {
            try
            {
                var res = await _schoolService.GetDistrict();

                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<string>.ErrorResponse("Exception: " + ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("schooldetailupdate")]
        public async Task<IActionResult> schooldetailupdate(SchoolUpdate request)
        {
            try
            {
                var res = await _schoolService.schooldetailupdate(request);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }

        #endregion

        // *********************** user code start ***************************** //
        #region user code

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var res = await _schoolService.GetUserDetail();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserReq req)
        {
            try
            {
                var res = await _schoolService.UpdateUser(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return ErrorRepsponse(ex.Message);
            }
        }

        [HttpGet("changestatusUser")]
        public async Task<IActionResult> changestatusUser(int userId)
        {
            try
            {
                var res = await _schoolService.changestatusUser(userId);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }

        #endregion

        // ***************************** Classs Code ********************** //
        #region Class code 
        [HttpGet("getclassList")]
        public async Task<IActionResult> getclassList()
        {
            try
            {
                var res = await _schoolService.getclassList();
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }

        [HttpPost("insertclass")]
        public async Task<IActionResult> insertclass(AddClassReq req)
        {
            try
            {
                var res = await _schoolService.insertclass(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpPost("updateclass")]
        public async Task<IActionResult> updateclass(UpdateClassReq req)
        {
            try
            {
                var res = await _schoolService.updateclass(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpGet("changestatusclass")]
        public async Task<IActionResult> changestatusclass(int classid)
        {
            try
            {
                var res = await _schoolService.changestatusclass(classid);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }

        #endregion


        // ********************************** section Code ************************ //
        #region Section code 
        [HttpGet("getsection")]
        public async Task<IActionResult> getsection()
        {
            try
            {
                var res = await _schoolService.getsection();
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }

        [HttpPost("insertsection")]
        public async Task<IActionResult> insertsection(AddSectionReq req)
        {
            try
            {
                var res = await _schoolService.insertsection(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpPost("updatesection")]
        public async Task<IActionResult> updatesection(UpdateSectionReq req)
        {
            try
            {
                var res = await _schoolService.updatesection(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpGet("changestatussection")]
        public async Task<IActionResult> changestatussection(int sectionid)
        {
            try
            {
                var res = await _schoolService.changestatussection(sectionid);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }
        #endregion


        // ****************************** subject Code   ********************************** //
        #region  subject Code 
        [HttpGet("getsubject")]
        public async Task<IActionResult> getsubject()
        {
            try
            {
                var res = await _schoolService.getsubject();
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }

        [HttpPost("insertsubject")]
        public async Task<IActionResult> insertsubject(AddSubjectReq req)
        {
            try
            {
                var res = await _schoolService.insertsubject(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpPost("updatesubject")]
        public async Task<IActionResult> updatesubject(UpdateSubjectReq req)
        {
            try
            {
                var res = await _schoolService.updatesubject(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

        [HttpGet("changestatussubject")]
        public async Task<IActionResult> changestatussubject(int subjectid)
        {
            try
            {
                var res = await _schoolService.changestatussubject(subjectid);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return ErrorRepsponse(ex.Message);

            }
        }

        #endregion


        // ******************************** Grate Code Start   ********************************************// 
        #region Grade code 

        [HttpGet("GetGradeList")]
        public async Task<IActionResult> GetGradeList()
        {
            try
            {
                var res = await _schoolService.GetGradeList();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("InsertGrade")]
        public async Task<IActionResult> InsertGrade(AddGradeReq req)
        {
            try
            {
                var res = await _schoolService.insertgrade(req);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost("UpdateGrade")]
        public async Task<IActionResult> UpdateGrade(UpdateGradeReq req)
        {
            try
            {
                var res = await _schoolService.updategrade(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        #endregion


        // ***************************** Event Code Start  *********************** //
        #region  Event Code 

        [HttpGet("GetEvent")]
        public async Task<IActionResult> GetEvent()
        {
            try
            {
                var res = await _schoolService.GetEvent();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertEvent")]
        public async Task<IActionResult> InsertEvent(EventReqModel req)
        {
            try
            {
                var res = await _schoolService.InsertEvent(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(UpdateEventReqModel req)
        {
            try
            {
                var res = await _schoolService.UpdateEvent(req);
                return Ok(res);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ChangeEventStatus")]
        public async Task<IActionResult> ChangeEventStatus(int EventId)
        {
            try
            {
                var res = await _schoolService.ChangeEventStatus(EventId);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        #endregion


        // *************************** Exam Code Start ****************** //
        #region exam code 

        //[HttpGet("GetExam")]
        //public async Task<IActionResult> GetExam()
        //{
        //    try
        //    {
        //        var res = await _schoolService.GetExam();
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("InsertExam")]
        //public async Task<IActionResult> InsertExam(ExamreqModel req)
        //{
        //    try
        //    {
        //        var res = await _schoolService.InsertExam(req);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("UpdateExam")]
        //public async Task<IActionResult> UpdateExam(UpdateExamreqModel req)
        //{
        //    try
        //    {
        //        var res = await _schoolService.UpdateExam(req);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("ChangeExamStatus")]
        //public async Task<IActionResult> ChangeExamStatus(int ExamId)
        //{
        //    try
        //    {
        //        var res = await _schoolService.ExamChangeStatus(ExamId);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }
        //}

        #endregion


        // *************************** Exam Marks Code  ****************** //
        #region exam code 
        //[HttpGet("GetExamMarks")]
        //public async Task<IActionResult> GetExamMarks()
        //{
        //    try
        //    {
        //        var res = await _schoolService.GetExamTotalMarks();
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("InsertExamMarks")]
        //public async Task<IActionResult> InsertExamMarks(ExamMarksModel req)
        //{
        //    try
        //    {
        //        var res = await _schoolService.insertExamTotalMarks(req);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        #endregion

    }
}
