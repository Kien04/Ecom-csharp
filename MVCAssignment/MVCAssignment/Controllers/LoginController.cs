using Microsoft.AspNetCore.Mvc;
using MVCAssignment.Models;

namespace MVCAssignment.Controllers
{
    public class LoginController : Controller
    {
        AppDbContext db = new AppDbContext();

        public IActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerLogin(string Email , string Password)
        {
            var checkpassword = db.CustomersTabel.Where(e => e.Password == Password&& e.Email==Email).FirstOrDefault();
 
            if (checkpassword != null)
            {
                HttpContext.Session.SetString("LoginId", checkpassword.AccountId);
                var x = HttpContext.Session.GetString("LoginId");
                return RedirectToAction("ProductPage", "Customer");
            }
            ViewBag.ErrorMsg = "Incorrect Password";
            return View();
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(string username,string password)
        {
            if (username=="Admin1" && password=="123")
            {
                return RedirectToAction("ProductList", "AdminManageProduct");
            }
            ViewBag.error = "Incorrect username or password";
            return View();
        }
    }
}
