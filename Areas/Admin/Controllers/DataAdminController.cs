using System;
using System.Collections.Generic;
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
            using (var db = new ShopThoiTrangEntities3())
            {
                var query = from p in db.Categories
                            select p;
                return query.ToList().AsQueryable();
            }
        }
        public static bool AddCategory(Category cate)
        {
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
            {
                var existingCategory = db.Categories.FirstOrDefault(c => c.CategoryID == category.CategoryID);
                if (existingCategory == null)
                {
                    return false; 
                }

                // Kiểm tra xem đã tồn tại bản ghi có CategoryName trùng với đối tượng category truyền vào không
                var duplicateCategory = db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName);
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
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
            {
                return db.Categories.FirstOrDefault(c => c.CategoryID == categoryId);
            }
        }

        //PRODUCT
        public static IQueryable<Product> GetProducts(String tagname, String cate)
        {
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
            {
                return db.Products.SingleOrDefault(p => p.ProductID == n);
            }
        }

    }
}