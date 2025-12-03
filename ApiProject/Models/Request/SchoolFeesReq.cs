using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models.Request
{

    public class AddClassFeesReq
    {
        [Required]
        public int? ClassId { get; set; }
        public double? admission_fee { get; set; }
        public double? tution_fee { get; set; }
        public double? exam_fee { get; set; }
        public double? Develoment_fee { get; set; }
        public double? Games_fees { get; set; }
        public double? total { get; set; }
    }
    public class UpdateClassFeesReq
    {
        public int? fee_id { get; set; }
        [Required]
        public int? ClassId { get; set; }
        public double? admission_fee { get; set; }
        public double? tution_fee { get; set; }
        public double? exam_fee { get; set; }
        public double? Develoment_fee { get; set; }
        public double? Games_fees { get; set; }
        public double? total { get; set; }
    }
    public class ClassFeesReq
    {
        public int? fee_id { get; set; }

        [Required]
        public int? ClassId { get; set; }
        public double? admission_fee { get; set; }
        public double? tution_fee { get; set; }
        public double? exam_fee { get; set; }
        public double? Develoment_fee { get; set; }
        public double? Games_fees { get; set; }
        public double? total { get; set; }
        public bool? active { get; set; }
    }

    // FEE iNSTALLMENT
    public class AddFeesInstallmentReq
    {
        public int? ClassId { get; set; }
        public double? TotalFee { get; set; }
        public string? Installmentno { get; set; }
        public string? Installment { get; set; }
        public double? InsAmount { get; set; }
       // public DateTime? Date { get; set; }
    }

    public class FeesInstReq
    {
        public int? InstallmentId { get; set; }

        [Required(ErrorMessage = "ClassId is mandatory.")]
        public int? ClassId { get; set; }
        public double? InsAmount { get; set; }
        public DateTime? Date { get; set; }
        public string? Installment { get; set; }
        public int? Installmentno { get; set; }
    }

    public class StudentFeesDetailReq
    {
        [Required]
        public int? ClassId { get; set; }

        [Required]
        public int? StudentId { get; set; }
      //  public string? srno { get; set; }

    }
    public class StudentFeesReq
    {
        [Required]
        public int? ClassId { get; set; }

        [Required]
        public int? StudentId { get; set; }

        [Required]
        public double? PayFees { get; set; }

        [Required]
        public string? PaymentMode { get; set; }

        [Required]
        public DateTime? PaymentDate { get; set; }

        public string? Remark { get; set; }
        public double Cash { get; set; }
        public double Upi { get; set; }
    }

    public class FeesCollectionReq
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }


    public class ClassFeesFilterReq
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int? ClassId { get; set; }
        public int? SectionId { get; set; }

        public string? srno { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


    }
}
