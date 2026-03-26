using ApiProject.Data;
using System.Drawing.Printing;
using System.Security.Cryptography.Pkcs;

namespace ApiProject.Models.Response
{


    public class GetEmployeeModel
    {
        public int Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public string? EmpCode { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class GetBankDetailsList
    {
        public int? AID { get; set; }
        public int? Emp_Id { get; set; }
        public string? AccountHolder { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
        public Nullable<bool> ActiveStatus { get; set; }
        public System.DateTime RTS { get; set; }
    }
    public class GetEmployeeDetailsModel
    {
        public int Emp_Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? Emp_Type { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string? Emp_Name { get; set; }
        public string? Father_husband_Name { get; set; }
        public string? Father_husband_Occupation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? Gendar { get; set; }
        public string? Marital_Status { get; set; }
        public string? Bloodgroup { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? Cast { get; set; }
        public string? Address { get; set; }
        public string? EmailId { get; set; }
        public string? Phoneno { get; set; }
        public string? Mobileno { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? P_Address { get; set; }
        public string? P_EmailId { get; set; }
        public string? P_Phoneno { get; set; }
        public string? P_Mobileno { get; set; }
        public string? P_State { get; set; }
        public string? P_District { get; set; }
        public string? P_City { get; set; }
        public string? Qualification { get; set; }
        public string? Experience { get; set; }
        public string? Specialization { get; set; }
        public string? Applied_Post { get; set; }
        public string? Appointed_Post { get; set; }
        public Nullable<int> Basic_Salary { get; set; }
        public Nullable<int> Allowances { get; set; }
        public Nullable<int> TotalSalary { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Specialnote { get; set; }
        public string? EmployrPhoto { get; set; }
        public string? ResumePhoto { get; set; }
        public string? ExperiencePhoto { get; set; }
        public string? AadharcardPhoto { get; set; }
        public string? PancardPhoto { get; set; }
        public string? EducationPhoto { get; set; }
        public string? GraduationPhoto { get; set; }
        public string? Adharcard { get; set; }
        public string? BankPssbookPhoto { get; set; }
        public string? PostGraductionPhoto { get; set; }
        public Nullable<bool> Active { get; set; }
        public GetBankDetailsList BankDetails { get; set; }

        //public Nullable<double> AdvanceSalary { get; set; }
        //public Nullable<double> DueAdvanceSalary { get; set; }
        //public Nullable<System.DateTime> AdvanceDate { get; set; }
    }

    public class GetEmployeeListModel
    {
        public int Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string? Father_husband_Name { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string? Gendar { get; set; }
        public string? Address { get; set; }
        public string? EmailId { get; set; }
        public string? Mobileno { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        //   public string? PinCode { get; set; }
        public Nullable<int> Basic_Salary { get; set; }
        public Nullable<int> Allowances { get; set; }
        public Nullable<int> TotalSalary { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Active { get; set; }
        public string? EmployeePhoto { get; set; }
    }
    public class getSubjectlist
    {
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public List<SubjectDto> Sujbect { get; set; }
    }
    public class SubjectDto
    {
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        // public int? SubjectPriority { get; set; }
    }

    public class GetEmpWorkAllocationModel
    {
        public int? Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public List<getSubjectlist> SubjectData { get; set; }

    }


    public class getEmpWorkAllo
    {
        public Nullable<int> Emp_Id { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public string? Subject { get; set; }
    }

    public class GetEmpsubjectData
    {
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public int? SubjectPriority { get; set; }
    }

    public class GetEmpClassbySubjectModel
    {
        public List<getEmpWorkAllo> EmpWorkAllocation { get; set; }
        public List<SectionData> Sectiondata { get; set; }
        public List<GetEmpsubjectData> EmpsubjectData { get; set; }
    }

    public class EmpAttendanceDetail
    {
        public Nullable<int> Emp_Id { get; set; }
        public string? Employeename { get; set; }
        public string? EmpCode { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class EmpAttendanceReportModel
    {
        public Nullable<int> Emp_Id { get; set; }
        public string? Employeename { get; set; }
        //   public Nullable<System.DateTime> Date { get; set; }
            public string? Monthname { get; set; }
        public int? TotalP { get; set; }
        public int? TotalA { get; set; }
        public int? TotalH { get; set; }
        public int? TotalHF { get; set; }
        public int? TotalL { get; set; }
        public Dictionary<int, string> AttendanceByDate { get; set; } = new();
    }

    public class GetAdvsalaryModel
    {
        public Nullable<int> Emp_Id { get; set; }
        public string? Employeename { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<double> AdvanceAmount { get; set; }

    }

    public class EmployeeSalaryModel
    {
        public int? Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public decimal? Basic_Salary { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? TotalSalary { get; set; }
        public Nullable<double> AdvanceSalary { get; set; }
        public decimal? totalday { get; set; }
        public decimal? Salary { get; set; }
        public int? Month { get; set; }
        public Nullable<double> PayAdvReAmt { get; set; }
        public Nullable<double> PayAdvAmount { get; set; }
        public Nullable<double> PayAmount { get; set; }
        // public double? AdvanceSry { get; set; }\

    }

    public class AddEmployeeSalaryModel
    {
        public int? Emp_Id { get; set; }
        public int? Month { get; set; }
        public decimal? totalday { get; set; }
        public decimal? Basic_Salary { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? TotalSalary { get; set; }
        //  public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<double> PayAdvanceAmount { get; set; }
        public Nullable<double> PayAmount { get; set; }

    }

}
