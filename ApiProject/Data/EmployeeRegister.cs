using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class EmployeeRegister
    {
        [Key]
        public int Emp_Id { get; set; }
        public string? Emp_Code { get; set; }
        public string? Emp_Type { get; set; }
        public string? Emp_Name { get; set; }
        public string? Father_husband_Type { get; set; }
        public string? Father_husband_Name { get; set; }
        public string? Father_husband_Occupation { get; set; }
        public string? DOB { get; set; }
        public string? Gendar { get; set; }
        public string? Marital_Status { get; set; }
        public string? Cast { get; set; }
        public string? Bloodgroup { get; set; }
        public string? Religion { get; set; }
        public string? Nationality { get; set; }
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
        public string? ExperiencePhoto { get; set; }
        public string? ResumePhoto { get; set; }
        public string? EmployrPhoto { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Nullable<bool> CheckTerms { get; set; }
        public string? Specialnote { get; set; }
        public string? AadharcardPhoto { get; set; }
        public string? PancardPhoto { get; set; }
        public string? EducationPhoto { get; set; }
        public string? GraduationPhoto { get; set; }
        public Nullable<int> Userid { get; set; }
        public string? Adharcard { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string? BankPssbookPhoto { get; set; }
        public string? PostGraductionPhoto { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<double> AdvanceSalary { get; set; }
        public Nullable<double> DueAdvanceSalary { get; set; }
        public Nullable<System.DateTime> AdvanceDate { get; set; }
    }
}
