using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using TopPost.Data;

namespace TopPost.ImageUpload
{
    public static class ImageUploader
    {
        public static string[] UrlFromFile(HttpPostedFileBase file)
        {
            TopPostData db = TopPostData.Create();

            string projectPath = "C:/Users/Ghos7/Documents/GitHub/TopPost/TopPost/TopPost.MVC";
            string fileName = (db.Posts.All().Count() + 1).ToString();
            string extension = Path.GetExtension(file.FileName);
            string imageUrl = "/images/" + fileName + extension;
            string thumbUrl = "/images/thumbnails/" + fileName + ".jpg";
            string physicalPathImage = projectPath + imageUrl;
            string physicalPathThumbnail = projectPath + thumbUrl;

            Image image = Image.FromStream(file.InputStream);
            Image thumb = image.GetThumbnailImage(180, 180, () => false, IntPtr.Zero);
            thumb.Save(physicalPathThumbnail, System.Drawing.Imaging.ImageFormat.Jpeg);
            file.SaveAs(physicalPathImage);            

            return new string[] { imageUrl, thumbUrl };
        }

        //public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //    return ms.ToArray();
        //}

        //public static Image byteArrayToImage(byte[] byteArrayIn)
        //{
        //    MemoryStream ms = new MemoryStream(byteArrayIn);
        //    Image returnImage = Image.FromStream(ms);
        //    return returnImage;
        //}
    }
}
