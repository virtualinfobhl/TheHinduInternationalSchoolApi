using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data
{
    public class BooksTbl
    {
        [Key]
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookNumber { get; set; }
        public string? Description { get; set; }
        public string? Publisher { get; set; }
        public string? Author { get; set; }
        public string? Subject { get; set; }
        public Nullable<long> TotalQuantity { get; set; }
        public Nullable<long> RQuantity { get; set; }
        public Nullable<long> RentQuantity { get; set; }
        public Nullable<double> BookPrice { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Userid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SessionId { get; set; }


        //public int BookId { get; set; }
        //public string? BookTitle { get; set; }
        //public string? BookNumber { get; set; }
        //public Nullable<System.DateTime> Date { get; set; }
        //public string? Publisher { get; set; }
        //public string? Author { get; set; }
        //public string? Subject { get; set; }
        //public Nullable<long> TotalQuantity { get; set; }
        //public Nullable<double> BookPrice { get; set; }
        //public string? Description { get; set; }
        //public Nullable<bool> Active { get; set; }
        //public Nullable<System.DateTime> CreateDate { get; set; }
        //public Nullable<System.DateTime> UpdateDate { get; set; }
        //public Nullable<int> Userid { get; set; }
        //public Nullable<int> SchoolId { get; set; }
        //public Nullable<int> BranchId { get; set; }
        //public Nullable<int> SessionId { get; set; }
    }
}
