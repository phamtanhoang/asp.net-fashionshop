    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("", "Login");      
        }
    }
}
