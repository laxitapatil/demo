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
    [Tags("Blog Category Management")]
    [Route("api/blogcategory")]
    [ApiController]
    public class BlogCategoryController : BaseController
    {
        private readonly string BLOG_CATEGORY = "blog category";

        public BlogCategoryController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        /// <summary>
        /// Retrieve information about a specific / all Blog Category based on the Blog Category ID.
        /// </summary>
        /// <param name="id">if null then all, else specific</param>
        /// <returns>200 - Blog Category detail, 401 Unauthorized, 500 Internal Server Error - Error message</returns>
        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                using BlogCategoryRepository repoBlogCategory = new(dbContext);
                string response = await repoBlogCategory.SelectAll(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new Blog Category.
        /// </summary>
        /// <param name="req">Id Optional</param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Post(BlogCategoryRequest req)
        {
            try
            {
                BlogCategory blogCategory = mapper.Map<BlogCategoryRequest, BlogCategory>(req);
                using BlogCategoryRepository repoBlogCategory = new(dbContext);

                blogCategory.Modified_date = CurrentTime;

                await repoBlogCategory.Insert(blogCategory);

                return blogCategory.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, BLOG_CATEGORY))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, BLOG_CATEGORY), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Modify the details of an existing Blog Category.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(BlogCategoryRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                using BlogCategoryRepository repoBlogCategory = new(dbContext);
                BlogCategory blogCategory = await repoBlogCategory.SelectOne(req.Id.Value);

                if (blogCategory == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                blogCategory.Name = req.Name;
                blogCategory.Is_active = req.Is_active;
                blogCategory.Order_by = req.Order_by;
                blogCategory.Modified_date = CurrentTime;

                await repoBlogCategory.Update(blogCategory);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, BLOG_CATEGORY));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete an existing Blog Category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                using BlogCategoryRepository repoBlogCategory = new(dbContext);
                BlogCategory blogCategory = await repoBlogCategory.SelectOne(id);

                if (blogCategory == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG_CATEGORY), statusCode: StatusCodes.Status400BadRequest);

                await repoBlogCategory.Delete(blogCategory);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, BLOG_CATEGORY));
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