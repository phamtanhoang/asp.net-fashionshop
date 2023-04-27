using Newtonsoft.Json;
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
        public ActionResult Index(string lang)
        {
            if (Session["UserID"] != null && DataAdminController.GetUserByID((int)Session["UserID"]).is_Admin==true)
            {
                using (var db = new ShopThoiTrangEntities())
                {
                    
                    if (string.IsNullOrEmpty(lang))
                    {
                        lang = DateTime.Now.Year.ToString();
                        
                    }
                    int langInt = int.Parse(lang);
                    var reports = DataAdminController.GetReports(langInt);
                    var cateReports = DataAdminController.GetCategoryReports(langInt);
                    var chartData = new
                    {
                        labels = reports.Select(x => x.Month).ToArray(),

                        datasets = new[]
                        {
                            new
                            {
                                label = "Tổng tiền của tháng",
                                data = reports.Select(x => x.Sum).ToArray(),
                                backgroundColor = "#3e95cd",

                            }
                        }
                    };
                    var chartData2 = new
                    {
                        labels = cateReports.Select(x => x.CategoryName).ToArray(),
                        datasets = new[]
                        {
                            new
                            {
                                data = cateReports.Select(x => x.TotalSalesAmount).ToArray(),
                                backgroundColor = new [] { "#4e73df", "#36b9cc" },

                                hoverBackgroundColor = new[] { "#2e59d9", "#17a673", "#2c9faf" },
                                hoverBorderColor= new [] { "rgba(234, 236, 244, 1)" },

                            }
                        }
                    };
                    TempData["CountOrder"] = DataAdminController.GetTotalOrderByYear(langInt).ToString();
                    TempData["CountOrderNoneActive"]  = DataAdminController.GetTotalInactiveOrderByYear(langInt).ToString();
                    TempData["SumOrder"]  = DataAdminController.GetSumOrderByYear(langInt).ToString("#,## VNĐ");
                    ViewBag.ChartData = JsonConvert.SerializeObject(chartData);
                    ViewBag.ChartData2 = JsonConvert.SerializeObject(chartData2);
                    return View();
                }
            }
            return RedirectToAction("", "Login");      
        }
    }
}
