using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var order = DataAdminController.GetOrders();
                var user = DataAdminController.GetUsers();
                ViewBag.user = user;
                ViewBag.order = order;
                return View();
            }
            return RedirectToAction("", "Login");
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Order order = DataAdminController.GetOrderByID(id);       
                ViewBag.order = order;
                return View();
            }
            return RedirectToAction("", "Login");
        }

        [HttpGet]
        public ActionResult Change(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                try
                {
                    var order = DataAdminController.GetOrderByID(id);
                    try
                    {
                        if (order == null)
                        {
                            return HttpNotFound();
                        }

                        var result = DataAdminController.ChangeOrder(id);
                        if (result)
                        {
                            TempData["SuccessMessage"] = "Thay đổi tình trang thành công";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Thay đổi tình trang không thành công";
                        }
                    }
                    catch
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra";
                    }
                }
                catch
                {

                }
                return RedirectToAction("Details", "Order", new { id = id }); 
            }
            return RedirectToAction("", "Login");
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Order order = DataAdminController.GetOrderByID(id);
                ViewBag.order = order;
                return View();
            }
            return RedirectToAction("", "Login");
        }

        
        // POST: Admin/Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                try
                {
                    var order = DataAdminController.GetOrderByID(id);
                    try
                    {
                        if (order == null)
                        {
                            return HttpNotFound();
                        }

                        var result = DataAdminController.DeleteOrder(id);
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
                    return View();
                }
                catch
                {
                    return View();
                }
            }
            return RedirectToAction("", "Login");
        }
    }
}
