using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopThoiTrang.Controllers
{
    public class DataController : Controller
    {

        public static IQueryable<Category> GetCategories()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from p in db.Categories
                            select p;
                return query.ToList().AsQueryable();
            }
        }

        public static Category GetCategoryByID(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Categories.SingleOrDefault(p => p.CategoryID == n);
            }
        }

        public static IQueryable<Product> GetProducts(String tagname, String cate)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = db.Products.AsQueryable();
                query = query.Where(p => p.active == true); // Chỉ lấy các sản phẩm có trường active bằng true
                if (!string.IsNullOrEmpty(tagname))
                {
                    query = query.Where(p => p.Tags.Any(t => t.TagName == tagname));
                }
                if (!string.IsNullOrEmpty(cate))
                {
                    query = query.Where(p => p.Category.CategoryName.Contains(cate));
                }
                return query.ToList().AsQueryable();
            }
        }

        public static IQueryable<Tag> GetTags()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from p in db.Tags
                            select p;

                return query.ToList().AsQueryable();
            }
        }
        public static IQueryable<Product> RandomProduct(IQueryable<Product> product, int n)
        {
            return product.OrderBy(x => Guid.NewGuid()).Take(n);
        }
        public static Product GetProduct(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Products.SingleOrDefault(p => p.ProductID == n);
            }
        }
        public static IQueryable<ImageProduct> Image(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = db.ImageProducts.AsQueryable();
                query = query.Where(p => p.Product.ProductID == n);
                return query.ToList().AsQueryable();
            }

        }
        public static IQueryable<Tag> GetTagsByProductID(int productID)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var tags = db.Products.Where(p => p.ProductID == productID)
                                 .SelectMany(p => p.Tags);
                return tags.ToList().AsQueryable();
            }
        }

        //user
        [Obsolete]
        public static int AddCustomer(User cus)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                if (db.Users.Any(c => c.Email == cus.Email))
                {
                    return 1;
                }
                if (db.Users.Any(c => c.PhoneNumber == cus.PhoneNumber))
                {
                    return 2;
                }
                cus.PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(cus.PassWord, "SHA1");
                db.Users.Add(cus);
                db.SaveChanges();
            }
            return 0;
        }

        [Obsolete]
        public static User LoginUser(string email, string passWord)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                string pw = FormsAuthentication.HashPasswordForStoringInConfigFile(passWord, "SHA1");
                User user = db.Users.FirstOrDefault(c => c.Email == email && c.PassWord == pw);
                return user;
            }
        }

        public static User getUserByID(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Users.SingleOrDefault(p => p.CustomerID == n);
            }
        }
        public static int ChangeInfoUser(int userID, string fullName, string phoneNumber, string email)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                User user = db.Users.SingleOrDefault(u => u.CustomerID == userID);
                if (user == null)
                    return 0;
                user.FullName = fullName;
                user.PhoneNumber = phoneNumber;
                user.Email = email;
                if (db.Users.Any(c => c.Email == user.Email && c.CustomerID != user.CustomerID))
                    return 2;
                if (db.Users.Any(c => c.PhoneNumber == user.PhoneNumber && c.CustomerID != user.CustomerID))
                    return 3;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return 0;
                }
                return 1;
            }
        }

        [Obsolete]
        public static int ChangePassWordUser(int userID, string pw, string npw, string cnpw)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                User user = db.Users.SingleOrDefault(u => u.CustomerID == userID);
                if (user == null)
                    return 0;
                string p = FormsAuthentication.HashPasswordForStoringInConfigFile(pw, "SHA1");
                if (user.PassWord != p)
                    return 1;
                if (npw != cnpw)
                    return 2;
                string np = FormsAuthentication.HashPasswordForStoringInConfigFile(npw, "SHA1");
                user.PassWord = np;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return 0;
                }
                return 3;
            }
        }

        public static bool DeleteUserAccount(int userID)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                User user = db.Users.SingleOrDefault(u => u.CustomerID == userID);
                if (user == null)
                    return false;
                db.Users.Remove(user);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
        }

        //order
        public static bool AddOrder(Order order)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return true;
            }
        }

        public static bool AddOrderDetails(OrderDetail order)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                db.OrderDetails.Add(order);
                db.SaveChanges();
                return true;
            }
        }


        //complain
        public static bool AddComplain(Complain com)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                db.Complains.Add(com);
                db.SaveChanges();
                return true;
            }
        }
    }
}