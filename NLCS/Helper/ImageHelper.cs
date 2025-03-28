using System;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
namespace NLCS.Helper
{
    public static class ImageHelper
    {
        public const string ImageDirectory = "wwwroot/Assets/images/"; //đường dẫn thư mục lưu ảnh
        public static async Task<string> SaveImageAsync(IFormFile image, IWebHostEnvironment env)
        {
            try
            {
                if(image == null || image.Length==0)
                {
                    throw new ArgumentException("File ảnh rỗng!");
                }

                string fileName = GenerateUniqueFileName(); // tạo một tên tệp tin duy nhất
                string imageFolderPath = Path.Combine(env.ContentRootPath, ImageDirectory);// lấy đường dẫn tuyệt đối của thư mục lưu ảnh.
                Directory.CreateDirectory(imageFolderPath);
                string fullPath = Path.Combine(imageFolderPath, fileName); // kết hợp đường dẫn thư mục với tên tệp tin.
                using (var stream = new FileStream(fullPath,FileMode.Create))
                {
                    await image.CopyToAsync(stream);// lưu ảnh vào thư mục

                }
                // trả về đường dẫn tương đối của tập tin ảnh đã lưu.
                return $"/Assets/images/{fileName}";
            }
            catch(Exception ex) 
            {
                    throw new Exception("Error saving image:" + ex.Message);
            }
        }
        private static string GenerateUniqueFileName()
        {
            return $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}.jpg"; // Tên file: yyyyMMddHHmmssfff_GUID.jpg
        }

        public static async Task DeleteImageAsync(string imagePath, IWebHostEnvironment env)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    throw new ArgumentException("Đường dẫn ảnh không hợp lệ!");
                }

                // Lấy đường dẫn tuyệt đối của ảnh
                string fileName = Path.GetFileName(imagePath);
                string imageFolderPath = Path.Combine(env.ContentRootPath, ImageDirectory);
                string fullPath = Path.Combine(imageFolderPath, fileName);

                // Kiểm tra xem tệp tin có tồn tại không
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath); // Xóa tệp tin
                }
                else
                {
                    throw new FileNotFoundException("Tệp tin không tồn tại!", fullPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting image: " + ex.Message);
            }
        }

    }

}