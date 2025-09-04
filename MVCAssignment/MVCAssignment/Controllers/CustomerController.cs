using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MVCAssignment.Models;
using MVCAssignment.ViewModels;
using Stripe;
using System.Security.Cryptography;

namespace MVCAssignment.Controllers
{
    public class CustomerController : Controller
    {
        static string CartList="";
        AppDbContext db = new AppDbContext();
        public IActionResult ProductPage()
        {
            ViewBag.LoginId = HttpContext.Session.GetString("LoginId");
            var LoginId = HttpContext.Session.GetString("LoginId");

            if (LoginId == null)
            {
                ViewBag.loginmenu = "x";
            }
            return View(db.ProductsTable);
        }
        public IActionResult CustomerProfile()
        {
            var LoginId = HttpContext.Session.GetString("LoginId");
            if (LoginId == null)
            {
                ViewBag.loginmenu = "x";
            }
            var selectedUser = db.CustomersTabel.Where(e => e.AccountId == LoginId).FirstOrDefault();
            if (selectedUser==null)
            {
  
                return RedirectToAction("CustomerLogin", "Login");
            }
            return View(selectedUser);

        }
        public IActionResult AddToCart(string ProductId)
        {
            var LoginId = HttpContext.Session.GetString("LoginId");

            if (LoginId == null)
            {
                return RedirectToAction("CustomerLogin", "Login");
            }
            var CheckProduct = db.CartTable.Where(e => e.Product_Id == ProductId && 
            e.User_Id == LoginId && e.Status == "Cart Pending").FirstOrDefault();
            if (CheckProduct != null)
            {
                CheckProduct.Quantity += 1;
            }
            else
            {
                db.CartTable.Add(new Cart

                {

                    IsSelected = true,
                    Product_Id = ProductId,

                    Quantity = 1,
                    Seller_Id = "Admin",
                    Status = "Cart Pending",
                    User_Id = LoginId,
                });
            }
            db.SaveChanges();

            //  ViewBag.texting = "Your selected Item"+ProductId;
            return RedirectToAction("ProductPage");

        }

        public IActionResult CartPage(string productid)
        {
            var LoginId = HttpContext.Session.GetString("LoginId");

            var find = db.CartTable.Where(e => LoginId == e.User_Id && e.Status== "Cart Pending").FirstOrDefault();
            if (find==null)
            {
                ViewBag.pendingpage = "x";
              
            }
            var findprocessing = db.CartTable.Where(e => LoginId == e.User_Id && e.Status == "Payment Processing").FirstOrDefault();
            if (findprocessing==null)
            {
                ViewBag.processingpage = "x";


            }
            if (LoginId == null)
            {
                ViewBag.loginmenu = "x";
                return RedirectToAction("CustomerLogin", "Login");
            }
            var CheckProduct = db.CartTable.Where(e => e.Product_Id == productid &&
           e.User_Id == LoginId && e.Status == "Payment Processing").FirstOrDefault();
            if (CheckProduct != null)
            {
                CheckProduct.Quantity += 1;
                db.SaveChanges();
            }
            var joinTable = db.CartTable.Join(db.ProductsTable,//1 checkpoint
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
                }).Where(x => x.User_Id == LoginId && (x.Status=="Cart Pending" || x.Status == "Payment Processing"));
            //var paymentprocessing = db.CartTable.Where(e => e.User_Id == LoginId && e.Status == "Payment Processing" &&e.Product_Id==productid).FirstOrDefault();


            ViewBag.Subtotal=joinTable.Sum(x => x.ProductDisPrice*x.Quantity);
            ViewBag.Tax = joinTable.Sum(x => (x.ProductDisPrice * x.Quantity)*0.06);
            ViewBag.GrandTotal = Convert.ToDouble(joinTable.Sum(x => (x.ProductDisPrice * x.Quantity)*1.06));
            return View(joinTable);
        }


        public IActionResult AddQty(int CartId)
        {

            var selectedItem = db.CartTable.Where(e => e.CartId == CartId).First();

            selectedItem.Quantity += 1;
            db.SaveChanges();
            return RedirectToAction("CartPage");
        }

        public IActionResult MinusQty(int CartId)
        {

            var selectedItem = db.CartTable.Where(e => e.CartId == CartId).First();
            if (selectedItem.Quantity==0)
            {
                db.CartTable.Remove(selectedItem);
                db.SaveChanges();
                return RedirectToAction("CartPage");

            }
            selectedItem.Quantity -= 1;
            db.SaveChanges();
            return RedirectToAction("CartPage");
        }

        public IActionResult TransactionHistories(string CartList)
        {
            var LoginId = HttpContext.Session.GetString("LoginId");

            if (LoginId == null)
            {
                return RedirectToAction("CustomerLogin", "Login");
            }
            var th = db.transactionHistories.Where(e => e.User_Id == LoginId && (e.TransactionStatus == "Pending"|| e.TransactionStatus == "Success")).ToList();
            return View(th);
        }

        public IActionResult AboutUs ()
        {
            var LoginId = HttpContext.Session.GetString("LoginId");
            if (LoginId == null)
            {
                ViewBag.loginmenu = "x";
            }
            return View();
        }


        

     
        //public IActionResult cl (string CartList)
        //{
        //    var LoginId = HttpContext.Session.GetString("LoginId");

        //    var findcl = db.transactionHistories.Where(e => e.CartList == CartList).FirstOrDefault();
        //    CartList;
            

        //    var joinTable = db.CartTable.Join(db.ProductsTable,//1 checkpoint
        //        x => x.Product_Id,//2
        //        z => z.ProductId,//3
        //        (x, z) => new CartViewModel
        //        {

        //            User_Id = x.User_Id,
        //            Product_Id = x.Product_Id,
        //            CartId = Convert.ToInt32(cid),
        //            Quantity = x.Quantity,
        //            Status = x.Status,
        //            ProductName = z.ProductName,
        //            ProductImage = z.ProductImage,
        //            ProductDescription = z.ProductDescription,
        //            ProductOriPrice = z.ProductOriPrice,
        //            ProductDisPrice = z.ProductDisPrice,
        //            IsSelected = x.IsSelected,
        //        }).Where(x => x.User_Id == LoginId && (x.Status == "Payment Failed"));

        //    ViewBag.SubTotal = joinTable.Sum(e => e.Quantity * e.ProductDisPrice);
        //    return RedirectToAction("PaymentCheckoutPage", "Payment");
        //}
    }
}
