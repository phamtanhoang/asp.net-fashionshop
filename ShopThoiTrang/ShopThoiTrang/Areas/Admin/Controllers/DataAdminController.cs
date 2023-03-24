using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        public static bool AddProduct(Product prod, int[] tagIds)
        {
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
            {
                var tags = db.Products.Where(p => p.ProductID == productID)
                                 .SelectMany(p => p.Tags);
                return tags.ToList().AsQueryable();
            }
        }
        //TAG
        public static IQueryable<Tag> GetTags()
        {
            using (var db = new ShopThoiTrangEntities3())
            {
                var query = from t in db.Tags
                            select t;
                return query.ToList().AsQueryable();
            }
        }
        public static Tag GetTagByID(int n)
        {
            using (var db = new ShopThoiTrangEntities3())
            {
                return db.Tags.SingleOrDefault(t => t.TagID == n);
            }
        }
        public static IQueryable<Product> GetProductsByTagID(int tagID)
        {
            using (var db = new ShopThoiTrangEntities3())
            {
                var products = db.Tags.Where(t => t.TagID == tagID)
                                 .SelectMany(t => t.Products);
                return products.ToList().AsQueryable();
            }
        }
        public static bool AddTag(Tag tag, int[] ProductID)
        {
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
            {
                var existingTag = db.Tags.FirstOrDefault(c => c.TagID == tag.TagID);
                if (existingTag == null)
                {
                    return false;
                }

                var duplicateTag = db.Tags.FirstOrDefault(c => c.TagName == tag.TagName);
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
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
            {
                var query = from t in db.ImageProducts
                            select t;
                return query.ToList().AsQueryable();
            }
        }
        public static ImageProduct GetImageProductByID(int n)
        {
            using (var db = new ShopThoiTrangEntities3())
            {
                return db.ImageProducts.SingleOrDefault(p => p.ImageProductID == n);
            }
        }
        public static bool AddImageProduct(ImageProduct imgprod)
        {
            using (var db = new ShopThoiTrangEntities3())
            {
                db.ImageProducts.Add(imgprod);
                db.SaveChanges();
            }
            return true;
        }
        public static bool DeleteImageProduct(ImageProduct imgprod)
        {
            using (var db = new ShopThoiTrangEntities3())
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
            using (var db = new ShopThoiTrangEntities3())
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
    }
}