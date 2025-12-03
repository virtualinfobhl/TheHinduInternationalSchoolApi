using System.ComponentModel.DataAnnotations;

namespace ApiProject.Models.Response
{
    public class ClassFeesRes
    {
        public int? fee_id { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }

        public double? admission_fee { get; set; }
        public double? tution_fee { get; set; }
        public double? exam_fee { get; set; }
        public double? Develoment_fee { get; set; }
        public double? Games_fees { get; set; }
        public double? total { get; set; }
        public bool? active { get; set; }

    }

    public class FeesInstRes
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public double? total { get; set; }
        public List<InstallmentDetail> installments { get; set; }
    }

    public class InstallmentDetail
    {
        public string? Installmentno { get; set; }
        public string? Installment { get; set; }
        public Nullable<double> FeeAmount { get; set; }

    }

    public class ClassIdByStudentRes
    {
        public int ClassId { get; set; }
        public string? stu_name { get; set; }
        public int? StuId { get; set; }

    }

    public class StudentFeesDetailRes
    {
        public int ClassId { get; set; }
        public string? stu_name { get; set; }
        public int? StudentId { get; set; }
        public string? srno { get; set; }
        public decimal TotalPaid { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? fathername { get; set; }
        public string? fathermobileno { get; set; }
        public Nullable<bool> RTE { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Tution_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public Nullable<double> total_fee { get; set; }

    }

    public class StudentFeesRes
    {
        public int receiptId { get; set; }
    }


    public class StudentFeesReceiptRes
    {
        public string stu_name { get; set; }
        public string? srno { get; set; }
        public string ReceiptNo { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? fathername { get; set; }
        public string? fathermobileno { get; set; }
        public double? PayFees { get; set; }
        public string? PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Remark { get; set; }
    }


    public class StudentFeesCollectionListRes
    {
        public int ReceiptId { get; set; }
        public string stu_name { get; set; }
        public string? srno { get; set; }
        public string ReceiptNo { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? fathername { get; set; }
        public string? fathermobileno { get; set; }
        public double? PayFees { get; set; }
        public string? PaymentMode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Remark { get; set; }

    }


    public class ClassFeesListSummary
    {
        public List<ClassFeesListRes> Data { get; set; }
        public decimal TotalAdmissionFee { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalFee { get; set; }
    }
    public class ClassFeesTotals
    {
        public decimal TotalAdmissionFee { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalFee { get; set; }
    }

    public class ClassFeesListRes
    {
        public string stu_name { get; set; }
        public string? srno { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? fathername { get; set; }
        public string? fathermobileno { get; set; }
        public decimal TotalPaid { get; set; }
        public Nullable<bool> RTE { get; set; }

        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Tution_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public Nullable<double> total_fee { get; set; }

    }

    public class ClassFeesInstaListRes
    {
        public string stu_name { get; set; }
        public string? srno { get; set; }
        public string? ClassName { get; set; }
        public string? SectionName { get; set; }
        public string? fathername { get; set; }
        public string? fathermobileno { get; set; }
        public decimal TotalPaid { get; set; }
        public Nullable<bool> RTE { get; set; }
        public Nullable<double> admission_fee { get; set; }
        public Nullable<double> PramoteFees { get; set; }
        public Nullable<double> AFeeDiscount { get; set; }
        public Nullable<double> AdmissionPayfee { get; set; }
        public Nullable<double> exam_fee { get; set; }
        public Nullable<double> Tution_fee { get; set; }
        public Nullable<double> Develoment_fee { get; set; }
        public Nullable<double> Games_fees { get; set; }
        public Nullable<double> total { get; set; }
        public Nullable<double> discount { get; set; }
        public Nullable<double> OldDuefees { get; set; }
        public Nullable<double> total_fee { get; set; }

        public List<FeeInstallment>? FeeInstallment { get; set; }

    }
    public class ClassWiseTotalFeeModel
    {
        public int? ClassId { get; set; }
        public Nullable<double> TotalFee { get; set; }
    }


    public class FeeInstallment
    {
        public Nullable<int> StudentId { get; set; }
        public Nullable<double> SInsAmount { get; set; }
    }
}
