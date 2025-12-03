using ApiProject.Data;
using ApiProject.Models.Request;
using ApiProject.Models.Response;
using AutoMapper;

namespace ApiProject.Helper.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<StudentRenewView, stduentlistres>();
            CreateMap<StudentRenewView, GetQuickStudentReqModel>();
            CreateMap<StudentRenewView, GetStudentQuickListModel>();
            CreateMap<StudentRenewView, GetStudentFeeListModel>();
            CreateMap<student_admission, getStudentListModel>();
            CreateMap<fee_installment, FeeInstallmentDto>();
            CreateMap<M_FeeDetail, AdmissionFeeReceiptDto>();
            CreateMap<University, ClassALLReqModel>();
            CreateMap<Subject, SubjectResModel>();
            CreateMap<collegeinfo, ClassSectionResModel>();
         //   CreateMap<ClassSectionTbl, ClassSectionlistReqModel>();
            CreateMap<GradeInfo, GradeReqModel>();
            CreateMap<fees, ClassFeesRes>();
            CreateMap<StudentRenewView, ClassIdByStudentRes>();
            CreateMap<StudentRenewView, StudentFeesDetailRes>();
            CreateMap<ExamTbl, GetExamReqModel>();
            CreateMap<District, DistrictResModel>();
            CreateMap<Event, GetEventReqModel>();
            CreateMap<ClassSubjectExamTbl, GetClassbysubjectMarksmodel>();

        }
    }
}
