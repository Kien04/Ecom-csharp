using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MVCAssignment.Models;
using MVCAssignment.ViewModels;
using Stripe;
using Stripe.Issuing;

namespace MVCAssignment.Controllers
{
    public class PaymentController : Controller
    {

        private readonly DtoStripeSecrets _stripeSecrets;

        public PaymentController(IOptions<DtoStripeSecrets> stripeSecrets)
        {
            _stripeSecrets = stripeSecrets.Value;
            StripeConfiguration.ApiKey = _stripeSecrets.SecretKey; // 设置全局 API Key
        }

        static double Subtotal = 0;
        static string CartList = "";
        //static string tid = "";

        static string transactionid = "";
        AppDbContext db = new AppDbContext();

        public IActionResult PaymentCheckoutPage(double Amount, string CartIdList)//1 |2 |4|
        {
            //var findproductid = db.CartTable.FirstOrDefault();
            string cl=CartIdList;
            CartList = CartIdList.TrimEnd('|');
            List<Cart> TempCartlist = new List<Cart>();
            foreach (var pair in CartIdList.TrimEnd('|').Split('|'))
            {

                var selectedCart = db.CartTable.Where(e => e.CartId.ToString() == pair).FirstOrDefault();

                TempCartlist.Add(selectedCart);
                //var pid = db.CartTable.Where(e => e.CartId.ToString() == pair && e.Status=="Cart Pending").FirstOrDefault();
                selectedCart.Status = "Payment Processing";


            }


            db.SaveChanges();
            Subtotal = Amount;


            var LoginId = HttpContext.Session.GetString("LoginId");

            var joinTable = TempCartlist.Join(db.ProductsTable,//1 checkpoint
    x => x.Product_Id,//2
    z => z.ProductId,//3
    (x, z) => new CartViewModel
    {

        User_Id = x.User_Id,
        Product_Id = x.Product_Id,
        CartId = x.CartId,
        Quantity = x.Quantity,
        Status = x.Status,
        ProductName = z.ProductName,
        ProductImage = z.ProductImage,
        ProductDescription = z.ProductDescription,
        ProductOriPrice = z.ProductOriPrice,
        ProductDisPrice = z.ProductDisPrice,
        IsSelected = x.IsSelected,
    }).Where(x => x.User_Id == LoginId && (x.Status == "Cart Pending" || x.Status == "Payment Processing"));

            ViewBag.Subtotal = Amount;
            ViewBag.Tax = Amount * 0.06;
            ViewBag.GrandTotal = Amount * 1.06;

            foreach (var pair in CartIdList.TrimEnd('|').Split('|'))
            {
                var finf = db.transactionHistories.Where(e => e.User_Id == LoginId && e.CartList.Contains(CartIdList)).FirstOrDefault();

                if (finf!=null)
                {
                    break;
                }
                else
                {
                    TransactionHistory newRecord = new TransactionHistory();

                    Random random = new Random();
                    var randomNumber = random.Next(1000000, 9999999);

                    transactionid = $"T-{randomNumber}";

                    var findcustomer = db.CustomersTabel.Where(e => e.AccountId == LoginId).FirstOrDefault();
                    newRecord.TransactionId = $"T-{randomNumber}";//auto generate
                    newRecord.User_Id = LoginId;
                    newRecord.BillingAddress = findcustomer.Address1+""+findcustomer.Address2;
                    newRecord.CreateTime = DateTime.Now.ToString();
                    newRecord.SubTotal = Subtotal;
                    newRecord.Tax = Subtotal * 0.06;
                    newRecord.GrandTotal = Subtotal * 1.06;
                    newRecord.CartList = cl;
                    newRecord.PaidTime = "Haven't pay yet";
                    newRecord.TransactionStatus = "Pending";

                    db.transactionHistories.Add(newRecord);
                    db.SaveChanges();
                    break;
                }

            }
                

           
            //var amount = db.CartTable.Where(e=>e.User_Id==LoginId &&
            //e.Status=="Cart Pending").Sum()

            return View(joinTable);
        }

        [HttpPost]
        public IActionResult PaymentCheckoutPage(string Name, string CardNumber, string ExpMonth, string ExpYear, string Cvv, string CartIdList)
        {
            var LoginId = HttpContext.Session.GetString("LoginId");
            var findUser = db.CustomersTabel.Where(e => e.AccountId == LoginId).FirstOrDefault();

            try
            {
                StripePayment stripePayment = new StripePayment(new CreditCard
                {

                    Name = $"{findUser.FirstName} {findUser.LastName}",
                    Email = findUser.Email,
                    AddressLine1 = findUser.Address1,
                    AddressLine2 = findUser.Address2,
                    AddressCity = findUser.City,
                    AddressState = findUser.State,
                    AddressZip = "45000",
                    Descripcion = $"Purchase Successfull",
                    DetailsDescripcion = $"Purchase on  {DateTime.Now:d}",
                    Amount = Convert.ToInt64((Subtotal * 1.06) * 100),//2000=20,10000=100
                    Currency = "MYR",
                    Number = CardNumber,//4242 4242 4242 4242
                    ExpMonth = ExpMonth,
                    ExpYear = ExpYear, //2023
                    Cvc = Cvv //123

                }, _stripeSecrets);
                Charge charge = stripePayment.ProccessPayment();

                foreach (var pair in CartList.Split('|'))
                {
                    var selectedCart = db.CartTable.Where(e => e.CartId.ToString() == pair).FirstOrDefault();

                    selectedCart.Status = "Paid";
                }
                db.SaveChanges();





                //TransactionHistory newRecord = new TransactionHistory();
                //newRecord.TransactionId = "T-0001";
                //newRecord.User_Id = LoginId;
                //newRecord.BillingAddress = "No.2 Address 45200 Kuala Lumpur";
                //newRecord.CreateTime = DateTime.Now.ToString();
                //newRecord.SubTotal = Subtotal;
                //newRecord.Tax = Subtotal * 0.06; 
                //newRecord.GrandTotal = Subtotal * 1.06;
                //newRecord.CartList = CartList;
                //newRecord.PaidTime = "Paid";
                //newRecord.TransactionStatus = "Success";
                //db.transactionHistories.Add(newRecord);
                //db.SaveChanges();


                var tran = db.transactionHistories.Where(e => e.TransactionId == transactionid).FirstOrDefault();
                tran.TransactionStatus = "Success";
                tran.PaidTime = DateTime.Now.ToString();
                db.SaveChanges();
                return RedirectToAction("PaymentSuccess", "Payment");
            }
            catch (Exception ex)
            {
                foreach (var pair in CartList.Split('|'))
                {
                    var selectedCart = db.CartTable.Where(e => e.CartId.ToString() == pair).FirstOrDefault();

                    selectedCart.Status = "Payment Failed";
                }


                var tran = db.transactionHistories.Where(e => e.TransactionId == transactionid).FirstOrDefault();
                tran.TransactionStatus = "Failed";
                tran.PaidTime = "Paymnet Failed";


                db.SaveChanges();

                //~/CONTROLLER/ACTION
                return RedirectToAction("PaymentErrorPage", "Payment", new { Msg = ex.Message });
            }

            return View();
        }

        public IActionResult PaymentErrorPage(string Msg)
        {
            ViewBag.ErrorMsg = Msg;
            return View();
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }

    }
}
