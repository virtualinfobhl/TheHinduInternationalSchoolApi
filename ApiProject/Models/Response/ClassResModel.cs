using ApiProject.Data;
using ApiProject.Models.Request;

namespace ApiProject.Models.Response
{
    public class ClassResModel
    {
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        //public int ClassPriority { get; set; }
        public Nullable<bool> CActive { get; set; }

    }

    public class ClassALLResModel
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? ClassPriority { get; set; }
        public Nullable<bool> CActive { get; set; }
        public List<ClassSectionResModel> classSectionRes { get; set; }

    }


    public class ClassFeeResModel
    {
     //   public int fee_id { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> tution_fee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<bool> active { get; set; }
    }
    public class ClassSectionResModel
    {
        public int? collegeid { get; set; }
        public string? collegename { get; set; }
        public Nullable<int> university_id { get; set; }
        public string? ClassName { get; set; }
        public Nullable<bool> active { get; set; }

    }

    public class FeendSectionByClasssModel
    {
        public List<SectionDataList> SectionList { get; set; }
        public List<ClassFeeResModel> FeeList { get; set; }
        public List<InstallmentDetail> installments { get; set; }
    }

    public class GetParentsDetailModel
    {
        public int? ParentsId { get; set; }
        public string? father_name { get; set; }
        public string? father_mobile { get; set; }
        public string? father_occupation { get; set; }
        public Nullable<double> Fatherlncome { get; set; }
        public string? mother_name { get; set; }
        public string? mother_mobile { get; set; }
        public Nullable<double> MotherIncome { get; set; }
        public string? mother_occupation { get; set; }
    }


    public class quickadmissionres
    {
        public int StudentId { get; set; }
        public int? ReceiptId { get; set; }
    }

    public class studentreesponse
    {
        public int StudentId { get; set; }
        public string? studentName { get; set; }
        public int? sectionId { get; set; }
    }

    public class GetQuickStudentReqModel
    {
            public int SRId { get; set; }
            public int StudentId { get; set; }
            public Nullable<int> ClassId { get; set; }
            public Nullable<int> SectionId { get; set; }
            public string stu_name { get; set; }
            public string gender { get; set; }
            public Nullable<System.DateTime> DOB { get; set; }
            public string SRNo { get; set; }
            public string RollNo { get; set; }
            public string StuCode { get; set; }
            public string father_name { get; set; }
            public string mother_name { get; set; }
            public string father_mobile { get; set; }
            public Nullable<System.DateTime> admission_date { get; set; }
            public Nullable<bool> RTE { get; set; }
            public string address { get; set; }
            public string state { get; set; }
            public string district { get; set; }
            public string city { get; set; }
            public string pincode { get; set; }
            public string stu_photo { get; set; }
            public string email { get; set; }
            public Nullable<bool> Active { get; set; }
            public Nullable<int> ParentsId { get; set; }
            public List<Parents>? Parentsdetails { get; set; }

    }

    public class Parents
    {
        public int? ParentsId { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianMobileNo { get; set; }
        public string? FatherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? MotherName { get; set; }
    }

    public class stduentlistres
    {
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public int SRId { get; set; }
        public string RollNo { get; set; }
        public string Attendance { get; set; }
        public Nullable<System.DateTime> RenewDate { get; set; }
        public Nullable<bool> completed { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> SchoolId { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Tution_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public Nullable<double> total_fee { get; set; }
        public Nullable<bool> StudentTc { get; set; }
        public string SRNo { get; set; }
        public string StuCode { get; set; }
        public string stu_name { get; set; }
        public string father_name { get; set; }
        public string father_occupation { get; set; }
        public Nullable<double> Fatherlncome { get; set; }
        public string mother_name { get; set; }
        public Nullable<double> MotherIncome { get; set; }
        public string mother_occupation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string gender { get; set; }
        public string cast_category { get; set; }
        public string blood_group { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phoneno { get; set; }
        public string mobileno { get; set; }
        public string p_address { get; set; }
        public string p_email { get; set; }
        public string p_phoneno { get; set; }
        public string p_mobileno { get; set; }
        public string p_city { get; set; }
        public string stu_photo { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<System.DateTime> admission_date { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string father_mobile { get; set; }
        public string pincode { get; set; }
        public string p_pincode { get; set; }
        public string AdharCard { get; set; }
        public string CanceledReason { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string stu_mobile { get; set; }
        public string mother_mobile { get; set; }
        public string stu_aadhar { get; set; }
        public string stu_birth { get; set; }
        public string father_aadhar { get; set; }
        public string mother_aadhar { get; set; }
        public string Bonafide_Certificat { get; set; }
        public string Income_Certificate { get; set; }
        public string Jan_Aadhar { get; set; }
        public string LastSchlName { get; set; }
        public string LastClass { get; set; }
        public Nullable<double> LastExanTotalMarks { get; set; }
        public string LastDivision { get; set; }
        public string LastParecentage { get; set; }
        public string LastRemarks { get; set; }
        public string LastMarkSheetPhoto { get; set; }
        public Nullable<bool> StuDetail { get; set; }
        public Nullable<bool> StuFee { get; set; }
        public int StudentId { get; set; }
        public Nullable<bool> RTE { get; set; }
        public string city { get; set; }
        public string Status { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string p_state { get; set; }
        public string p_district { get; set; }
        public Nullable<int> ParentsId { get; set; }

    }

    public class getStudentListModel
    {
        // Student Details
        public int StudentId { get; set; }
        public string? SRNo { get; set; }
        public string? StuCode { get; set; }
        public string? stu_name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? gender { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? RollNo { get; set; }
        public int? parentid { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianMobileNo { get; set; }
        public string? FatherName { get; set; }
        public string? FatherMobileNo { get; set; }
        public string? FatherOccupation { get; set; }
        public Nullable<double> FatherIncome { get; set; }
        public string? MotherName { get; set; }
        public string? MotherMobileNo { get; set; }
        public string? MotherOccupation { get; set; }
        public Nullable<double> MotherIncome { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Tution_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public Nullable<double> total_fee { get; set; }
        public Nullable<bool> StudentTc { get; set; }
        public string? cast_category { get; set; }
        public string? blood_group { get; set; }
        public string? address { get; set; }
        public string? email { get; set; }
        public string? phoneno { get; set; }
        public string? mobileno { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
        public string? city { get; set; }
        public string? p_address { get; set; }
        public string? p_email { get; set; }
        public string? p_phoneno { get; set; }
        public string? p_mobileno { get; set; }
        public string? p_state { get; set; }
        public string? p_district { get; set; }
        public string? p_city { get; set; }
        public string? stu_photo { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<System.DateTime> admission_date { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? pincode { get; set; }
        public string? p_pincode { get; set; }
        public string? AdharCard { get; set; }
        public string? CanceledReason { get; set; }
        public string? Religion { get; set; }
        public string? Caste { get; set; }
        public string? stu_mobile { get; set; }
        public string? stu_aadhar { get; set; }
        public string? stu_birth { get; set; }
        public string? father_aadhar { get; set; }
        public string? mother_aadhar { get; set; }
        public string? Bonafide_Certificat { get; set; }
        public string? Income_Certificate { get; set; }
        public string? Jan_Aadhar { get; set; }
        public string? LastSchlName { get; set; }
        public string? LastClass { get; set; }
        public Nullable<double> LastExanTotalMarks { get; set; }
        public string? LastDivision { get; set; }
        public string? LastParecentage { get; set; }
        public string? LastRemarks { get; set; }
        public string? LastMarkSheetPhoto { get; set; }
        public string? LastSchoolTC { get; set; }
        public Nullable<bool> StuDetail { get; set; }
        public Nullable<bool> StuFee { get; set; }
        public Nullable<bool> RTE { get; set; }

        // Fee Installments
        public List<FeeInstallmentDto> FeeInstallments { get; set; }

        // Admission Fee Receipts
        public AdmissionFeeReceiptDto AdmissionReceipts { get; set; }

    }

    public class StudentDetailsById
    {
        public string? Srno { get; set; }
        public string? RollNo { get; set; }
        public string? StudentName { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public string? MobileNo { get; set; }

    }


    public class StudentFeeTCModel
    {
        public string? Srno { get; set; }
        public Nullable<bool> RTE { get; set; }
        public Nullable<double> TotalFee { get; set; }
        public Nullable<double> ToPaidFee { get; set; }
        public Nullable<double> ToDueFee { get; set; }
        public Nullable<double> TransportDueFee { get; set; }

    }


    public class FeeInstallmentDto
    {
        public Nullable<double> SInsAmount { get; set; }
    }


    public class AdmissionFeeReceiptDto
    {
        public Nullable<double> PayFees { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public Nullable<double> Cash { get; set; }
        public Nullable<double> Upi { get; set; }

    }


    public class GradeResModel
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string Percent_Upto { get; set; }
        public string Percent_From { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> SchoolId { get; set; }

    }


    public class SubjectResModel
    {
        public int? subject_id { get; set; }
        public int? university_id { get; set; }
        public string? subject_name { get; set; }
        public Nullable<int> Priority { get; set; }
        public string? Marks_Type { get; set; }
        public int? Quarterly { get; set; }
        public int? first_test { get; set; }
        public int? second_test { get; set; }
        public int? third_test { get; set; }
        public int? fourth_test { get; set; }
        public int? half_yearly { get; set; }
        public int? yearly { get; set; }
        public Nullable<bool> active { get; set; }

    }

}
