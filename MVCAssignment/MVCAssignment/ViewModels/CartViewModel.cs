using System.ComponentModel.DataAnnotations;

namespace MVCAssignment.ViewModels
{
    public class CartViewModel
    {
        [Key]
        public int CartId { get; set; }
        public string Product_Id { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string User_Id { get; set; }
        public bool IsSelected { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double ProductOriPrice { get; set; }

        public double ProductDisPrice { get; set; }

        public string ProductDescription { get; set; }


    }
}
