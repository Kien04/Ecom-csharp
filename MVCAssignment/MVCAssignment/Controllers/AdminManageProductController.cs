using Microsoft.AspNetCore.Mvc;
using MVCAssignment.Models;
using System.Security.Cryptography;

namespace MVCAssignment.Controllers
{
    public class AdminManageProductController : Controller
    {
        AppDbContext db = new AppDbContext();

        public IActionResult ProductList()
        {
            return View(db.ProductsTable);
        }

        public IActionResult CreateProduct()
        {
            int x = db.ProductsTable.Count()+1;
          
            ViewBag.GenerateProductID = $"000{x}";
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            var checkProductId=db.ProductsTable.Where(e=>e.ProductId==product.ProductId).FirstOrDefault();  
            if (checkProductId!=null)
            {
                ViewBag.Msg = "This Product ID already exists";
                return View();

            }
             if (product.ProductOriPrice<=0 )
            {
                ViewBag.OriPriceErrorMsg = "Ori Price Cannot less than 0";
                return View();
            }
            if (product.ProductDisPrice <0)
            {
                ViewBag.DisPriceErrorMsg = "Dis Price Cannot less than 0";
                return View();
            }
            else if (product.ProductDisPrice==0)
            {
                product.ProductDisPrice = product.ProductOriPrice;
            }
            else if (product.ProductDisPrice> product.ProductOriPrice)
            {
                ViewBag.DisPriceErrorMsg = "Dis Price Cannot greater than Product Ori Price";

                return View();
            }
            int x = db.ProductsTable.Count();
            x++;

            product.ProductId = $"P-000{x}";
            db.ProductsTable.Add(product);
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public IActionResult Edit(int Id)
        {
            var findpro = db.ProductsTable.Where(e => Id == e.Id).FirstOrDefault();

            return View(findpro);

        }
        [HttpPost]
        public IActionResult Edit(Product EditProduct)
        {
            var findpro = db.ProductsTable.Where(e => EditProduct.Id == e.Id).FirstOrDefault();
            findpro.ProductName = EditProduct.ProductName;
            if (EditProduct.ProductName == null)
            {
                ViewBag.ErrorName = "Please Enter Product Name";
                return View();
            }
            //ViewBag.pid = findpro.ProductId;
            findpro.ProductOriPrice = EditProduct.ProductOriPrice;
            findpro.ProductDisPrice = EditProduct.ProductDisPrice;
            findpro.ProductImage = EditProduct.ProductImage;
            findpro.ProductDescription = EditProduct.ProductDescription;
            if (EditProduct.ProductDescription==null)
            {
                ViewBag.ErrorDescription = "Please Enter Product Description";
                return View();
            }
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }

        public IActionResult Delete(int Id)
        {
            var findpro = db.ProductsTable.Where(e => Id == e.Id).FirstOrDefault();

            return View(findpro);
        }

        [HttpPost]
        public IActionResult Delete(Product DeleteProduct)
        {
            var findpro = db.ProductsTable.Where(e => DeleteProduct.Id == e.Id).FirstOrDefault();
            db.ProductsTable.Remove(findpro);
            db.SaveChanges();
            return RedirectToAction("ProductList");
        }

            public IActionResult Details(int Id)
        {
            var findpro = db.ProductsTable.Where(e => Id == e.Id).FirstOrDefault();


            return View(findpro);
        }

        //public IActionResult EditProduct(string PID)
        //{
        //    var findpro = db.ProductsTable.Where(e => PID == e.ProductId).FirstOrDefault();

        //    return RedirectToAction("ProductList");
        //}

    }
}
