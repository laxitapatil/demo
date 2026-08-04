using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Webp;

namespace Api.Provider
{
    public class Common
    {
        private readonly IConfiguration _configuration;
        private readonly ViewRender _viewRender;

        public Common(IConfiguration configuration, ViewRender viewRender)
        {
            _configuration = configuration;
            _viewRender = viewRender;
        }

        public enum SizeOption
        {
            GenerateWidth,
            GenerateHeight
        }
        private struct Size
        {
            public float Width { get; }
            public float Height { get; }

            public Size(float width, float height)
            {
                Width = width;
                Height = height;
            }
        }

        public static void makeThumbnail(string fsSourceImage, string fsDestinationImage, int fiWidth, int fiHeight, bool flgKeepOriginal, SizeOption foSizeOption)
        {
            try
            {
                //fsSourceImage = fsSourceImage.ToLower();
                FileInfo loFileInfo = new FileInfo(fsSourceImage);
                if (loFileInfo.Exists)
                {
                    using (Image image = Image.Load(fsSourceImage))
                    {
                        int sourceWidth = image.Width;
                        int sourceHeight = image.Height;

                        var newSize = CalculateNewSize(sourceWidth, sourceHeight, fiWidth, fiHeight, foSizeOption);

                        using (Image resizedImage = image.Clone(ctx => ctx.Resize((int)newSize.Width, (int)newSize.Height)))
                        {
                            IImageFormat format = GetImageFormat(loFileInfo.Extension);
                            using (FileStream output = new FileStream(fsDestinationImage, FileMode.Create))
                            {
                                resizedImage.Save(output, format);
                            }
                        }
                    }

                    if (!flgKeepOriginal)
                    {
                        if (File.Exists(fsSourceImage))
                            File.Delete(fsSourceImage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception or log error here
                GC.Collect();
            }
        }

        private static Size CalculateNewSize(int sourceWidth, int sourceHeight, int maxWidth, int maxHeight, SizeOption option)
        {
            float newWidth = 0;
            float newHeight = 0;

            if (option == SizeOption.GenerateWidth)
            {
                if (sourceHeight <= maxHeight)
                    newHeight = sourceHeight;
                else
                    newHeight = maxHeight;

                newWidth = (newHeight / sourceHeight) * sourceWidth;

                if (newWidth > maxWidth)
                {
                    newWidth = maxWidth;
                    newHeight = (newWidth / sourceWidth) * sourceHeight;
                }
            }
            else
            {
                if (sourceWidth <= maxWidth)
                    newWidth = sourceWidth;
                else
                    newWidth = maxWidth;

                newHeight = (newWidth / sourceWidth) * sourceHeight;

                if (newHeight > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = (newHeight / sourceHeight) * sourceWidth;
                }
            }

            return new Size(newWidth, newHeight);
        }

        public static string ConvertToWebP(string sourceImagePath, string? destinationPath = null, int quality = 85)
        {
            try
            {
                FileInfo sourceInfo = new FileInfo(sourceImagePath);
                if (!sourceInfo.Exists)
                    throw new FileNotFoundException("Source image not found");

                string webpPath = destinationPath ?? Path.ChangeExtension(sourceImagePath, ".webp");

                using (Image image = Image.Load(sourceImagePath))
                {
                    using (FileStream output = new FileStream(webpPath, FileMode.Create))
                    {
                        image.Save(output, new WebpEncoder()
                        {
                            Quality = quality,
                            Method = WebpEncodingMethod.BestQuality
                        });
                    }
                }

                return webpPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting image to WebP: {ex.Message}");
            }
        }

        private static IImageFormat GetImageFormat(string extension)
        {
            return extension switch
            {
                ".png" => PngFormat.Instance,
                ".gif" => GifFormat.Instance,
                ".bmp" => BmpFormat.Instance,
                _ => JpegFormat.Instance,
            };
        }
        //Generate unique 32 unique case_id
        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
