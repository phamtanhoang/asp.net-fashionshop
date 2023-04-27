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
            decimal temp = 0;
            foreach (CartItem item in cart)
            {
                temp += item.Quantity * item.UnitPrice;
            }
            

            decimal total = temp + 50000;

            ViewBag.temp = temp;
            ViewBag.total = total;
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

        public ActionResult UpdateCart(int productId, int quantity)
        {
            // Lấy danh sách sản phẩm trong giỏ hàng từ Session
            List<CartItem> cart = (List<CartItem>)Session["Cart"];

            // Tìm sản phẩm cần cập nhật số lượng
            CartItem itemToUpdate = cart.FirstOrDefault(item => item.ProductID == productId);

            // Nếu sản phẩm được tìm thấy, cập nhật số lượng mới
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;
            }

            // Lưu lại danh sách sản phẩm mới vào Session
            Session["Cart"] = cart;

            // Điều hướng người dùng trở lại trang giỏ hàng
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAllCart()
        {
            List<CartItem> cart = (List<CartItem>)Session["Cart"];

            
            Session["Cart"] = null;

            return RedirectToAction("Cart");
        }

        public ActionResult Order(string address)
        {
            if (Session["UserID"] != null)
            {
                List<CartItem> cart = (List<CartItem>)Session["Cart"];

                if (cart == null)
                {
                    return View();
                }
                decimal temp = 0;
                foreach (CartItem item in cart)
                {
                    temp += item.Quantity * item.UnitPrice;
                }

                Order order = new Order
                {
                    CustomerID = (int)Session["UserID"],
                    DeliveryAddress = address,
                    Temp = temp,
                    Ship = 50000,
                    OrderDate = DateTime.Now
                };
                if (DataController.AddOrder(order))
                {
                    foreach (CartItem item in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail
                        {
                            OrderID = order.OrderID,
                            ProductID = item.ProductID,
                            Quantity = (short)item.Quantity,
                            UnitPrice = item.UnitPrice
                        };
                        DataController.AddOrderDetails(orderDetail);
                    }
                    Session["Cart"] = null;
                    return RedirectToAction("Cart");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return HttpNotFound();
        }
    }
}