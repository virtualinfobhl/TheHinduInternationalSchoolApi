using ApiProject.Data;

namespace ApiProject.Models.Response
{

    public class GetStudentFeeListModel
    {
        public int StudentId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? stu_name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? SRNo { get; set; }
        public string? RollNo { get; set; }
        public Nullable<System.DateTime> admission_date { get; set; }
        public Nullable<bool> RTE { get; set; }
        public Nullable<int> ParentsId { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public Nullable<double> total_fee { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        //public SectionResponse Section { get; set; }
        //public List<Parent>? Parentsdetail { get; set; }
        public List<StudentReceiptModel> Studentreceipt { get; set; }

    }

    public class GetStudentFeeDetailsModel
    {
        public int stu_id { get; set; }
        public string? stu_name { get; set; }
        public string? stu_code { get; set; }
        public string? Srno { get; set; }
        public Nullable<bool> RTE { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public Nullable<double> PayAdmissionFee { get; set; }
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> FeeDiscount { get; set; }
        public Nullable<double> DueOldFee { get; set; }
        public Nullable<double> TotalNetFee { get; set; }
        public Nullable<double> PaidFee { get; set; }
        public Nullable<double> DueFee { get; set; }

        public List<FeeReceiptModel> FeeReceipt { get; set; }
    }

    public class ClasswiseInstallModel
    {
        public int stu_id { get; set; }
        public string? stu_name { get; set; }
        public string? stu_code { get; set; }
        public string? Srno { get; set; }
        public Nullable<bool> RTE { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public Nullable<double> TotalNetFee { get; set; }
        public Nullable<double> PaidFee { get; set; }
        public Nullable<double> DueFee { get; set; }

        public List<ClasswiseInstallmentModel> Installments { get; set; }
    }

    public class ClasswiseDueeFeeModel
    {
        public int stu_id { get; set; }
        public string? stu_name { get; set; }
        public string? Srno { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public Nullable<double> DueFee { get; set; }
    }

    public class AllClasswiseDueeFeeModel
    {
        public int stu_id { get; set; }
        public string? stu_name { get; set; }
        public string? Srno { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public Nullable<double> DueFee { get; set; }
        public Nullable<double> TransportDueFee { get; set; }

    }

    public class FeeReceiptModel
    {
        public int? Receiptid { get; set; }
        public string? ReceiptNo { get; set; }
        public string? FeeType { get; set; }
        public Nullable<double> PayFees { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public string? Remark { get; set; }
    }

    public class GetExamSubjectModel
    {
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
    }
    public class GetTestMarksModel
    {
        public int? SubjectId { get; set; }
        public long? MaxMarks { get; set; }
        public long? TestMarks { get; set; }
        public string? MGrade { get; set; }
        public string? TestType { get; set; }
        public string? MarksType { get; set; }
        public string? Subjects { get; set; }

    }

    //public class GetGeademodel
    //{
    //    public string? GradeName { get; set; }
    //}


    public class TestExamMarksmOdel
    {
        public int stu_id { get; set; }
        public string? stu_name { get; set; }
        public string? RollNo { get; set; }
        //     public string? Srno { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public List<GetExamSubjectModel> SubjectName { get; set; }
        public List<GetTestMarksModel> TestMarks { get; set; }
        public Nullable<double> TotalMatks { get; set; }
        public Nullable<double> MaxTotal { get; set; }
        public string? Grade { get; set; }

    }

    public class StudentMarksheetModel
    {
        public int stu_id { get; set; }
        public string? stu_name { get; set; }
        public string? Srno { get; set; }
        public string? RollNo { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public Nullable<double> TotalMatks { get; set; }
        public Nullable<double> MaxTotal { get; set; }
    }



    public class Parent
    {
        public int? ParentsId { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianMobileNo { get; set; }
        public string? FatherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? MotherName { get; set; }
    }

    public class StudentReceiptModel
    {
        public string? ReceiptNo { get; set; }
        public string? Status { get; set; }
        public string? FeeType { get; set; }
        public Nullable<double> PayFees { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public string? Remark { get; set; }
        public Nullable<double> Cash { get; set; }
        public Nullable<double> Upi { get; set; }
    }

    public class GetStudentQuickListModel
    {
        public int stu_id { get; set; }
        public string? stu_name { get; set; }
        public string? stu_code { get; set; }
        public string? Srno { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> admission_date { get; set; }
        public Nullable<bool> RTE { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }

    }

    public class GetStudentDetailsLisModel
    {
        public int stu_id { get; set; }
        public string? stu_photo { get; set; }
        public string? stu_name { get; set; }
        public string? stu_code { get; set; }
        public string? Srno { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> admission_date { get; set; }
        public Nullable<bool> RTE { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? Address { get; set; }
    }

    public class ClasswiseStudentListModel
    {
        public int? StudentId { get; set; }
        public string? stu_name { get; set; }
        public string? SRNo { get; set; }
        public string? RollNo { get; set; }
        public Nullable<bool> RTE { get; set; }
        public string? FatherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? MotherName { get; set; }
        public Nullable<double> total_fee { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public double? PaidFee { get; set; }
        public List<ClasswiseInstallmentModel> ClassInstallments { get; set; }
    }

    public class ClasswiseInstallmentModel
    {
        public string? Installment { get; set; }
        public Nullable<double> SInsAmount { get; set; }
    }

    public class DailyCollectionModel
    {
        public string? SRNo { get; set; }
        public string? stu_name { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? ReceiptNo { get; set; }
        public string? FeeType { get; set; }
        public Nullable<double> PayFees { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public string? Remark { get; set; }
        public Nullable<double> Cash { get; set; }
        public Nullable<double> Upi { get; set; }
    }


}




