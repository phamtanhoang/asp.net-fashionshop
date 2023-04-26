using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
            
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Index(FormCollection collect)
        {
            User userLogin = ShopThoiTrang.Controllers.DataController.LoginUser(collect["Email"], collect["PassWord"]);
            if (userLogin == null||userLogin.is_Admin==false)
            {
                TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu chưa đúng!!!";
                return View();
            }
            else
            {   
                Session["UserID"] = userLogin.CustomerID;
            }
            return RedirectToAction("","Admin");
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            return RedirectToAction("index", "Admin");
        }
    }
}