namespace MVCAssignment.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductId { get; set; }

        public string ProductName { get; set; }
        public string  ProductImage { get; set; }
        public double ProductOriPrice { get; set; }

        public double ProductDisPrice { get; set; }

        public string ProductDescription { get; set; }
    }
}
