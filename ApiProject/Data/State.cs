using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class State
    {
        [Key]
        public short? State_Code { get; set; }
        public string? State_Name { get; set; }
        public byte State_Priority { get; set; }
        public bool State_Active { get; set; }
       
    }
}
