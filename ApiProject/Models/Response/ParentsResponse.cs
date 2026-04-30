using Newtonsoft.Json;

namespace ApiProject.Models.Response
{
    public class ParentsResponse
    {
        public string Token { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolName { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string ParentMobile { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string SrNo { get; set; }
        public string StartSession { get; set; }
        public string EndSession { get; set; }
        public string ResponseCode { get; set; }
    }

    public class GetParentDetailsModel
    {
        public int StudentId { get; set; }
        public string stuphoto { get; set; }
        public string? Studentname { get; set; }
        public string? SRNo { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? SectionId { get; set; }
        public string? SectionName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? gender { get; set; }
        public string? cast_category { get; set; }
        public string? Religion { get; set; }
        public string? Caste { get; set; }
        public string? FatherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? FatherOccupation { get; set; }
        public Nullable<double> FatherIncome { get; set; }
        public string? MotherName { get; set; }
        public string? MotherMobileNo { get; set; }
        public string? MotherOccupation { get; set; }
        public Nullable<double> MotherIncome { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianMobileNo { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
        public string? city { get; set; }
        public string? pincode { get; set; }
        public string? address { get; set; }
        public string? RollNumber { get; set; }

    }

    public class GetStudentModel
    {
        public int StudentId { get; set; }
        public string stuphoto { get; set; }
        public string? Studentname { get; set; }
    }

    public class GetPasswordModel
    {
        public int? StudentId { get; set; }
        public int? ParentsId { get; set; }
        public string? Studentname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class GetChangepasswordReq
    {
        public int? ParentsId { get; set; }
        public string? password { get; set; }
    }
    public class GetStudentFeeModel
    {
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> PaidAmount { get; set; }
        public Nullable<double> DueAmount { get; set; }
    }

    public class StudentFeePaymentResult
    {
        public int FDId { get; set; }
        public string ReceiptNo { get; set; }
        public string OrderId { get; set; }
        public string merchantOrderId { get; set; }
        public string State { get; set; }
        public long? ExpireAt { get; set; }
        public string Token { get; set; }
    }

    public class StudentTransportPaymentResult
    {
        public int NewPaymentId { get; set; }
        public string ReceiptNo { get; set; }
        public string OrderId { get; set; }
        public string merchantOrderId { get; set; }
        public string State { get; set; }
        public long? ExpireAt { get; set; }
        public string Token { get; set; }
    }

    public class GetStuFeeInstallmentModel
    {
        public List<Nullable<double>> Installment { get; set; }
    }
    public class GetStuDueInstallmentModel
    {
        //   public Nullable<double> PaidAmount { get; set; }
        public List<Nullable<double>> DueInstallment { get; set; }
    }

    public class GetInstallmentModel
    {
        public string? Installmentno { get; set; }
        public Nullable<double> Dueinstallment { get; set; }
    }

    public class GetFeereceiptModel
    {
        public int FDId { get; set; }
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? ReceiptNo { get; set; }
        public Nullable<double> PayFee { get; set; }
        public string? FeeType { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string? PaymentMode { get; set; }
        public string? Remark { get; set; }

    }
    public class getStudentInstallmentModel
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Srno { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? sectionId { get; set; }
        public string? sectionName { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> PaidFee { get; set; }
        public Nullable<double> Duefee { get; set; }
        public List<GetInstallmentModel> DueInstallment { get; set; }
        public List<GetFeereceiptModel> FeeReceipt { get; set; }

    }

    public class GetTransportInstallFeeModel
    {
        public int? StuRouteAssignId { get; set; }
        public int? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Srno { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? sectionId { get; set; }
        public string? sectionName { get; set; }
        public int? VehicleId { get; set; }
        public string? VehicleNo { get; set; }
        public int? Routeid { get; set; }
        public string? RouteName { get; set; }
        public int? StoppageId { get; set; }
        public string? StoppageName { get; set; }
        public Nullable<double> TransportFee { get; set; }
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> PaidFee { get; set; }
        public Nullable<double> TDiscount { get; set; }
        public Nullable<double> DueFee { get; set; }
        public Nullable<double> OldDueFee { get; set; }
        public Nullable<double> OldPaidFee { get; set; }
        public List<TInstallmentList> TransInatallment { get; set; }
        public List<TransReceiptList> TransReceiptList { get; set; }
    }

    public class GetStuInstallmentModel
    {
        public Nullable<int> university_id { get; set; }
        public Nullable<int> IntallmentID { get; set; }
        public Nullable<double> total_fee { get; set; }
        public string? Installment { get; set; }
        public Nullable<double> FAmount { get; set; }
    }

    // student  attendance 

    public class GetAttendanceModel
    {
        public string? monthname { get; set; }
        public Dictionary<int, string> AttendanceByDate { get; set; } = new();
        public int? TotalP { get; set; }
        public int? TotalA { get; set; }
        public int? TotalH { get; set; }
        public int? TotalHF { get; set; }
        public int? TotalL { get; set; }
    }

    public class GetTestTypeModel
    {
        public int? studentid { get; set; }
        public List<typelist> testtype { get; set; }

    }
    public class typelist
    {
        public string? testexamtype { get; set; }
    }

    public class GetTestwiseExamModel
    {
        public List<GetExamSubjectModel> SubjectName { get; set; }
        public List<GetTestMarksModel> TestMarks { get; set; }
        public Nullable<double> TotalMatks { get; set; }
        public Nullable<double> MaxTotal { get; set; }

    }

}
