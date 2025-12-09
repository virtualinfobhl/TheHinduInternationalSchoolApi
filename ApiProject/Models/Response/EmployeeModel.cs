using ApiProject.Data;
using System.Drawing.Printing;
using System.Security.Cryptography.Pkcs;

namespace ApiProject.Models.Response
{


    public class GetEmployeeModel
    {
        public int Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public Nullable<bool> Active { get; set; }
    }

    public class GetEmployeeListModel
    {
        public int Emp_Id { get; set; }
        public string? Emp_Name { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string? Father_husband_Name { get; set; }
        public string? DOB { get; set; }
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
        public string? Status { get; set; }
        public string? Note { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public class EmpAttendanceReportModel
    {
        public Nullable<int> Emp_Id { get; set; }
        public string? Employeename { get; set; }
        //   public Nullable<System.DateTime> Date { get; set; }
        //    public string? Monthname { get; set; }
        public int? TotalP { get; set; }
        public int? TotalA { get; set; }
        public int? TotalH { get; set; }
        public int? TotalHF { get; set; }
        public int? TotalL { get; set; }
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
