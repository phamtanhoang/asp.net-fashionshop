using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class UserAdminController : Controller
    {
        // GET: Admin/UserAdmin
        public ActionResult Index()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var user = DataAdminController.GetUsers();
                return View(user);
            }
            return RedirectToAction("", "Login");
        }

        // GET: Admin/UserAdmin/Details/5
        public ActionResult Details(int id)
        {

            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                User user = ShopThoiTrang.Controllers.DataController.getUserByID(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                ViewBag.user = user;
                return View();
            }
            return RedirectToAction("", "Login");
        }

        // GET: Admin/UserAdmin/Create
        public ActionResult Create()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                return View();
            }
            return RedirectToAction("", "Login");
        }

        // POST: Admin/UserAdmin/Create
        [HttpPost]
        [Obsolete]
        public ActionResult Create(User cus, FormCollection collect)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                try
                {
                    if (collect["PassWord"] != collect["ComfirmPassWord"])
                    {
                        TempData["ErrorMessage"] = "Mật khẩu không khớp!!!";
                    }
                    else
                    {
                        cus.Active = true;
                        int added = ShopThoiTrang.Controllers.DataController.AddCustomer(cus);
                        if (added == 2)
                        {
                            TempData["ErrorMessage"] = "Số điện thoại " + collect["PhoneNumber"] + " đã tồn tại!!!";
                        }
                        else if (added == 1)
                        {
                            TempData["ErrorMessage"] = "Email " + collect["Email"] + " đã tồn tại!!!";
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "Thêm người dùng thành công";
                            return RedirectToAction("Index");
                        }
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

        // GET: Admin/UserAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                User user = ShopThoiTrang.Controllers.DataController.getUserByID(id);
                if (user == null || user.is_Admin==true)
                {
                    return HttpNotFound();
                }
                ViewBag.user = user;
                return View();
            }
            return RedirectToAction("", "Login");
        }

        // POST: Admin/UserAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                User u = DataAdminController.GetUserByID(id);
                try
                {
                    if (u == null)
                    {
                        return HttpNotFound();
                    }

                    var result = DataAdminController.DeleteUser(u);
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
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa người dùng";
                }
                return View();
            }
            return RedirectToAction("", "Login");
            
        }
    }
}
