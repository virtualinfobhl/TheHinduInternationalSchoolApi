using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class fees
    {
        [Key]

        public int fee_id { get; set; }
        public Nullable<int> university_id { get; set; }
        public Nullable<int> college_id { get; set; }
        public Nullable<int> course_id { get; set; }
        public Nullable<int> YearID { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> tution_fee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int fee_id { get; set; }
        //public Nullable<int> ClassId { get; set; }
        //public Nullable<double> admission_fee { get; set; }
        //public Nullable<double> tution_fee { get; set; }
        //public Nullable<double> exam_fee { get; set; }
        //public Nullable<double> Develoment_fee { get; set; }
        //public Nullable<double> Games_fees { get; set; }
        //public Nullable<double> total { get; set; }
        //public Nullable<bool> active { get; set; }
        //public Nullable<int> Userid { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> SessionId { get; set; }
        //public System.DateTime? CreateDate { get; set; }
        //public System.DateTime? UpdateDate { get; set; }

    }
}
