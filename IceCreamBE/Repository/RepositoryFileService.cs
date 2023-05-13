using Microsoft.AspNetCore.Hosting;
using IceCreamBE.Repository.Irepository;

namespace IceCreamBE.Repository
{
    public class RepositoryFileService : IRepositoryFileService
    {
        private IWebHostEnvironment environment;
        public RepositoryFileService(IWebHostEnvironment env)
        {
            this.environment = env;
        }

        public Tuple<int, string, string> SaveImage(IFormFile imageFile, string? FolderPatch = "Other")
        {
            try
            {
                var contentPath = this.environment.ContentRootPath;
                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Assets\\"+FolderPatch); 
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string, string>(0, "", msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                // we are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string, string>(1, $"Assets\\{FolderPatch}\\{newFileName}", newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string, string>(0, "","Error has occured");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Assets\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
