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
            var tag = DataAdminController.GetTags();
            return View(tag);
        }

        // GET: Admin/Tag/Details/5
        public ActionResult Details(int id)
        {
            var tag = DataAdminController.GetTagByID(id);
            IEnumerable<Product> products = DataAdminController.GetProductsByTagID(id);
            return View(tag,products);
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
            var products = DataAdminController.GetProducts("","");
            return View(products);
        }

        // POST: Admin/Tag/Create
        [HttpPost]
        public ActionResult Create(Tag tag, int[] ProductID)
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

        // GET: Admin/Tag/Edit/5
        public ActionResult Edit(int id)
        {
            Tag tag = DataAdminController.GetTagByID(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Admin/Tag/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        // GET: Admin/Tag/Delete/5
        public ActionResult Delete(int id)
        {
            Tag tag = DataAdminController.GetTagByID(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Admin/Tag/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
    }
}
