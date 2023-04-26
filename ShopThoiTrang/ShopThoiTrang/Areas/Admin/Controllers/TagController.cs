using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class TagController : Controller
    {
        // GET: Admin/Tag
        public ActionResult Index()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var tag = DataAdminController.GetTags();
                return View(tag);
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/Tag/Details/5
        public ActionResult Details(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var tag = DataAdminController.GetTagByID(id);
                IEnumerable<Product> products = DataAdminController.GetProductsByTagID(id);
                return View(tag, products);
            }
            return RedirectToAction("", "Login");
            
        }

        private ActionResult View(Tag tag, IEnumerable<Product> products)
        {
            ViewBag.tag = tag;
            ViewBag.products = products;
            return View();
        }

        // GET: Admin/Tag/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var products = DataAdminController.GetProducts("", "");
                return View(products);
            }
            return RedirectToAction("", "Login");
            
        }

        // POST: Admin/Tag/Create
        [HttpPost]
        public ActionResult Create(Tag tag, int[] ProductID)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                try
                {
                    bool added = DataAdminController.AddTag(tag, ProductID);
                    if (added)
                    {
                        TempData["SuccessMessage"] = "Thêm " + tag.TagName + " thành công";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = tag.TagName + " đã tồn tại";
                    }
                }
                catch
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm nhãn sản phẩm";
                }

                var products = DataAdminController.GetProducts("", "");
                return View(products);
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/Tag/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Tag tag = DataAdminController.GetTagByID(id);
                if (tag == null)
                {
                    return HttpNotFound();
                }
                return View(tag);
            }
            return RedirectToAction("", "Login");
            
        }

        // POST: Admin/Tag/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var tag = DataAdminController.GetTagByID(id);
                try
                {
                    if (tag == null)
                    {
                        return HttpNotFound();
                    }

                    tag.TagName = collection["TagName"];

                    var result = DataAdminController.EditTag(tag);
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
                return View(tag);
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/Tag/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Tag tag = DataAdminController.GetTagByID(id);
                if (tag == null)
                {
                    return HttpNotFound();
                }
                return View(tag);
            }
            return RedirectToAction("", "Login");
            
        }

        // POST: Admin/Tag/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var tag = DataAdminController.GetTagByID(id);
                try
                {
                    if (tag == null)
                    {
                        return HttpNotFound();
                    }

                    var result = DataAdminController.DeleteTag(tag);
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
                return View(tag);
            }
            return RedirectToAction("", "Login");
            
        }
    }
}
