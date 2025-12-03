namespace ApiProject.Models.Response
{
    public class ClassByFeeInResponse
    {
        public int? ClassId { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> tution_fee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }

        public List<FeeInstallmentClass> FeeInstallments { get; set; }
     //   public List<SectionDto> Sections { get; set; }

        public class FeeInstallmentClass
        {
         //   public int? ClassId { get; set; }
            public string? Installmentno { get; set; }
            public double? SInsAmount { get; set; }
        }

        public class SectionDto
        {
            public int? SchoolId { get; set; }
            public int? ClassId { get; set; }
            public int? SectionId { get; set; }
            public string? SectionName { get; set; }
            public int? SectionPriority { get; set; }
        }

    }
}
