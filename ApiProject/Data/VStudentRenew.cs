using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class VStudentRenew
    {

        [Key]
        public int SRId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? RollNo { get; set; }
        public string? Attendance { get; set; }
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
        public string? SRNo { get; set; }
        public string? StuCode { get; set; }
        public string? stu_name { get; set; }
        public string? father_name { get; set; }
        public string? father_occupation { get; set; }
        public Nullable<double> Fatherlncome { get; set; }
        public string? mother_name { get; set; }
        public Nullable<double> MotherIncome { get; set; }
        public string? mother_occupation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? gender { get; set; }
        public string? cast_category { get; set; }
        public string? blood_group { get; set; }
        public string? address { get; set; }
        public string? email { get; set; }
        public string? phoneno { get; set; }
        public string? mobileno { get; set; }
        public string? p_address { get; set; }
        public string? p_email { get; set; }
        public string? p_phoneno { get; set; }
        public string? p_mobileno { get; set; }
        public string? p_city { get; set; }
        public string? stu_photo { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<System.DateTime> admission_date { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? father_mobile { get; set; }
        public string? pincode { get; set; }
        public string? p_pincode { get; set; }
        public string? AdharCard { get; set; }
        public string? CanceledReason { get; set; }
        public string? Religion { get; set; }
        public string? Caste { get; set; }
        public string? stu_mobile { get; set; }
        public string? mother_mobile { get; set; }
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
        public string? LastStudentTC { get; set; }
        public Nullable<bool> StuDetail { get; set; }
        public Nullable<bool> StuFee { get; set; }
        public int StudentId { get; set; }
        public Nullable<bool> RTE { get; set; }
        public string? city { get; set; }
        public string? Status { get; set; }
        public string? state { get; set; }
        public string? district { get; set; }
        public string? p_state { get; set; }
        public string? p_district { get; set; }
        public Nullable<int> ParentsId { get; set; }
    }
}
