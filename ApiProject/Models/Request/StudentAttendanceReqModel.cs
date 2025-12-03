namespace ApiProject.Models.Request
{
    public class StudentAttendanceReqModel
    {
            required
            public int StudentId { get; set; }

            required
            public int ClassId { get; set; }
            required 
            public string Status { get; set; }
            public string? Note { get; set; }
            
            required
            public System.DateTime Date { get; set; }
            //public string Present { get; set; }
            //public string Late { get; set; }
            //public string Absent { get; set; }
            //public string Holiday { get; set; }
            //public string HalfDay { get; set; }
        
    }

    public class GetStudentAttendanceReqModel
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public System.DateTime? Date { get; set; }
    }

    public class StudentMonthlyAttendanceReqModel
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int Month { get; set; }
    }

}
