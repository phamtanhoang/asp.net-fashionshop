
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class ImageProductController : Controller
    {
        // GET: Admin/ImageProduct
        public ActionResult Index()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var imgs = DataAdminController.GetImageProducts();
                return View(imgs);
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/ImageProduct/Details/5
        public ActionResult Details(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                ImageProduct img = DataAdminController.GetImageProductByID(id);
                if (img == null)
                {
                    return HttpNotFound();
                }
                Product prod = DataAdminController.GetProductByID(img.ProductID.Value);
                return View(img, prod);
            }
            return RedirectToAction("", "Login");
            
        }

        private ActionResult View(ImageProduct img, Product prod)
        {
            ViewBag.img = img;
            ViewBag.prod = prod;
            return View();
        }

        // GET: Admin/ImageProduct/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var products = DataAdminController.GetProducts("", "");
                return View(products);
            }
            return RedirectToAction("", "Login");
            
        }

        // POST: Admin/ImageProduct/Create
        [HttpPost]
        [Obsolete]
        public ActionResult Create(ImageProduct imgprod,FormCollection collection, HttpPostedFileBase Image)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        imgprod.Image = CloudinaryController.UploadImage(Image);
                        if (imgprod.Image != "")
                        {
                            bool added = DataAdminController.AddImageProduct(imgprod);
                            if (added)
                            {
                                TempData["SuccessMessage"] = "Thêm ảnh sản phẩm thành công";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Thêm ảnh sản phẩm không thành công";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm";
                        }
                    }

                }
                catch
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm";
                }
                var prod = DataAdminController.GetProducts("", "");
                return View(prod);
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/ImageProduct/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                ImageProduct img = DataAdminController.GetImageProductByID(id);
                if (img == null)
                {
                    return HttpNotFound();
                }
                var products = DataAdminController.GetProducts("", "");
                return View(img, products);
            }
            return RedirectToAction("", "Login");
            
        }

        private ActionResult View(ImageProduct img, IQueryable<Product> products)
        {
            ViewBag.img = img;
            ViewBag.products = products;
            return View();
        }

        // POST: Admin/ImageProduct/Edit/5
        [HttpPost]
        [Obsolete]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase Image)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                ImageProduct imgprod = DataAdminController.GetImageProductByID(id);
                try
                {
                    if (imgprod == null)
                    {
                        return HttpNotFound();
                    }

                    imgprod.ProductID = Int32.Parse(collection["ProductID"]);
                    if (ModelState.IsValid)
                    {
                        imgprod.Image = CloudinaryController.UploadImage(Image);
                    }
                    var result = DataAdminController.EditImageProduct(imgprod);
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
                catch
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa loại sản phẩm";
                }

                var products = DataAdminController.GetProducts("", "");
                return View(imgprod, products);
            }
            return RedirectToAction("", "Login");
            
        }


        // GET: Admin/ImageProduct/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                ImageProduct img = DataAdminController.GetImageProductByID(id);
                if (img == null)
                {
                    return HttpNotFound();
                }
                Product prod = DataAdminController.GetProductByID(img.ProductID.Value);
                return View(img, prod);
            }
            return RedirectToAction("", "Login");
        }

        // POST: Admin/ImageProduct/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var imgprod = DataAdminController.GetImageProductByID(id);
                try
                {
                    if (imgprod == null)
                    {
                        return HttpNotFound();
                    }

                    var result = DataAdminController.DeleteImageProduct(imgprod);
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
                return View(imgprod);
            }
            return RedirectToAction("", "Login");

        }
    }
}
