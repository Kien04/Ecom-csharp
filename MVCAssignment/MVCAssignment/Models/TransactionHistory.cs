namespace MVCAssignment.Models
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string CreateTime { get; set; }
        public string PaidTime { get; set; }
        public string TransactionStatus { get; set; }
        public string CartList { get; set; }
        public string BillingAddress { get; set; }
        public string User_Id { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double GrandTotal { get; set; }
    }
}
