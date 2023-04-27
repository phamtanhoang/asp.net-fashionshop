using Newtonsoft.Json;
using ShopThoiTrang.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IQueryable<Category> categories = DataController.GetCategories();
            IQueryable<Product> products = DataController.GetProducts("New","");
            IQueryable<Product> all_product = DataController.RandomProduct(DataController.GetProducts("", ""),10);
            IQueryable<Product> products1 = DataController.RandomProduct(DataController.GetProducts("", "Áo"), 6);
            IQueryable<Product> products2 = DataController.RandomProduct(DataController.GetProducts("", "Quần"),6);
            IQueryable<Product> products3 = DataController.RandomProduct(DataController.GetProducts("", "Phụ kiện"),6);
            return View(categories,products, all_product, products1, products2, products3);
        }

        private ActionResult View(IQueryable<Category> categories, IQueryable<Product> products, IQueryable<Product> all_product, IQueryable<Product> products1, IQueryable<Product> products2, IQueryable<Product> products3)
        {
            ViewBag.Categories = categories;
            ViewBag.Products = products;           
            ViewBag.RandomProducts = all_product;    
            ViewBag.RandomProducts1 = products1.ToList();
            ViewBag.RandomProducts2 = products2.ToList();
            ViewBag.RandomProducts3 = products3.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Complain(string complain)
        {
            if (Session["UserID"] != null)
            {
                Complain com = new Complain
                {
                    ComplainContent = complain,
                    CreateDate = DateTime.Now,
                    UserID = (int)Session["UserID"]
                };
                DataController.AddComplain(com);

                if (Request.UrlReferrer != null)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
                return RedirectToAction("index");
            }
            return HttpNotFound();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}