namespace PharmactMangmentEditeIdea.HelperImage
{
    public static class DecumentSettings
    {
        // contains tow methods
        // 1- upload image
        public static string UploadImage(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file.");

            // 1 - Build the full folder path
            var fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

            // 2 - Ensure the folder exists
            if (!Directory.Exists(fullFolderPath))
                Directory.CreateDirectory(fullFolderPath);

            // 3 - Generate a unique file name
            var fileName = GenerateName(file); // Assume GenerateName handles extension safely

            // 4 - Combine to full image path
            var fullImagePath = Path.Combine(fullFolderPath, fileName);

            // 5 - Save the file
            using var fileStream = new FileStream(fullImagePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }


        // 2- delete image
        public static void DeleteImage(string flodername, string imageName)
        {
            // 1- get folder path
            var FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", flodername);
            // 2- اجمع المسار الكامل للصورة
            var FullPathImage = Path.Combine(FullPath, imageName);
            // 3- delete image
            if (File.Exists(FullPathImage))
            {
                File.Delete(FullPathImage);
            }

        }

        private static string GenerateName(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"{timestamp}.{fileExtension}";
        }

    }
}
