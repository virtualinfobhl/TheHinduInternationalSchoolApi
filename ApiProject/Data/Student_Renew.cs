using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public partial class Student_Renew
    {
        [Key]
        public int SRId { get; set; }
        public Nullable<int> StuId { get; set; }
        public int? ClassId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string? RollNo { get; set; }
        public string? TotalGrade { get; set; }
        public string? Attendance { get; set; }
        public string? MaxAttendance { get; set; }
        public Nullable<bool> StuDetail { get; set; }
        public Nullable<bool> StuFees { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<bool> completed { get; set; }
        public Nullable<bool> RTE { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Tution_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<double> total_fee { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> stu_fee { get; set; }
        public Nullable<double> due_fee { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public string? payment_mode { get; set; }
        public string? Status { get; set; }
        public Nullable<bool> Dropout { get; set; }
        public Nullable<int> ParentsId { get; set; }


    }
}
