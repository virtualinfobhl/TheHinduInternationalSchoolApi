using ApiProject.Models.Response;
using Microsoft.EntityFrameworkCore;
using ThisApiProject.Data;

namespace ApiProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<institute> institute { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<SessionInfo> SessionInfo { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<collegeinfo> collegeinfo { get; set; }
        public DbSet<ClassSectionTbl> ClassSectionTbl { get; set; }
        public DbSet<fees> fees { get; set; }

        public DbSet<student_admission> student_admission { get; set; }
        public DbSet<Student_Renew> Student_Renew { get; set; }
        public DbSet<InstallmentTbl> InstallmentTbl { get; set; }
        public DbSet<fee_installment> fee_installment { get; set; }
        public DbSet<M_FeeDetail> M_FeeDetail { get; set; }
        public DbSet<ExpenseTbl> ExpenseTbl { get; set; }
        public DbSet<SchlExpenseTbl> SchlExpenseTbl { get; set; }
        public DbSet<VStudentRenew> VStudentRenew { get; set; }
        public DbSet<StudentRenewView> StudentRenewView { get; set; }
        public DbSet<TestExamTbl> TestExamTbl { get; set; }
        public DbSet<ParentsTbl> ParentsTbl { get; set; }
        public DbSet<ClassSubjectExamTbl> ClassSubjectExamTbl { get; set; }
        public DbSet<ExamTbl> ExamTbl { get; set; }
        public DbSet<GradeInfo> GradeInfo { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Student_Attendance> Student_Attendance { get; set; }

        public DbSet<StuPersonalty> StuPersonalty { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventCertificate> EventCertificate { get; set; }

        public DbSet<TransDriverTbl> TransDriverTbl { get; set; }
        public DbSet<TransBusTbl> TransBusTbl { get; set; }
        public DbSet<TransRouteTbl> TransRouteTbl { get; set; }
        public DbSet<TransStoppageTbl> TransStoppageTbl { get; set; }
        public DbSet<TransportFeeTbl> TransportFeeTbl { get; set; }
        public DbSet<RouteAssignTbl> RouteAssignTbl { get; set; }
        public DbSet<StuRouteAssignTbl> StuRouteAssignTbl { get; set; }
        public DbSet<TransInstallmentTbl> TransInstallmentTbl { get; set; }
        public DbSet<NewTransportFeeTbl> NewTransportFeeTbl { get; set; }

        public DbSet<EmployeeRegister> EmployeeRegister { get; set; }
        public DbSet<EmployeeBankDetailTbl> EmployeeBankDetailTbl { get; set; }
        public DbSet<Emp_Workallocation> Emp_Workallocation { get; set; }
        public DbSet<Emp_Attendance> Emp_Attendance { get; set; }
        public DbSet<EmpAdvanceSalaryTbl> EmpAdvanceSalaryTbl { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalary { get; set; }

        public DbSet<BooksTbl> BooksTbl { get; set; }
        public DbSet<LibraryCardTbl> LibraryCardTbl { get; set; }
        public DbSet<BookIssueTbl> BookIssueTbl { get; set; }

    }
}
