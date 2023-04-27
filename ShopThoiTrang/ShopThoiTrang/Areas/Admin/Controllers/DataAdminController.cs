using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ShopThoiTrang.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class DataAdminController : Controller
    {

        //CATEGORY
        public static IQueryable<Category> GetCategories()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from p in db.Categories
                            select p;
                return query.ToList().AsQueryable();
            }
        }
        public static bool AddCategory(Category cate)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                if (db.Categories.Any(c => c.CategoryName == cate.CategoryName))
                {
                    return false;
                }
                db.Categories.Add(cate);
                db.SaveChanges();
            }
            return true;
        }
        public static bool EditCategory(Category category)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingCategory = db.Categories.FirstOrDefault(c => c.CategoryID == category.CategoryID);
                if (existingCategory == null)
                {
                    return false; 
                }

                // Kiểm tra xem đã tồn tại bản ghi có CategoryName trùng với đối tượng category truyền vào không
                var duplicateCategory = db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName && c.CategoryID != category.CategoryID);
                if (duplicateCategory != null)
                {
                    return false; // Đã tồn tại bản ghi khác có CategoryName giống với đối tượng category truyền vào
                }

                // Cập nhật thông tin của bản ghi có CategoryID trùng với đối tượng category truyền vào
                existingCategory.CategoryName = category.CategoryName;

                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
            return true;
        }
        public static bool DeleteCategory(Category category)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingCategory = db.Categories.FirstOrDefault(c => c.CategoryID == category.CategoryID);
                if (existingCategory == null)
                {
                    return false; // Không tìm thấy bản ghi cần xóa
                }

                db.Categories.Remove(existingCategory);
                db.SaveChanges();
            }
            return true;
        }
        public static Category GetCategoryByID(int categoryId)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Categories.FirstOrDefault(c => c.CategoryID == categoryId);
            }
        }

        //PRODUCT
        public static IQueryable<Product> GetProducts(String tagname, String cate)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = db.Products.AsQueryable();
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
        public static Product GetProductByID(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Products.SingleOrDefault(p => p.ProductID == n);
            }
        }
        public static bool AddProduct(Product prod, int[] tagIds)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                if (db.Products.Any(p => p.ProductName == prod.ProductName))
                {
                    return false;
                }
                db.Products.Add(prod);

                foreach (int tagId in tagIds)
                {
                    Tag tag = db.Tags.Find(tagId);
                    if (tag != null)
                    {
                        prod.Tags.Add(tag);
                    }
                }
                db.SaveChanges();
            }
            return true;
        }
        public static bool DeleteProduct(Product prod)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingProduct = db.Products.FirstOrDefault(p => p.ProductID == prod.ProductID);
                if (existingProduct == null)
                {
                    return false; // Không tìm thấy bản ghi cần xóa
                }
                existingProduct.Tags.Clear();
                db.Products.Remove(existingProduct);
                db.SaveChanges();
            }
            return true;
        }
        public static bool EditProduct(Product prod, int[] tagID)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingProd = db.Products.FirstOrDefault(p => p.ProductID == prod.ProductID);
                if (existingProd == null)
                {
                    return false;
                }

                //var duplicateProd = db.Products.FirstOrDefault(p => p.ProductName == prod.ProductName);
                var duplicateProd = db.Products.FirstOrDefault(p => (p.ProductName == prod.ProductName && p.ProductID != prod.ProductID));
                if (duplicateProd != null)
                {
                    return false; 
                }
                existingProd.ProductName = prod.ProductName;
                existingProd.UnitPrice = prod.UnitPrice;
                existingProd.Description = prod.Description;
                existingProd.CategoryID = prod.CategoryID;
                existingProd.active = prod.active;
                if (prod.Image != "")
                {
                    existingProd.Image = prod.Image;
                }

                existingProd.Tags.Clear();
                foreach (int tagId in tagID)
                {
                    Tag tag = db.Tags.Find(tagId);
                    if (tag != null)
                    {
                        existingProd.Tags.Add(tag);
                    }
                }
                db.SaveChanges(); 
            }
            return true;
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
        //TAG
        public static IQueryable<Tag> GetTags()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from t in db.Tags
                            select t;
                return query.ToList().AsQueryable();
            }
        }
        public static Tag GetTagByID(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Tags.SingleOrDefault(t => t.TagID == n);
            }
        }
        public static IQueryable<Product> GetProductsByTagID(int tagID)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var products = db.Tags.Where(t => t.TagID == tagID)
                                 .SelectMany(t => t.Products);
                return products.ToList().AsQueryable();
            }
        }
        public static bool AddTag(Tag tag, int[] ProductID)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                if (db.Tags.Any(c => c.TagName == tag.TagName && c.TagID != tag.TagID))
                {
                    return false;
                }

                db.Tags.Add(tag);
                foreach (int prodId in ProductID)
                {
                    Product product = db.Products.Find(prodId);
                    if (product != null)
                    {
                        tag.Products.Add(product);
                    }
                }

                db.SaveChanges();
            }
            return true;
        }
        public static bool EditTag(Tag tag)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingTag = db.Tags.FirstOrDefault(c => c.TagID == tag.TagID);
                if (existingTag == null)
                {
                    return false;
                }

                var duplicateTag = db.Tags.FirstOrDefault(c => c.TagName == tag.TagName && c.TagID != tag.TagID);
                if (duplicateTag != null)
                {
                    return false;
                }

                existingTag.TagName = tag.TagName;

                db.SaveChanges(); 
            }
            return true;
        }
        public static bool DeleteTag(Tag tag)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingTag = db.Tags.FirstOrDefault(c => c.TagID == tag.TagID);
                if (existingTag == null)
                {
                    return false; 
                }

                existingTag.Products.Clear();

                db.Tags.Remove(existingTag);

                db.SaveChanges();
            }
            return true;
        }

        //IMAGE-PRODUCTS
        public static IQueryable<ImageProduct> GetImageProducts()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from t in db.ImageProducts
                            select t;
                return query.ToList().AsQueryable();
            }
        }
        public static ImageProduct GetImageProductByID(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.ImageProducts.SingleOrDefault(p => p.ImageProductID == n);
            }
        }
        public static bool AddImageProduct(ImageProduct imgprod)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                db.ImageProducts.Add(imgprod);
                db.SaveChanges();
            }
            return true;
        }
        public static bool DeleteImageProduct(ImageProduct imgprod)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingImageProduct = db.ImageProducts.FirstOrDefault(c => c.ImageProductID == imgprod.ImageProductID);
                if (existingImageProduct == null)
                {
                    return false; // Không tìm thấy bản ghi cần xóa
                }

                db.ImageProducts.Remove(existingImageProduct);
                db.SaveChanges();
            }
            return true;
        }
        public static bool EditImageProduct(ImageProduct imgprod)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingImageProduct = db.ImageProducts.FirstOrDefault(c => c.ImageProductID == imgprod.ImageProductID);
                if (existingImageProduct == null)
                {
                    return false; 
                }

                existingImageProduct.ProductID = imgprod.ProductID;
                if (imgprod.Image != "")
                {
                    existingImageProduct.Image = imgprod.Image;
                }

                db.SaveChanges(); 
            }
            return true;
        }

        //USERS
        public static IQueryable<User> GetUsers()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from p in db.Users
                            select p;
                return query.ToList().AsQueryable();
            }
        }
        public static User GetUserByID(int n)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Users.SingleOrDefault(p => p.CustomerID == n);
            }
        }
        public static bool DeleteUser(User u)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingU = db.Users.FirstOrDefault(c => c.CustomerID== u.CustomerID);
                if (existingU == null)
                {
                    return false; // Không tìm thấy bản ghi cần xóa
                }

                db.Users.Remove(existingU);
                db.SaveChanges();
            }
            return true;
        }

        //ORDER
        public static IQueryable<Order> GetOrders()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from p in db.Orders
                            select p;
                return query.ToList().AsQueryable();
            }
        }
        public static Order GetOrderByID(int id)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Orders.SingleOrDefault(p => p.OrderID == id);
            }
        }

        public static IQueryable<OrderDetail> GetOrderDetails(int id)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = db.OrderDetails.AsQueryable();
                if (id>0)
                {
                    query = query.Where(p => p.OrderID == id);
                }
                return query.ToList().AsQueryable();
            }
        }
        public static bool DeleteOrder(int id)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingOrder = db.Orders.FirstOrDefault(c => c.OrderID == id);
                if (existingOrder == null)
                {
                    return false; // Không tìm thấy bản ghi cần xóa
                }

                existingOrder.OrderDetails.Clear();
                db.Orders.Remove(existingOrder);
                db.SaveChanges();
            }
            return true;
        }
        public static bool ChangeOrder(int id)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existingOrder = db.Orders.FirstOrDefault(c => c.OrderID == id);
                if (existingOrder == null)
                    return false;
                if (existingOrder.Active == true)
                    existingOrder.Active = false;
                else
                    existingOrder.Active = true;
                db.SaveChanges();
            }
            return true;
        }

        //complain
        public static IQueryable<Complain> GetComplains()
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = from p in db.Complains select p;
                return query.ToList().AsQueryable();
            }
        }
        public static Complain GetComplainByID(int id)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Complains.SingleOrDefault(p => p.ComplainID == id);
            }
        }

        public static bool changeActiveComplain(Complain com)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existing = db.Complains.FirstOrDefault(c => c.ComplainID == com.ComplainID);
                if (existing == null)
                    return false;
                existing.Active = true;
                db.SaveChanges();
            }
            return true;
        }
        public static bool DeleteComplain(int id)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var existing = db.Complains.FirstOrDefault(c => c.ComplainID == id);
                if (existing == null)
                {
                    return false; // Không tìm thấy bản ghi cần xóa
                }
                db.Complains.Remove(existing);
                db.SaveChanges();
            }
            return true;
        }

        public static List<Report> GetReports(int year)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var query = db.Orders.Where(o => o.OrderDate.Year == year)
                                     .GroupBy(o => o.OrderDate.Month)
                                     .Select(g => new Report
                                     {
                                         Month = g.Key,
                                         Sum = g.Sum(o => o.Temp + o.Ship)
                                     })
                                     .OrderBy(r => r.Month);

                return query.ToList();
            }
        }

        public static List<ReportCate> GetCategoryReports(int year)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                var data = db.Orders
                .Where(o => o.OrderDate.Year == year)
                .Join(db.OrderDetails, o => o.OrderID, od => od.OrderID, (o, od) => new { Order = o, OrderDetail = od })
                .Join(db.Products, od => od.OrderDetail.ProductID, p => p.ProductID, (od, p) => new { od.Order, od.OrderDetail, Product = p })
                .Join(db.Categories, p => p.Product.CategoryID, c => c.CategoryID, (p, c) => new { c.CategoryName, TotalSalesAmount = p.OrderDetail.Quantity * p.OrderDetail.UnitPrice })
                .GroupBy(x => x.CategoryName)
                .Select(g => new ReportCate { CategoryName = g.Key, TotalSalesAmount = g.Sum(x => x.TotalSalesAmount) })
                .ToList();
                return data;
            }
        }

        public static int GetTotalOrderByYear(int year)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Orders.Where(o => o.OrderDate.Year == year).Count();
            }
        }
        public static int GetTotalInactiveOrderByYear(int year)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return db.Orders.Where(o => o.OrderDate.Year == year && o.Active == false).Count();
            }
        }
        public static float GetSumOrderByYear(int year)
        {
            using (var db = new ShopThoiTrangEntities())
            {
                return (float)db.Orders.Where(o => o.OrderDate.Year == year && o.Active == false).Sum(o => (o.Temp+o.Ship));
            }
        }
    }
}
