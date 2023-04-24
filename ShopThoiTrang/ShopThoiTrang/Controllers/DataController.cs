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
                    query = query.Where(p => p.Category.CategoryName == cate);
                }
                return query.ToList().AsQueryable();
            }
        }
        public static IQueryable<Tag> GetTags()
        {
            using (var db= new ShopThoiTrangEntities())
            {
                var query = from p in db.Tags
                            select p;

                return query.ToList().AsQueryable();
            }
        }
        public static IQueryable<Product> RandomProduct(IQueryable<Product> product,int n)
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
    }
}