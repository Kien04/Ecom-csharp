using Microsoft.AspNetCore.Mvc;
using MVCAssignment.Models;
using System.Text.RegularExpressions;

namespace MVCAssignment.Controllers
{
    public class AdminManageCustomerController : Controller
    {
        AppDbContext db = new AppDbContext();

     
        public IActionResult CustomerList()
        {
            return View(db.CustomersTabel);
        }

        public IActionResult CreateCustomer()
        {
            return View();

        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer NewCustomer, string confirmpassword)
        {
            Regex ICFormat1 = new Regex("^\\d{6}\\-\\d{2}\\-\\d{4}$");
            Regex ICFormat2 = new Regex("^\\d{5}\\-\\d{2}\\-\\d{4}$");
            if (ICFormat1.IsMatch(NewCustomer.NRIC) == false && ICFormat2.IsMatch(NewCustomer.NRIC) == false)
            {
                ViewBag.ErrorICFormat = "Please Follow NRIC Format XXXXXX-XX-XXXX";
                return View();
            }
            else if (NewCustomer.NRIC==null)
            {
                ViewBag.ErrorICFormat = "Please Follow NRIC Format XXXXXX-XX-XXXX";

                return View();

            }
            var finemail = db.CustomersTabel.Where(e => e.Email == NewCustomer.Email).FirstOrDefault();
             if (finemail!=null)
            {
                ViewBag.email = "This email already exists";
                return View();

            }
            if (NewCustomer.Password != confirmpassword)
            {
                ViewBag.ErrorPassword = "Incorrect Password";
                return View();
            }
            int x = db.CustomersTabel.Count();
            x++;
            NewCustomer.AccountId = $"C-000{x}";
            var findudplicate = db.CustomersTabel.Where(e => e.AccountId == NewCustomer.AccountId).FirstOrDefault();
            if (findudplicate != null)
            {
                x++;
                x++;
                NewCustomer.AccountId = $"C-000{x}";

            }

            //Random random = new Random();
            //var randomNumber = random.Next(1000000, 9999999);
            //NewCustomer.AccountId = $"C-{randomNumber}";
            //var findudplicate = db.CustomersTabel.Where(e => e.AccountId== NewCustomer.AccountId).FirstOrDefault();

            //if (findudplicate!=null)
            //{

            //    randomNumber = random.Next(1000000, 9999999);
            //    NewCustomer.AccountId = $"C-{randomNumber}";

            //}
            //var find = db.CustomersTabel.Where(e => e.AccountId == NewCustomer.AccountId).FirstOrDefault();
            //if (find!=null)
            //{
            //    x++;
            //    int[] i = { 10, 100 };
            //    if (x >= 10)
            //    {
            //        NewCustomer.AccountId = $"C-00{x}";

            //    }
            //    else
            //    {
            //        NewCustomer.AccountId = $"C-000{x}";

            //    }

            //}
            if (NewCustomer.Password.Length<6 || NewCustomer.Password.Length >10)
            {
                ViewBag.ErrorPasswordDigit = "Please Enter Password Digit 6-10";
                return View();
            }
            db.CustomersTabel.Add(NewCustomer);
            db.SaveChanges();
            return RedirectToAction("CustomerList");
        }

        public IActionResult DeleteCustomer(int Id)
        {
            var delete = db.CustomersTabel.Where(e => e.Id == Id).FirstOrDefault();
            return View(delete);

        }

        [HttpPost]
        public IActionResult DeleteCustomer(Customer DeleteCustomer)
        {
            var delete = db.CustomersTabel.Where(e => e.Id == DeleteCustomer.Id).FirstOrDefault();
            db.CustomersTabel.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("CustomerList");

        }

        public IActionResult CustomerDetails(int Id)
        {
            var details = db.CustomersTabel.Where(e=>e.Id== Id).FirstOrDefault();
            return View(details);
        }

        public IActionResult EditCustomer(int Id)
        {
            var findcustomer = db.CustomersTabel.Where(e => e.Id == Id).FirstOrDefault();
            return View(findcustomer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Customer EditCustomer)
        {
            var findcustomer = db.CustomersTabel.Where(e => e.Id == EditCustomer.Id).FirstOrDefault();
            findcustomer.FirstName = EditCustomer.FirstName;
            findcustomer.LastName = EditCustomer.LastName;
            findcustomer.Email = EditCustomer.Email;
            findcustomer.Address1 = EditCustomer.Address1;
            findcustomer.Address2 = EditCustomer.Address2;
            findcustomer.State = EditCustomer.State;
            findcustomer.PostalCode = EditCustomer.PostalCode;
            findcustomer.Country = EditCustomer.Country;
            findcustomer.DateOfBirth = EditCustomer.DateOfBirth;
            findcustomer.Gender=EditCustomer.Gender;
            findcustomer.CountryCode = EditCustomer.CountryCode;
            findcustomer.UserImage = EditCustomer.UserImage;
            findcustomer.PhoneNumber = EditCustomer.PhoneNumber;
            findcustomer.NRIC = EditCustomer.NRIC;
            Regex ICFormat1 = new Regex("^\\d{6}\\-\\d{2}\\-\\d{4}$");
            Regex ICFormat2 = new Regex("^\\d{5}\\-\\d{2}\\-\\d{4}$");
            if (ICFormat1.IsMatch(findcustomer.NRIC) == false && ICFormat2.IsMatch(findcustomer.NRIC) == false)
            {
                ViewBag.ErrorICFormat = "Please Follow NRIC Format XXXXXX-XX-XXXX";
                return View();
            }
            findcustomer.Password = EditCustomer.Password;
            findcustomer.City = EditCustomer.City;
            db.SaveChanges();
            return RedirectToAction("CustomerList");
        }

    }

}
