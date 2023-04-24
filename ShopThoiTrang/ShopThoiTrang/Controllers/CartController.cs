using Newtonsoft.Json;
using ShopThoiTrang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart
        public ActionResult Index()
        {
            // Lấy danh sách các cart items từ Session Storage
            var cartItemsJson = Session["CartItems"] as string;
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);
            Console.WriteLine(cartItemsJson);
            return View(cartItems);

        }
    }
}