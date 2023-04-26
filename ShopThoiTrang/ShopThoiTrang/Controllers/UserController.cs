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
            if (Session["UserID"] != null)
            {
                User u = DataController.getUserByID((int)Session["UserID"]);
                ViewBag.FullName = u.FullName;
                ViewBag.PhoneNumber = u.PhoneNumber;
                ViewBag.Email = u.Email;
                ViewBag.PassWord = u.PassWord;
                return View();
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ChangeInfomation(FormCollection form)
        {
            if (Session["UserID"] != null)
            {
                int useriD = (int)Session["UserID"];
                string fullname = form["FullName"];
                string phoneNumber = form["PhoneNumber"];
                string email = form["email"];
                int change = DataController.ChangeInfoUser(useriD, fullname, phoneNumber, email);
                if (change==0)
                {
                    TempData["ErrorMessage"] = "Sửa thông tin người dùng không thành công!!!";
                    return RedirectToAction("index", "user");
                    
                }
                else if (change==2)
                {
                    TempData["ErrorMessage"] = "Email "+ email +" đã tồn tại!!!";
                    return RedirectToAction("index", "user");
                }
                else if (change == 3)
                {
                    TempData["ErrorMessage"] = "Số điện thoại " + phoneNumber + " đã tồn tại!!!";
                    return RedirectToAction("index", "user");
                }
                else
                {
                    TempData["SuccessMessage"] = "Sửa thông tin người dùng thành công.";
                    return RedirectToAction("index", "user");
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Obsolete]
        public ActionResult ChangePassWord(FormCollection form)
        {
            if (Session["UserID"] != null)
            {
                int useriD = (int)Session["UserID"];
                string pw = form["PassWord"];
                string npw = form["NewPassWord"];
                string cnpw = form["ComfirmNewPassWord"];
                int change = DataController.ChangePassWordUser(useriD, pw, npw, cnpw);
                if(change==0)
                {
                    TempData["ErrorMessage"] = "Đổi mật khẩu không thành công!!!";
                    return RedirectToAction("index", "user");
                }
                if(change==1)
                {
                    TempData["ErrorMessage"] = "Mật khẩu không đúng!!!";
                    return RedirectToAction("index", "user");
                }
                if (change == 2)
                {
                    TempData["ErrorMessage"] = "Mật khẩu mới không khớp!!!";
                    return RedirectToAction("index", "user");
                }
                if (change == 3)
                {
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công.";
                    return RedirectToAction("index", "user");
                }
            }
            return HttpNotFound();
        }

        public ActionResult DeleteUserAccount()
        {

            if (Session["UserID"] != null && ShopThoiTrang.Areas.Admin.Controllers.DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin != true)
            {
                int useriD = (int)Session["UserID"];
                if (DataController.DeleteUserAccount(useriD))
                {
                    Session["UserID"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Xóa Tài khoản không thành công !!!";
                    return RedirectToAction("index", "user");
                }
            }
            return HttpNotFound();
        }

        public ActionResult Account()
        {
            if (Session["UserID"] == null)
            {
                return View();
            }
            return HttpNotFound();
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
                cus.Active = true;
                int added = DataController.AddCustomer(cus);
                if (added == 2)
                {
                    TempData["TitleMessage"] = "Đăng kí không thành công";
                    TempData["ErrorMessage"] = "Số điện thoại " + collect["PhoneNumber"] + " đã tồn tại!!!";
                }
                else if (added == 1)
                {
                    TempData["TitleMessage"] = "Đăng kí không thành công";
                    TempData["ErrorMessage"] = "Email " + collect["Email"] + " đã tồn tại!!!";
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
                TempData["TitleMessage"] = "Đăng nhập không thành công";
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
            Session["UserID"]=null;
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}