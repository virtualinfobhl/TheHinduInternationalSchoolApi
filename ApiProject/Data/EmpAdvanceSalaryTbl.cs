using System.ComponentModel.DataAnnotations;

namespace ThisApiProject.Data
{
    public class EmpAdvanceSalaryTbl
    {
        [Key]
        public int AdvId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public Nullable<double> AdvanceAmt { get; set; }
        public Nullable<double> PayAdvanceAmt { get; set; }
        public Nullable<double> DueAdvanceAmt { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public Nullable<System.DateTime> Updatedate { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> SessionId { get; set; }
    }
}
