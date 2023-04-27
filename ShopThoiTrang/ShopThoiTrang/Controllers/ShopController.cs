using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index(string cateName, string search)
        {
            IQueryable<Category> categories = DataController.GetCategories();
            IQueryable<Product> products = DataController.RandomProduct(DataController.GetProducts("New", ""),5);
            IQueryable<Product> all_product =  DataController.GetProducts(search, cateName);
            return View(products, all_product, categories);
        }
        
        private ActionResult View(IQueryable<Product> products, IQueryable<Product> all_product, IQueryable<Category> categories)
        {
            ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.AllProducts = all_product;
            return View();
        }

        [HttpPost]
        public ActionResult SearrchByCategory(string cateName)
        {
            return RedirectToAction("Index", new { cateName = cateName });
        }
        // GET: Shop/Details/5
        public ActionResult Details(int id)
        {

            IQueryable<Category> categories = DataController.GetCategories();
            var product = DataController.GetProduct(id);
            IQueryable<Product> products = DataController.RandomProduct(DataController.GetProducts("", ""), 6);
            IQueryable<ImageProduct> img = DataController.Image(id);
            return View(product, products, categories, img);
        }

        private ActionResult View(Product product, IQueryable<Product> products, IQueryable<Category> categories, IQueryable<ImageProduct> img)
        {
            ViewBag.Categories = categories;
            ViewBag.product = product;
            ViewBag.Products = products;
            ViewBag.Image = img;
            return View();
        }

    }
}
