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
            // Lấy danh sách sản phẩm trong giỏ hàng từ Session
            List<CartItem> cart = (List<CartItem>)Session["Cart"];

            // Nếu giỏ hàng chưa được khởi tạo, tạo một giỏ hàng mới
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            // Truyền danh sách sản phẩm trong giỏ hàng cho View
            ViewBag.CartItems = cart;
            ViewBag.Categories = DataController.GetCategories();
            return View();
        }

        public ActionResult AddToCart(int productId)
        {
            // Lấy danh sách sản phẩm trong giỏ hàng từ Session
            List<CartItem> cart = (List<CartItem>)Session["Cart"];

            // Nếu giỏ hàng chưa được khởi tạo, tạo một giỏ hàng mới
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            Product prod = DataController.GetProduct(productId);
            // Tạo một đối tượng CartItem để lưu trữ thông tin sản phẩm mới
            CartItem item = new CartItem
            {
                ProductID = prod.ProductID,
                ProductName = prod.ProductName,
                UnitPrice =  prod.UnitPrice,
                ProductImage = prod.Image,
                Quantity = 1
            };

            // Tìm kiếm sản phẩm trong giỏ hàng để tăng số lượng hoặc thêm sản phẩm mới vào giỏ hàng
            CartItem existingItem = cart.FirstOrDefault(x => x.ProductID == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(item);
            }

            // Lưu danh sách sản phẩm trong giỏ hàng vào Session
            Session["Cart"] = cart;

            // Chuyển hướng đến trang giỏ hàng
            return RedirectToAction("index");
        }

        public ActionResult DeleteItemToCart(int productId)
        {
            // Lấy danh sách sản phẩm trong giỏ hàng từ Session
            List<CartItem> cart = (List<CartItem>)Session["Cart"];

            // Tìm sản phẩm cần xóa
            CartItem itemToRemove = cart.FirstOrDefault(item => item.ProductID == productId);

            // Nếu sản phẩm được tìm thấy, xóa nó ra khỏi danh sách
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
            }

            // Lưu lại danh sách sản phẩm mới vào Session
            Session["Cart"] = cart;

            // Điều hướng người dùng trở lại trang giỏ hàng
            return RedirectToAction("Cart");
        }
    }
}