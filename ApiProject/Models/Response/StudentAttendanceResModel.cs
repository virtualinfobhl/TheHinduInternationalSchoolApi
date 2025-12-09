namespace ApiProject.Models.Response
{

    public class StudentAttendanceListResModel
    {
        public int StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? RollNo { get; set; }
        public string? SRNo { get; set; }
        public string? stu_name { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
    //    public System.DateTime Date { get; set; }


    }

    public class StudentMonthlyAttendanceResModel
    {
        public int? StudentId { get; set; }
        public string? StuName { get; set; }
        public int? classid { get; set; }
        public int? sectionid { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? SRNo { get; set; }

        public Dictionary<int, string> AttendanceByDate { get; set; } = new();

        // Total Summary
        public int? TotalP { get; set; }
        public int? TotalA { get; set; }
        public int? TotalH { get; set; }
        public int? TotalHF { get; set; }
        public int? TotalL { get; set; }
    }

    public class StudentAttendanceResModel
    {

        public string stu_name { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SRNo { get; set; }

    }
    public class TodayAttendancePercentageRes
    {
        public int TotalStudents { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double PresentPercentage { get; set; }
        public double AbsentPercentage { get; set; }
    }

    public class StudentRollNoResponse
    {
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? StudentName { get; set; }
     //   public string? StudentPhoto { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Attendance { get; set; }
        public string? SRNo { get; set; }
        public string? RollNo { get; set; }
        public string? ClassName { get; set; }
        public string? Sectionname { get; set; }
    }
    public class SectionResponse
    {
        public string? SectionName { get; set; }
    }

    public class StudentDiscountFeeResponse
    {
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? StudentName { get; set; }
   //     public string? StudentPhoto { get; set; }
        public string? SRNo { get; set; }
        public string? RollNo { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public double? discount { get; set; }
        public double? total_fee { get; set; }
        public bool? RTE { get; set; }
    }

    public class StudentexamMarksResponse
    {
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? stu_name { get; set; }
        public string? SRNo { get; set; }
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public decimal? MaxTotal { get; set; }
        public double? Written { get; set; }
        public double? Oral { get; set; }
        public double? Pratical { get; set; }
        public double? Total { get; set; }
        public string? Grade { get; set; }
        public string? MGrade { get; set; }
    }

   
    public class GetTestvaluemodel
    {
        public int? SubjectId { get; set; }
        public decimal? TestType { get; set; }
    }

    public class StudentPersonalResponse
    {
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public string? stu_name { get; set; }
        public int? SectionId { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? Direction { get; set; }
        public string? Concentration { get; set; }
        public string? Discipline { get; set; }
        public string? Independently { get; set; }
        public string? Intiative { get; set; }
        public string? Cleanliness { get; set; }
        public string? Etiquette { get; set; }
        public string? OtherPro { get; set; }
        public string? Passionate { get; set; }
        public string? Confident { get; set; }
        public string? Responsible { get; set; }

    }

    public class StudentEventCertificate
    {
        public int? ECId { get; set; }
        public int? EventId { get; set; }
        public int? EmployeId { get; set; }
        public string? EventName { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? RankType { get; set; }
        public string? Description { get; set; }
        public DateTime? IssueDate { get; set; }
        public GetEmployeDataModel TeacherDetail { get; set; }
        public GetStudentDatamodel StudentDetails { get; set; }
    }

    public class GetEmployeDataModel
    {
        public string? Emp_Name { get; set; }
        public string? Father_husband_Name { get; set; }
    }

    public class GetStudentDatamodel
    {
        public string? stu_name { get; set; }
        public string? SRNo { get; set; }
        public string? RollNo { get; set; }
        public string? FatherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? MotherName { get; set; }
        public string? MotherMobileNo { get; set; }
    }

    public class StudentClassExamData
    {
        public List<SectionData> SectionDatas { get; set; }
        public List<StudentExamData> ExamDatas { get; set; }

    }

    public class SectionData
    {
        // public int? SchoolId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }
    //    public int? SectionPriority { get; set; }
    }
    public class StudentExamData
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? ExamId { get; set; }
        public int? SubjectId { get; set; }
        public int? SchoolId { get; set; }
        public string? ExamName { get; set; }
        public int? ExamPriority { get; set; }
        public string? SubjectName { get; set; }
        //     public int? SubjectPriority { get; set; }

    }

    public class ClassExamMarksModelreq
    {
        public int? ClassId { get; set; }
        public int? ExamId { get; set; }
        //public int? SectionId { get; set; }
        //public int? SubjectId { get; set; }
    }

    public class EventCartificateModelReq
    {
        public int? EventId { get; set; }
        public int? EmployeId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? Status { get; set; }
        public string? RankType { get; set; }
        public string? Description { get; set; }
        public DateTime? IssueDate { get; set; }

        //  public int ECId { get; set; }
        //   public bool? Active { get; set; }
    }


}
