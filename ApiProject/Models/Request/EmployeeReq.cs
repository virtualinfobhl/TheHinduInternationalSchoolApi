using ApiProject.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ApiProject.Models.Request
{
    public class GetEmployeReq
    {
        public int EmpId { get; set; }
    }

    public class EmpBankdetailReq
    {
        public string? AHName { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
    }

    public class AddEmployeeDetailReq
    {
        public Nullable<System.DateTime> JoiningDate { get; set; }
     //   public string? Emp_Type { get; set; }
        public string? Emp_Name { get; set; }
        public string? Father_husband_Name { get; set; }
        public string? DOB { get; set; }
        public string? Gendar { get; set; }
        public string? Marital_Status { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? Cast { get; set; }
        public string? Bloodgroup { get; set; }
        public string? Aadharcard { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public string? Phoneno { get; set; }
        public string? Mobileno { get; set; }
        public string? EmailId { get; set; }
        public string? P_Address { get; set; }
        public string? P_State { get; set; }
        public string? P_District { get; set; }
        public string? P_City { get; set; }
        public string? P_PinCode { get; set; }
        public string? P_Phoneno { get; set; }
        public string? P_Mobileno { get; set; }
        public string? P_EmailId { get; set; }
        public string? Qualification { get; set; }
        public string? Experience { get; set; }
        public string? Specialization { get; set; }
        public string? Applied_Post { get; set; }
        public string? Appointed_Post { get; set; }
        public Nullable<int> Basic_Salary { get; set; }
        public Nullable<int> Allowances { get; set; }
        public Nullable<int> TotalSalary { get; set; }
        public string? Specialnote { get; set; }

        public IFormFile? Employeeimg { get; set; }
        public IFormFile? Aadharimg { get; set; }
        public IFormFile? Panimg { get; set; }
        public IFormFile? Educationimg { get; set; }
        public IFormFile? Graduationimg { get; set; }
        public IFormFile? PostGraduationimg { get; set; }
        public IFormFile? Resumeimg { get; set; }
        public IFormFile? Experienceimg { get; set; }
        public IFormFile? BPassbookimg { get; set; }

        //public IFormFile? EmployeePhoto { get; set; }
        //public IFormFile? Experiencephoto { get; set; }
        //public IFormFile? Resumephoto { get; set; }
        //public IFormFile? AadharPhoto { get; set; }
        //public IFormFile? PancardPhoto { get; set; }
        //public IFormFile? EducationMarksheet { get; set; }
        //public IFormFile? GraduationMarksheet { get; set; }
        //public IFormFile? PostGraduationmarksheet { get; set; }
        //public IFormFile? BankPassbook { get; set; }

        public EmpBankdetailReq BankDetail { get; set; }

    }

    public class UpdateEmpBankdetailsReq
    {
        public int? BankId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public string? AHName { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
    }
    public class UpdatreEmployeeDetailReq
    {
        public int Emp_Id { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
     //   public string? Emp_Type { get; set; }
        public string? Emp_Name { get; set; }
        public string? Father_husband_Name { get; set; }
        public string? DOB { get; set; }
        public string? Gendar { get; set; }
        public string? Marital_Status { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? Cast { get; set; }
        public string? Bloodgroup { get; set; }
        public string? Aadharcard { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public string? Phoneno { get; set; }
        public string? Mobileno { get; set; }
        public string? EmailId { get; set; }
        public string? P_Address { get; set; }
        public string? P_State { get; set; }
        public string? P_District { get; set; }
        public string? P_City { get; set; }
        public string? P_PinCode { get; set; }
        public string? P_Phoneno { get; set; }
        public string? P_Mobileno { get; set; }
        public string? P_EmailId { get; set; }
        public string? Qualification { get; set; }
        public string? Experience { get; set; }
        public string? Specialization { get; set; }
        public string? Applied_Post { get; set; }
        public string? Appointed_Post { get; set; }
        public Nullable<int> Basic_Salary { get; set; }
        public Nullable<int> Allowances { get; set; }
        public Nullable<int> TotalSalary { get; set; }
        public string? Specialnote { get; set; }
        public IFormFile? Employeeimg { get; set; }
        public IFormFile? Aadharimg { get; set; }
        public IFormFile? Panimg { get; set; }
        public IFormFile? Educationimg { get; set; }
        public IFormFile? Graduationimg { get; set; }
        public IFormFile? PostGraduationimg { get; set; }
        public IFormFile? Resumeimg { get; set; }
        public IFormFile? Experienceimg { get; set; }
        public IFormFile? BPassbookimg { get; set; }
        public UpdateEmpBankdetailsReq BankDetail { get; set; }

    }

    public class GetWorkallcation
    {
        public Nullable<int> Emp_Id { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
    }

    public class SubjectWork
    {
        public Nullable<int> SubjectId { get; set; }
    }

    public class AddWorkallcation
    {
        public GetWorkallcation WorkAllo { get; set; }
        public List<SubjectWork> subjectWorks { get; set; }
    }

    //public class getEmployeeAttendance
    //{
    //   // public System.DateTime? Date { get; set; }
    //    public DateOnly? Date { get; set; }
    //}
    public class getEmployeeAttendance
    {
        private DateTime? _date;

        public DateTime? Date
        {
            get => _date?.Date;   // time हट जाएगा
            set => _date = value?.Date;
        }
    }

    public class addEmployeeAttendanceReq
    {
        required
        public Nullable<int> Emp_Id { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }

        required
        public System.DateTime Date { get; set; }
    }

    public class EmpAttendanceReportReq
    {
        public int? Months { get; set; } 
    }

    public class AddAdavanceSalaryReq
    {
        public Nullable<int> Emp_Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<double> AdvanceAmount { get; set; }
    }

    //public class UpdatamployeeloyeeDetailReq
    //{
    //    public int? Emp_Id { get; set; }
    //    public Nullable<System.DateTime> JoiningDate { get; set; }
    //    public string? Emp_Type { get; set; }
    //    public string? Emp_Name { get; set; }
    //    public string? Father_husband_Name { get; set; }
    //    public string? DOB { get; set; }
    //    public string? Gendar { get; set; }
    //    public string? Marital_Status { get; set; }
    //    public string? Nationality { get; set; }
    //    public string? Religion { get; set; }
    //    public string? Cast { get; set; }
    //    public string? Bloodgroup { get; set; }
    //    public string? Aadharcard { get; set; }
    //    public string? Address { get; set; }
    //    public string? State { get; set; }
    //    public string? District { get; set; }
    //    public string? City { get; set; }
    //    public string? PinCode { get; set; }
    //    public string? Phoneno { get; set; }
    //    public string? Mobileno { get; set; }
    //    public string? EmailId { get; set; }
    //    public string? P_Address { get; set; }
    //    public string? P_State { get; set; }
    //    public string? P_District { get; set; }
    //    public string? P_City { get; set; }
    //    public string? P_PinCode { get; set; }
    //    public string? P_Phoneno { get; set; }
    //    public string? P_Mobileno { get; set; }
    //    public string? P_EmailId { get; set; }
    //    public string? Qualification { get; set; }
    //    public string? Experience { get; set; }
    //    public string? Specialization { get; set; }
    //    public string? Applied_Post { get; set; }
    //    public string? Appointed_Post { get; set; }
    //    public Nullable<int> Basic_Salary { get; set; }
    //    public Nullable<int> Allowances { get; set; }
    //    public Nullable<int> TotalSalary { get; set; }
    //    public string? Specialnote { get; set; }

    //    public IFormFile? Employeeimg { get; set; }
    //    public IFormFile? Aadharimg { get; set; }
    //    public IFormFile? Panimg { get; set; }
    //    public IFormFile? Educationimg { get; set; }
    //    public IFormFile? Graduationimg { get; set; }
    //    public IFormFile? PostGraduationimg { get; set; }
    //    public IFormFile? Resumeimg { get; set; }
    //    public IFormFile? Experienceimg { get; set; }
    //    public IFormFile? BPassbookimg { get; set; }

    //    //public IFormFile? EmployeePhoto { get; set; }
    //    //public IFormFile? Experiencephoto { get; set; }
    //    //public IFormFile? Resumephoto { get; set; }
    //    //public IFormFile? AadharPhoto { get; set; }
    //    //public IFormFile? PancardPhoto { get; set; }
    //    //public IFormFile? EducationMarksheet { get; set; }
    //    //public IFormFile? GraduationMarksheet { get; set; }
    //    //public IFormFile? PostGraduationmarksheet { get; set; }
    //    //public IFormFile? BankPassbook { get; set; }

    //    public EmpBankdetailsReq BankDetail { get; set; }

    //}
}
