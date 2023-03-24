using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            var cate = DataAdminController.GetCategories();
            return View(cate);
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int id)
        {
            Category category = DataAdminController.GetCategoryByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            IQueryable<Product> products = DataAdminController.GetProducts("", category.CategoryName);
            return View(category, products);
        }

        private ActionResult View(Category category, IQueryable<Product> products)
        {
            ViewBag.category = category;
            ViewBag.products = products;
            return View();
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        public ActionResult Create(Category cate)
        {
            try
            {
                bool added = DataAdminController.AddCategory(cate);
                if (added)
                {
                    TempData["SuccessMessage"] = "Thêm " + cate.CategoryName + " thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = cate.CategoryName + " đã tồn tại";
                    return View();
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm loại sản phẩm";
            }

            return View();
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = DataAdminController.GetCategoryByID(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var category = DataAdminController.GetCategoryByID(id);
            try
            {
                if (category == null)
                {
                    return HttpNotFound();
                }

                category.CategoryName = collection["CategoryName"];

                var result = DataAdminController.EditCategory(category);
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
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int id)
        {
            Category category = DataAdminController.GetCategoryByID(id);
            if (category == null)
            {
                return HttpNotFound(); 
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var category = DataAdminController.GetCategoryByID(id);
            try
            {
                if (category == null)
                {
                    return HttpNotFound();
                }

                var result = DataAdminController.DeleteCategory(category);
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
            return View(category);
        }
    }
}
