using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class SessionInfo
    {
        [Key]
        public int Id { get; set; }
        public string? StartSession { get; set; }
        public string? EndSession { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> branch_id { get; set; }
    }
}
