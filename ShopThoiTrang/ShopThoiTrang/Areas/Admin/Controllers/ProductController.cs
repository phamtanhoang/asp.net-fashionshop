using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var prod = DataAdminController.GetProducts("", "");
                return View(prod);
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Product product = DataAdminController.GetProductByID(id);
                if (product == null)
                {

                    return HttpNotFound();
                }
                var cate = DataAdminController.GetCategories();
                IEnumerable<Tag> tags = DataAdminController.GetTagsByProductID(id);
                return View(product, cate, tags);
            }
            return RedirectToAction("", "Login");
        }

        private ActionResult View(Product product, IQueryable<Category> cate, IEnumerable<Tag> tags)
        {
            ViewBag.product = product;
            ViewBag.category = cate;
            ViewBag.tags = tags;
            return View();
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var cate = DataAdminController.GetCategories();
                var tag = DataAdminController.GetTags();
                return View(cate, tag);
            }
            return RedirectToAction("", "Login");
            
        }

        private ActionResult View(IQueryable<Category> cate, IQueryable<Tag> tag)
        {
            ViewBag.cate = cate;
            ViewBag.tag = tag;
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [Obsolete]
        public ActionResult Create(Product product, HttpPostedFileBase Image, int[] TagID)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                try
                {
                    using (var db = new ShopThoiTrangEntities())
                    {
                        var duplicateProd = db.Products.FirstOrDefault(p => p.ProductName == product.ProductName);
                        if (duplicateProd == null)
                        {

                            if (ModelState.IsValid)
                            {
                                product.Image = CloudinaryController.UploadImage(Image);
                                if (product.Image != "")
                                {
                                    bool added = DataAdminController.AddProduct(product, TagID);
                                    if (added)
                                    {
                                        TempData["SuccessMessage"] = "Thêm " + product.ProductName + " thành công";
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        TempData["ErrorMessage"] = product.ProductName + " đã tồn tại";
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm";
                                }
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = product.ProductName + " đã tồn tại";
                        }
                    }

                }
                catch
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm";
                }

                var cate = DataAdminController.GetCategories();
                var tag = DataAdminController.GetTags();
                return View(cate, tag);
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Product product = DataAdminController.GetProductByID(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                var cate = DataAdminController.GetCategories();
                var tag = DataAdminController.GetTags();
                var tagprod = DataAdminController.GetTagsByProductID(id);
                return View(product, cate, tag, tagprod);
            }
            return RedirectToAction("", "Login");
            
        }
        private ActionResult View(Product product, IQueryable<Category> cate, IEnumerable<Tag> tags, IQueryable<Tag> tagprod)
        {
            ViewBag.product = product;
            ViewBag.category = cate;
            ViewBag.tags = tags;
            ViewBag.tagProd = tagprod;
            return View();
        }
        // POST: Admin/Product/Edit/5
        [HttpPost]
        [Obsolete]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase Image, int[] TagID)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var prod = DataAdminController.GetProductByID(id);
                try
                {
                    if (prod == null)
                    {
                        return HttpNotFound();
                    }
                    prod.ProductName = collection["ProductName"];
                    prod.UnitPrice = Decimal.Parse(collection["UnitPrice"]);
                    prod.active = Boolean.Parse(collection["active"]);
                    prod.CategoryID = Int32.Parse(collection["CategoryID"]);
                    prod.Description = collection["Description"];
                    using (var db = new ShopThoiTrangEntities())
                    {
                        var duplicateProd = db.Products.FirstOrDefault(p => p.ProductName == prod.ProductName && p.ProductID != id);
                        if (duplicateProd == null)
                        {

                            if (ModelState.IsValid)
                            {
                                prod.Image = CloudinaryController.UploadImage(Image);
                            }
                            var result = DataAdminController.EditProduct(prod, TagID);
                            if (result)
                            {
                                TempData["SuccessMessage"] = "Chỉnh sửa thành công";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Chỉnh sửa không thành công";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Chỉnh sửa không thành công";
                        }
                    }
                }
                catch
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa sản phẩm";
                }
                var cate = DataAdminController.GetCategories();
                var tag = DataAdminController.GetTags();
                var tagprod = DataAdminController.GetTagsByProductID(id);
                return View(prod, cate, tag, tagprod);
            }
            return RedirectToAction("", "Login");
            
        }
        private ActionResult View(Product product, IQueryable<Category> cate)
        {
            ViewBag.product = product;
            ViewBag.category = cate;
            return View();
        }
        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Product product = DataAdminController.GetProductByID(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                var cate = DataAdminController.GetCategories();
                return View(product, cate);
            }
            return RedirectToAction("", "Login");
            
        }

        // POST: Admin/Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var prod = DataAdminController.GetProductByID(id);
                try
                {
                    if (prod == null)
                    {
                        return HttpNotFound();
                    }

                    var result = DataAdminController.DeleteProduct(prod);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Xóa thành công";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Xóa không thành công";
                    }
                }
                catch
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa loại sản phẩm";
                }
                return View(prod);
            }
            return RedirectToAction("", "Login");
            
        }
    }
}
