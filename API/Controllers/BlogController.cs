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
    [Tags("Blog Management")]
    [Route("api/blog")]
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly string BLOG = "blog";

        public BlogController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        /// <summary>
        /// Retrieve information about a specific / all Blog based on the Blog ID.
        /// </summary>
        /// <param name="id">if null then all, else specific</param>
        /// <param name="companyId">optional filter by company</param>
        /// <returns>200 - Blog detail, 401 Unauthorized, 500 Internal Server Error - Error message</returns>
        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id, Guid? companyId)
        {
            try
            {
                using BlogRepository repoBlog = new(dbContext);
                string response = await repoBlog.SelectAll(id, companyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new Blog.
        /// </summary>
        /// <param name="req">Id Optional</param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Post(BlogRequest req)
        {
            try
            {
                Blog blog = mapper.Map<BlogRequest, Blog>(req);
                using BlogRepository repoBlog = new(dbContext);

                blog.Created_by = TokenName;
                blog.Created_date = CurrentTime;

                await repoBlog.Insert(blog);

                return blog.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, BLOG))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, BLOG), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Modify the details of an existing Blog.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(BlogRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG), statusCode: StatusCodes.Status400BadRequest);

                using BlogRepository repoBlog = new(dbContext);
                Blog blog = await repoBlog.SelectOne(req.Id.Value);

                if (blog == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG), statusCode: StatusCodes.Status400BadRequest);

                blog.Blog_category_id = req.Blog_category_id;
                blog.Title = req.Title;
                blog.Slug = req.Slug;
                blog.Short_description = req.Short_description;
                blog.Description = req.Description;
                blog.Image = req.Image;
                blog.Tags = req.Tags;
                blog.Is_active = req.Is_active;
                blog.Is_popular = req.Is_popular;
                blog.Order_by = req.Order_by;
                blog.Modified_by = TokenName;
                blog.Modified_date = CurrentTime;

                await repoBlog.Update(blog);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, BLOG));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete an existing Blog.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG), statusCode: StatusCodes.Status400BadRequest);

                using BlogRepository repoBlog = new(dbContext);
                Blog blog = await repoBlog.SelectOne(id);

                if (blog == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, BLOG), statusCode: StatusCodes.Status400BadRequest);

                await repoBlog.Delete(blog);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, BLOG));
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