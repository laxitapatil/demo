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
    [Tags("News Category Management")]
    [Route("api/newscategory")]
    [ApiController]
    public class NewsCategoryController : BaseController
    {
        private readonly string NEWS_CATEGORY = "news category";

        public NewsCategoryController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                using NewsCategoryRepository repoNewsCategory = new(dbContext);
                string response = await repoNewsCategory.SelectAll(id);
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
                using NewsCategoryRepository repoNewsCategory = new(dbContext);
                string response = await repoNewsCategory.SelectAll(null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Post(NewsCategoryRequest req)
        {
            try
            {
                NewsCategory newsCategory = mapper.Map<NewsCategoryRequest, NewsCategory>(req);
                using NewsCategoryRepository repoNewsCategory = new(dbContext);

                newsCategory.Modified_date = CurrentTime;

                await repoNewsCategory.Insert(newsCategory);

                return newsCategory.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, NEWS_CATEGORY))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, NEWS_CATEGORY), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(NewsCategoryRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                using NewsCategoryRepository repoNewsCategory = new(dbContext);
                NewsCategory newsCategory = await repoNewsCategory.SelectOne(req.Id.Value);

                if (newsCategory == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                newsCategory.Name = req.Name;
                newsCategory.Is_active = req.Is_active;
                newsCategory.Order_by = req.Order_by;
                newsCategory.Modified_date = CurrentTime;

                await repoNewsCategory.Update(newsCategory);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, NEWS_CATEGORY));
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
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                using NewsCategoryRepository repoNewsCategory = new(dbContext);
                NewsCategory newsCategory = await repoNewsCategory.SelectOne(id);

                if (newsCategory == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, NEWS_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                await repoNewsCategory.Delete(newsCategory);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, NEWS_CATEGORY));
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