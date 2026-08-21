using Api.Provider;
using AutoMapper;
using Core.Domain;
using Core.Request;
using Core.Response;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Tags("Media Management")]
    [Route("api/media")]
    [ApiController]
    public class MediaController : BaseController
    {
        private readonly string MEDIA = "media";

        public MediaController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

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