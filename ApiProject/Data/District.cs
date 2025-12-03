using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class District
    {

        [Key]
        public int District_Code { get; set; }
        public string? District_Name { get; set; }
        public int? State_Code { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> branch_id { get; set; }
    }
}
