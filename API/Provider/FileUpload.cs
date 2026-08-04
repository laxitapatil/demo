namespace Api.Provider
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public FileUpload(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        #region Private Methods


        #region Get Directories

        /// <summary>
        /// Get bank directory path.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetBankDirectory() =>
            Path.Combine(_webHostEnvironment.WebRootPath, "upload", "bank");

        private async Task<string> GetUserDirectory() =>
            Path.Combine(_webHostEnvironment.WebRootPath, "upload", "user");


        private bool IsImage(string extension)
        {
            string[] imageTypes = { ".jpg", ".jpeg", ".png", ".webp", ".bmp" };
            return imageTypes.Contains(extension);
        }

        #endregion Get Directories


        /// <summary>
        /// Verify directory, if not exists then create new directory.
        /// </summary>
        /// <param name="folder"></param>
        private async Task<bool> VerifyDirectory(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return true;
        }


        /// <summary>
        /// Upload file to local directory.
        /// </summary>
        /// <param name="folder">folder location</param>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<string> UploadFile(string folder, IFormFile file)
        {
            string fileName = DateTime.UtcNow.Ticks.ToString() + Path.GetExtension(file.FileName).ToLower();
            await file.CopyToAsync(new FileStream(Path.Combine(folder, fileName), FileMode.Create));
            return fileName;
        }

        /// <summary>
        /// Upload converted file to local directory.
        /// </summary>
        /// <param name="folder">folder location</param>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<string> UploadWebPFile(string folder, IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();

            // Case 1: Already WebP → just save with a new unique name
            if (extension == ".webp")
            {
                string finalFileName = DateTime.UtcNow.Ticks.ToString() + ".webp";
                string finalFilePath = Path.Combine(folder, finalFileName);

                using (var stream = new FileStream(finalFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return finalFileName;
            }

            // Case 2: Other formats → convert to WebP
            string tempFilePath = Path.Combine(folder, Guid.NewGuid().ToString() + extension);
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string finalFileNameConv = DateTime.UtcNow.Ticks.ToString() + ".webp";
            string finalFilePathConv = Path.Combine(folder, finalFileNameConv);
            Common.ConvertToWebP(tempFilePath, finalFilePathConv);

            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            return finalFileNameConv;
        }

        public async Task<string> UploadCaseDocument(IFormFile file, int caseId, string bankCode, DateTime caseCreatedDate)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty");

            string extension = Path.GetExtension(file.FileName).ToLower();

            string year = caseCreatedDate.ToString("yyyy");
            string month = caseCreatedDate.ToString("MM");

            string folder = Path.Combine(_webHostEnvironment.WebRootPath, "upload", year, month, bankCode, caseId.ToString());

            await VerifyDirectory(folder);

            string originalName = Path.GetFileNameWithoutExtension(file.FileName);
            originalName = string.Concat(originalName.Split(Path.GetInvalidFileNameChars()));

            string finalFileName;
            string finalPath;

            // IMAGE CASE
            if (IsImage(extension))
            {
                finalFileName = originalName + ".webp";
                finalPath = Path.Combine(folder, finalFileName);

                string tempFile = Path.Combine(folder, Guid.NewGuid() + extension);

                using (var stream = new FileStream(tempFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                Common.ConvertToWebP(tempFile, finalPath);

                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
            else
            {
                // DOCUMENT CASE
                finalFileName = originalName + extension;
                finalPath = Path.Combine(folder, finalFileName);

                using (var stream = new FileStream(finalPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Path.Combine("upload", year, month, bankCode, caseId.ToString(), finalFileName).Replace("\\", "/");
        }
        #endregion Private Methods



        #region Bank

        /// <summary>
        /// Verify bank directory.
        /// </summary>
        /// <returns></returns>
        public async Task VerifyBankDirectory() =>
            await VerifyDirectory(await GetBankDirectory());


        /// <summary>
        /// Upload client image.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UploadBankDocument(IFormFile file)
        {
            var folder = await GetBankDirectory();
            await VerifyDirectory(folder);
            return await UploadFile(folder, file);
        }

        /// <summary>
        /// Delete bank document.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteBankDocument(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var directory = await GetBankDirectory();

            var fullPath = Path.Combine(directory, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }


        #endregion Bank

        #region Case Document
        /// <summary>
        /// Delete case document from case document directory.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task  DeleteCaseDocument(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        #endregion

        #region User
        /// <summary>
        /// Verify User directory.
        /// </summary>
        /// <returns></returns>
        public async Task VerifyUserDirectory() =>
            await VerifyDirectory(await GetUserDirectory());

        /// <summary>
        /// Upload User image.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UploadUserImage(IFormFile file)
        {
            var folder = await GetUserDirectory();
            await VerifyDirectory(folder);
            return await UploadWebPFile(folder, file);
        }

        /// <summary>
        /// Delete User image from user directory.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task DeleteUserImage(string file)
        {
            var path = Path.Combine(await GetUserDirectory(), file);
            if (File.Exists(path))
                File.Delete(path);
        }


        #endregion User
    }
}