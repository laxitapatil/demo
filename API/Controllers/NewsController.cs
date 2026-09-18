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
    [Tags("News Management")]
    [Route("api/news")]
    [ApiController]
    public class NewsController : BaseController
    {
        private readonly string NEWS = "news";

        public NewsController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id, Guid? companyId)
        {
            try
            {
                using NewsRepository repoNews = new(dbContext);
                string response = await repoNews.SelectAll(id, companyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublic()
        {
            try
            {
                using NewsRepository repoNews = new(dbContext);
                string response = await repoNews.SelectAll(null, null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Post(NewsRequest req)
        {
            try
            {
                News news = mapper.Map<NewsRequest, News>(req);
                using NewsRepository repoNews = new(dbContext);

                news.Created_by = TokenName;
                news.Created_date = CurrentTime;

                await repoNews.Insert(news);

                return news.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, NEWS))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, NEWS), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(NewsRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS), statusCode: StatusCodes.Status400BadRequest);

                using NewsRepository repoNews = new(dbContext);
                News news = await repoNews.SelectOne(req.Id.Value);

                if (news == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS), statusCode: StatusCodes.Status400BadRequest);

                news.News_category_id = req.News_category_id;
                news.Title = req.Title;
                news.Slug = req.Slug;
                news.Short_description = req.Short_description;
                news.Description = req.Description;
                news.Image = req.Image;
                news.Tags = req.Tags;
                news.Is_active = req.Is_active;
                news.Is_popular = req.Is_popular;
                news.Order_by = req.Order_by;
                news.Modified_by = TokenName;
                news.Modified_date = CurrentTime;

                await repoNews.Update(news);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, NEWS));
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
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS), statusCode: StatusCodes.Status400BadRequest);

                using NewsRepository repoNews = new(dbContext);
                News news = await repoNews.SelectOne(id);

                if (news == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS), statusCode: StatusCodes.Status400BadRequest);

                await repoNews.Delete(news);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, NEWS));
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