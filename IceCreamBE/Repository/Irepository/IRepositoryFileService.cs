namespace IceCreamBE.Repository.Irepository
{
    public interface IRepositoryFileService
    {
        public Tuple<int, string, string> SaveImage(IFormFile imageFile, string FolderPath);
        public bool DeleteImage(string imageFileName);
        public bool CheckImage(string filePath, string? FolderPatch = "Other");
    }
}
