namespace ApiProject.Models.Request
{
    public class SchoolUpdate
    {
        //   public string schoolname { get; set; }
        //   public string ownername { get; set; }
        //   public Nullable<System.DateTime> rgstdate { get; set; }
        public string? Registrationno { get; set; }
        public string? InstituteCode { get; set; }
        public string? rgstdate { get; set; }
        public string? email { get; set; }
        public string? landlinenum { get; set; }
        public string? mobileno1 { get; set; }
        public string? mobileno2 { get; set; }
        public string? weburl { get; set; }
        public string? ServiceTaxNo { get; set; }
        public string? TINno { get; set; }
        public string? PANno { get; set; }
        public string? statename { get; set; }
        public string? districtname { get; set; }
        public string? cityname { get; set; }
        public string? pincode { get; set; }
        public string? address { get; set; }
        public IFormFile? logo { get; set; }
        public IFormFile? RLogo { get; set; }

        //    public string? logoimg { get; set; }
        //    public string? rlogo { get; set; }
        //    public Nullable<bool> companyactive { get; set; }
        //    public Nullable<System.DateTime> joiningdate { get; set; }
        //    public Nullable<System.DateTime> expiredate { get; set; }
        //    public Nullable<bool> active { get; set; }
    }


    public class GetUserReqmodel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? MobileNo { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
        public bool? Active { get; set; }
    }

    public class UpdateUserReq
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
    }

    public class UpUserReqModel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
    }
    // class 
    public class AddClassReq
    {
        public string? university_name { get; set; }
    }

    public class UpdateClassReq
    {
        public int university_id { get; set; }
        public string? university_name { get; set; }
    }
    public class ClassALLReqModel
    {
        //public int ClassId { get; set; }
        //public string? ClassName { get; set; }
        //public Nullable<bool> Active { get; set; }
        public int university_id { get; set; }
        public string? university_name { get; set; }
        public Nullable<bool> Active { get; set; }

    }

    // section
    public class AddSectionReq
    {
        public int? ClassId { get; set; }
        public string? SectionName { get; set; }

    }

    public class UpdateSectionReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }

    }

    // Grade 

    public class GradeReqModel
    {
        public int grade_id { get; set; }
        public string? grade_name { get; set; }
        public string? Percent_Upto { get; set; }
        public string? Percent_From { get; set; }
        public Nullable<bool> Active { get; set; }
    }
    public class AddGradeReq
    {
        public string? grade_name { get; set; }
        public string? Percent_Upto { get; set; }
        public string? Percent_From { get; set; }
    }

    public class UpdateGradeReq
    {
        public int grade_id { get; set; }
        public string? grade_name { get; set; }
        public string? Percent_Upto { get; set; }
        public string? Percent_From { get; set; }
    }


    public class GetExamReqModel
    {
        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public int? ExamPriority { get; set; }
        public Nullable<bool> EActive { get; set; }
    }

    public class ExamreqModel
    {
        public string? ExamName { get; set; }
        public int? ExamPriority { get; set; }
    }
    public class UpdateExamreqModel
    {
        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public int? ExamPriority { get; set; }
    }

    public class AddSubjectReq
    {
        public int? ClassId { get; set; }
        public string? SubjectName { get; set; }
        public Nullable<int> SubjectPriority { get; set; }
        public string? Marks_Type { get; set; }
        public int? Quarterly { get; set; }
        public int? first_test { get; set; }
        public int? second_test { get; set; }
        public int? third_test { get; set; }
        public int? fourth_test { get; set; }
        public int? half_yearly { get; set; }
        public int? yearly { get; set; }

    }

    public class UpdateSubjectReq
    {
        public int SubjectId { get; set; }
        public int? ClassId { get; set; }
        public string? SubjectName { get; set; }
        public Nullable<int> SubjectPriority { get; set; }
        public string? Marks_Type { get; set; }
        public int? Quarterly { get; set; }
        public int? first_test { get; set; }
        public int? second_test { get; set; }
        public int? third_test { get; set; }
        public int? fourth_test { get; set; }
        public int? half_yearly { get; set; }
        public int? yearly { get; set; }

    }



    public class SubjectReqModel
    {
        public int? ClassId { get; set; }
        public string? SubjectName { get; set; }
        //    public Nullable<bool> SbjActive { get; set; }
        public Nullable<int> SubjectPriority { get; set; }

    }

    public class UpdateSubjectReqModel
    {

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Nullable<int> SubjectPriority { get; set; }

    }

    public class GetEventReqModel
    {
        public int? EventId { get; set; }
        public string? Eventname { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class EventReqModel
    {
        public string? Eventname { get; set; }
        // public int EventId { get; set; }
        //   public Nullable<bool> Active { get; set; }
    }

    public class UpdateEventReqModel
    {
        public int EventId { get; set; }
        public string? Eventname { get; set; }

    }

    public class SectionDataList
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }

    }

    public class StudentDataList
    {
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public int? SectionId { get; set; }
    }

    public class GetClassbySectionNdStudent
    {
        public List<SectionDataList> SectionData { get; set; }
        public List<StudentDataList> StudentData { get; set; }
    }

    public class GetSubjectModel
    {
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? Markstype { get; set; }
    }

    public class GetClassbySectionSubject
    {
        public List<SectionDataList> SectionData { get; set; }
        public List<GetSubjectModel> SubjectData { get; set; }
    }

    public class GetClassbysubjectMarksmodel
    {
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? SubjectId { get; set; }
        public string? Subjectname { get; set; }
        public int? ExamId { get; set; }
        public string? ExamName { get; set; }
        public string? MarksType { get; set; }
        public double? MaxMarks { get; set; }

    }

    public class ExamMarksModel
    {
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? ExamId { get; set; }
        public string? MarksType { get; set; }
        public double? MaxMarks { get; set; }
    }



}
