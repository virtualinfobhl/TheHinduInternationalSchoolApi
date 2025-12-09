using ApiProject.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ApiProject.Models.Request
{
    public class quickadmissionmodel
    {

        [Required(ErrorMessage = "SR No is required.")]
        public string SRNo { get; set; }
        public string? stu_code { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        public string stu_name { get; set; }

        [Required(ErrorMessage = "Father name is required.")]
        public string father_name { get; set; }

        [Required(ErrorMessage = "Father mobile number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Father mobile number must be 10 digits.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Father mobile number must contain only digits.")]
        public string father_mobile { get; set; }

        [Required(ErrorMessage = "Mother name is required.")]
        public string mother_name { get; set; }
        public string GuardianName { get; set; }
        public string GuardianMobileNo { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public System.DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string? gender { get; set; }

        [Required(ErrorMessage = "Admission date is required.")]
        public System.DateTime admission_date { get; set; }

        public string? RTE { get; set; }

        [Required(ErrorMessage = "Class is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Class.")]
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public double? AFeeDiscount { get; set; }
        public double? AdmissionPayfee { get; set; }
        public string? PaymentMode { get; set; }

    }

    public class getstudentlistReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? studentId { get; set; }
        public string? srno { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class getstudentDellistReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? studentId { get; set; }
        public string? srno { get; set; }

    }

    public class GetStudentReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? StudentId { get; set; }
        public string? srno { get; set; }
        public System.DateTime? Fromdate { get; set; }
        public System.DateTime? Todate { get; set; }

    }
    public class GetStudentIDCardReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }

    }

    public class GetStudentTCDropoutReq
    {
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }

    }

    public class AddStudentReqModel
    {
        public Nullable<System.DateTime> admission_date { get; set; }
        public string? SRNo { get; set; }
        public string? stu_name { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? RTE { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? gender { get; set; }
        public string? cast_category { get; set; }
        public string? Religion { get; set; }
        public string? Caste { get; set; }
        public string? email { get; set; }
        public string? blood_group { get; set; }
        public IFormFile? stuphoto { get; set; }
        public IFormFile? stuaadhar { get; set; }
        public IFormFile? stubirth { get; set; }


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

        public IFormFile? fatheraadhar { get; set; }
        public IFormFile? motheraadhar { get; set; }
        public IFormFile? IncomeCertificate { get; set; }
        public IFormFile? JanAadhar { get; set; }

        public string? address { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
        public string? city { get; set; }
        public string? pincode { get; set; }

        public string? p_address { get; set; }
        public string? p_state { get; set; }
        public string? p_district { get; set; }
        public string? p_city { get; set; }
        public string? p_pincode { get; set; }

        public string? LastSchlName { get; set; }
        public string? LastClass { get; set; }
        public Nullable<double> LastExanTotalMarks { get; set; }
        public string? LastDivision { get; set; }
        public string? LastParecentage { get; set; }
        public string? LastRemarks { get; set; }
        public IFormFile? LastMarkSheetPhotos { get; set; }
        public IFormFile? studentTcfile { get; set; }

        public List<FeeInstallmentReqDto> feeInstallments { get; set; }
        // Admission Fee Receipts
        public AdmissionFeeReceiptReqDto admissionReceipt { get; set; }
    }


    public class StudentUpdateReqModel
    {
        public int studentId { get; set; }
        public DateTime? admission_date { get; set; }
        public string? srNo { get; set; }
        public string? stu_name { get; set; }
        public int? classId { get; set; }
        public int? sectionId { get; set; }
        public string? rte { get; set; }
        public DateTime? dob { get; set; }
        public string? gender { get; set; }
        public string? cast_category { get; set; }
        public string? religion { get; set; }
        public string? caste { get; set; }
        public string? email { get; set; }
        public string? blood_group { get; set; }

        public int? parentid { get; set; }
        public string? fathername { get; set; }
        public string? fathermobileno { get; set; }
        public string? fatherOP { get; set; }
        public double? fatherIncome { get; set; }
        public string? mothername { get; set; }
        public string? mothermobileno { get; set; }
        public string? motherOP { get; set; }
        public double? motherIncome { get; set; }
        public string? guardianName { get; set; }
        public string? guardianMobileNo { get; set; }

        public string? address { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
        public string? city { get; set; }
        public string? pincode { get; set; }
        public string? p_address { get; set; }
        public string? p_state { get; set; }
        public string? p_district { get; set; }
        public string? p_city { get; set; }
        public string? p_pincode { get; set; }

        public string? lastSchlName { get; set; }
        public string? lastClass { get; set; }
        public double? lastExanTotalMarks { get; set; }
        public string? lastDivision { get; set; }
        public string? lastParecentage { get; set; }
        public bool? studentTc { get; set; }
        public string? lastRemarks { get; set; }


        public IFormFile? stuphoto { get; set; }
        public IFormFile? stuaadhar { get; set; }
        public IFormFile? stubirth { get; set; }
        public IFormFile? fatheraadhar { get; set; }
        public IFormFile? motheraadhar { get; set; }
        public IFormFile? IncomeCertificate { get; set; }
        public IFormFile? JanAadhar { get; set; }
        public IFormFile? lastMarkSheetPhoto { get; set; }
        public IFormFile? LastMarkSheetPhotos { get; set; }
        public IFormFile? studentTcfile { get; set; }

        public AdmissionFeeReceiptReqDto admissionReceipt { get; set; }
        public List<FeeInstallmentReqMOdel> feeInstallmentlist { get; set; }

    }

    public class FeeInstallmentReqMOdel
    {
        //public int? StudentId { get; set; }
        //public int? ClassId { get; set; }
        //public int? SectionId { get; set; }
        public int? IntallmentID { get; set; }
        public Nullable<double> total_fee { get; set; }
        public string? Installment { get; set; }
        public Nullable<double> FAmount { get; set; }
    }

    public class FeeInstallmentReqDto
    {
        //public double? SInsAmount { get; set; }
        public Nullable<int> IntallmentID { get; set; }
        public Nullable<double> total_fee { get; set; }
        //   public Nullable<double> due_fee { get; set; }
        public string? Installment { get; set; }
        public Nullable<double> FAmount { get; set; }
    }

    public class AdmissionFeeReceiptReqDto
    {
        public double? AdmissionPayFees { get; set; }
        public double? AdmissionFeeDiscount { get; set; }
        public double? pramoteFees { get; set; }
        public string? PaymentMode { get; set; }
        public double? FeeDiscount { get; set; }
        public DateTime? PaymentDate { get; set; }
        //    public double? oldDuefees { get; set; }
    }

    public class StudentExcelUploadListReq
    {
        public string SRNo { get; set; }
        public string stu_name { get; set; }
        public string father_name { get; set; }
        public string mother_name { get; set; }
        public System.DateTime DOB { get; set; }
        public System.DateTime admission_date { get; set; }
        public string father_mobile { get; set; }
        public string Address { get; set; }
        public string? PaymentMode { get; set; }
        public string? RTE { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public double? Discount { get; set; }
        public double? OldDuefees { get; set; }
        public double? AdmissionPayfee { get; set; }
        public System.DateTime? PaymentDate { get; set; }

    }

    //public class AddAdmissionFeeReceiptReqDto
    //{
    //    public double? PayFees { get; set; }
    //    //public DateTime? PaymentDate { get; set; }
    //    public string? PaymentMode { get; set; }
    //    public double? Cash { get; set; }
    //    public double? Upi { get; set; }
    //    public double? pramoteFees { get; set; }
    //    public double? aFeeDiscount { get; set; }
    //    public double? discount { get; set; }
    //    public double? oldDuefees { get; set; }
    //}

    public class BulkStudentReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
    }

    public class studentRollNoAttendaceReq
    {
        public int? StudentId { get; set; }
        public int? SectionId { get; set; }
        public string? RollNo { get; set; }
        public string? Attendance { get; set; }
        public string? Grade { get; set; }
    }

    public class studentDiscountfeeReq
    {
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }
        public double? discount { get; set; }
    }


    public class studentexamMarksReq
    {
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int? SubjectId { get; set; }
        public string? MatksType { get; set; }
        public string? TestType { get; set; }
    }


    public class UpdateStudentMarksRequest
    {
        public int? SubjectId { get; set; }
        public string? MatksType { get; set; }
        public string? TestType { get; set; }
        public List<SubjectDataModelReq> ExamModal { get; set; }
    }

    public class SubjectDataModelReq
    {
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }
        public decimal? Written { get; set; }
        public decimal? Oral { get; set; }
        public decimal? Pratical { get; set; }
        public decimal? Total { get; set; }
        public string? Grade { get; set; }
        public string? MGrade { get; set; }
    }

    public class examMarksmodel
    {
        public int? ClassId { get; set; }
        public string? MarksType { get; set; }
        public string? TestType { get; set; }
        public int? SubjectId { get; set; }
        public IFormFile? Marksdata { get; set; }
    }

    public class stuPersonalModelReq
    {
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public string? Discipline { get; set; }
        public string? Concentration { get; set; }
        public string? Intiative { get; set; }
        public string? Independently { get; set; }
        public string? Direction { get; set; }
        public string? Cleanliness { get; set; }
        public string? Etiquette { get; set; }
        public string? OtherPro { get; set; }
        public string? Passionate { get; set; }
        public string? Confident { get; set; }
        public string? Responsible { get; set; }
    }

    public static class StudentValidator
    {
        public static List<string> Validate(StudentExcelUploadListReq student)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(student.SRNo))
                errors.Add("Sr No. is required.");
            if (string.IsNullOrWhiteSpace(student.ClassName))
                errors.Add("Class Name is required.");
            if (string.IsNullOrWhiteSpace(student.stu_name))
                errors.Add("Name is required.");
            if (string.IsNullOrWhiteSpace(student.father_name))
                errors.Add("Father Name is required.");
            if (string.IsNullOrWhiteSpace(student.mother_name))
                errors.Add("Mother Name is required.");
            if (string.IsNullOrWhiteSpace(student.father_mobile))
                errors.Add("Father Mobile No. Name is required.");
            if (student.DOB == null || student.DOB < new DateTime(1990, 1, 1))
                errors.Add("DOB is invalid.");

            return errors;
        }
    }


    public class ExcelErrorRow
    {
        public int RowNumber { get; set; }
        public List<string> Errors { get; set; }
    }

}
