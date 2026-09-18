using Api.Provider;
using AutoMapper;
using Core.Domain;
using Core.Request;
using Core.Response;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Tags("Media Management")]
    [Route("api/media")]
    [ApiController]
    public class MediaController : BaseController
    {
        private readonly string MEDIA = "media";
        private readonly IWebHostEnvironment _env;

        // NOTE: IWebHostEnvironment is added here so we can find wwwroot to save uploaded files.
        // ASP.NET Core's built-in DI will supply this automatically - no manual wiring needed.
        public MediaController(IConfiguration configuration, IMapper mapper, DBContext dbContext, IWebHostEnvironment env)
            : base(configuration, mapper, dbContext)
        {
            _env = env;
        }

        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id, Guid? companyId)
        {
            try
            {
                using MediaRepository repoMedia = new(dbContext);
                string response = await repoMedia.SelectAll(id, companyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Post(MediaRequest req)
        {
            try
            {
                Media media = mapper.Map<MediaRequest, Media>(req);
                using MediaRepository repoMedia = new(dbContext);

                media.Modified_date = CurrentTime;

                await repoMedia.Insert(media);

                return media.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, MEDIA))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, MEDIA), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // ================= NEW: FILE UPLOAD ENDPOINT =================
        // Accepts an actual image file from the browser (multipart/form-data),
        // saves it to wwwroot/uploads/media, then inserts a Media row pointing
        // at the saved file - same DB insert path as the existing Post() above.
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] Guid company_id, [FromForm] bool is_active = true, [FromForm] int order_by = 0)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Problem("No file uploaded.", statusCode: StatusCodes.Status400BadRequest);

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                    return Problem("Unsupported file type.", statusCode: StatusCodes.Status400BadRequest);

                // wwwroot may not exist in some hosting setups - fall back to ContentRoot/wwwroot if needed
                var webRoot = string.IsNullOrEmpty(_env.WebRootPath)
                    ? Path.Combine(_env.ContentRootPath, "wwwroot")
                    : _env.WebRootPath;

                var uploadsFolder = Path.Combine(webRoot, "uploads", "media");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/uploads/media/{uniqueFileName}";

                Media media = new Media
                {
                    Company_id = company_id,
                    Name = fileUrl,
                    Is_active = is_active,
                    Order_by = order_by,
                    Modified_date = CurrentTime
                };

                using MediaRepository repoMedia = new(dbContext);
                await repoMedia.Insert(media);

                if (media.Id <= 0)
                {
                    // DB insert failed - clean up the file we just saved so we don't leave orphans on disk
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    return Problem(string.Format(MessageProvider.INSERT_FAILED, MEDIA), statusCode: StatusCodes.Status400BadRequest);
                }

                return Ok(new { id = media.Id, url = fileUrl });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(MediaRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MEDIA), statusCode: StatusCodes.Status400BadRequest);

                using MediaRepository repoMedia = new(dbContext);
                Media media = await repoMedia.SelectOne(req.Id.Value);

                if (media == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MEDIA), statusCode: StatusCodes.Status400BadRequest);

                media.Name = req.Name;
                media.Is_active = req.Is_active;
                media.Order_by = req.Order_by;
                media.Modified_date = CurrentTime;

                await repoMedia.Update(media);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, MEDIA));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, MEDIA), statusCode: StatusCodes.Status400BadRequest);

                using MediaRepository repoMedia = new(dbContext);
                Media media = await repoMedia.SelectOne(id);

                if (media == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MEDIA), statusCode: StatusCodes.Status400BadRequest);

                // Also delete the physical file from disk so we don't accumulate orphaned uploads
                if (!string.IsNullOrEmpty(media.Name) && media.Name.StartsWith("/uploads/media/"))
                {
                    var webRoot = string.IsNullOrEmpty(_env.WebRootPath)
                        ? Path.Combine(_env.ContentRootPath, "wwwroot")
                        : _env.WebRootPath;
                    var physicalPath = Path.Combine(webRoot, media.Name.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(physicalPath))
                        System.IO.File.Delete(physicalPath);
                }

                await repoMedia.Delete(media);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, MEDIA));
            }
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("fk"))
                    return Problem(MessageProvider.CHILD_FOUND, statusCode: StatusCodes.Status400BadRequest);

                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
