using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopThoiTrang.Areas.Admin.Controllers
{
    public class CloudinaryController : Controller
    {
        [HttpPost]
        [Obsolete]
        public static string UploadImage(HttpPostedFileBase Image)
        {
            if (Image != null && Image.ContentLength > 0)
            {
                Account account = new Account("dfzvtpwsd", "717947668599675", "8txPYPx5A5YPpCCfJlTZBANubJg");
                Cloudinary cloudinary = new Cloudinary(account);

                var uploadResult = cloudinary.Upload(new ImageUploadParams()
                {
                    File = new FileDescription(Image.FileName, Image.InputStream),
                    Folder = "product_images"
                });

                return uploadResult.SecureUri.AbsoluteUri;
            }

            return "";
        }
    }
}