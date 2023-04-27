using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class ComplainController : Controller
    {
        // GET: Admin/Complain
        public ActionResult Index()
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var complains = DataAdminController.GetComplains();
                ViewBag.complains = complains;
                return View();
            }
            return RedirectToAction("", "Login");
            
        }

        // GET: Admin/Complain/Details/5
        public ActionResult Details(int id)
        {

            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Complain complain = DataAdminController.GetComplainByID(id);
                if (complain == null)
                    return HttpNotFound();     
                DataAdminController.changeActiveComplain(complain);
                ViewBag.complain = complain;
                return View();
            }
            return RedirectToAction("", "Login");
        }

        
        // GET: Admin/Complain/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                Complain complain = DataAdminController.GetComplainByID(id);
                if (complain == null)
                    return HttpNotFound();
                DataAdminController.changeActiveComplain(complain);
                ViewBag.complain = complain;
                return View();
            }
            return RedirectToAction("", "Login");
        }

        // POST: Admin/Complain/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin == true)
            {
                var complain = DataAdminController.GetComplainByID(id);
                try
                {
                    if (complain == null)
                    {
                        return HttpNotFound();
                    }
                    var result = DataAdminController.DeleteComplain(complain.ComplainID);
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
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa góp ý";
                }
                return View();
            }
            return RedirectToAction("", "Login");
            
        }
    }
}
