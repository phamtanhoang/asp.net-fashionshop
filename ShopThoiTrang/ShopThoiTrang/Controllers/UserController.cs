using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Account()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Register(User cus, FormCollection collect)
        {
            if (collect["PassWord"] != collect["ComfirmPassWord"])
            {
                TempData["TitleMessage"] = "Đăng kí không thành công";
                TempData["ErrorMessage"] = "Mật khẩu không khớp!!!";
            }
            else
            {
                int added = DataController.AddCustomer(cus);
                if (added == 2)
                {
                    TempData["TitleMessage"] = "Đăng kí không thành công";
                    TempData["ErrorMessage"] = "Email " + collect["PhoneNumber"] + " đã tồn tại!!!";
                }
                else if (added == 1)
                {
                    TempData["TitleMessage"] = "Đăng kí không thành công";
                    TempData["ErrorMessage"] = "Số điện thoại " + collect["Email"] + " đã tồn tại!!!";
                }
                else
                {
                    TempData["TitleMessage"] = "Đăng kí thành công";
                    TempData["SuccessMessage"] = "Vui lòng đăng nhập!!!";
                }
            }
            return RedirectToAction("account", "user");
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Login(FormCollection collect)
        {
            User userLogin = DataController.LoginUser(collect["Email"], collect["PassWord"]);
            if (userLogin == null)
            {
                TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu chưa đúng!!!";
                return RedirectToAction("account", "user");
            }
            else
            {
                Session["UserID"] = userLogin.CustomerID;
            }
            return RedirectToAction("index", "home");

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}