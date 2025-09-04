using System.ComponentModel.DataAnnotations;

namespace MVCAssignment.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public string Product_Id { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string? Seller_Id { get; set; }
        public string User_Id { get; set; }
        public bool IsSelected { get; set; }

    }
}
