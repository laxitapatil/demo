using AutoMapper;
using Core.Domain;
using Core.Request;
using Api.Provider;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Tags("Testimonial Management")]
    [Route("api/testimonial")]
    [ApiController]
    public class TestimonialController : BaseController
    {
        private readonly string TESTIMONIAL = "testimonial";

        public TestimonialController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id, Guid? companyId)
        {
            try
            {
                using TestimonialRepository repoTestimonial = new(dbContext);
                string response = await repoTestimonial.SelectAll(id, companyId);
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
                using TestimonialRepository repoTestimonial = new(dbContext);
                string response = await repoTestimonial.SelectAll(null, null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Public endpoint - anyone can submit a testimonial. It is inactive by default until an admin approves it.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> Submit(TestimonialRequest req)
        {
            try
            {
                Testimonial testimonial = mapper.Map<TestimonialRequest, Testimonial>(req);
                using TestimonialRepository repoTestimonial = new(dbContext);

                testimonial.Is_active = false;
                testimonial.Created_date = CurrentTime;

                await repoTestimonial.Insert(testimonial);

                return testimonial.Id > 0
                    ? Ok("Thank you for sharing your experience!")
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, TESTIMONIAL), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(TestimonialRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, TESTIMONIAL), statusCode: StatusCodes.Status400BadRequest);

                using TestimonialRepository repoTestimonial = new(dbContext);
                Testimonial testimonial = await repoTestimonial.SelectOne(req.Id.Value);

                if (testimonial == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, TESTIMONIAL), statusCode: StatusCodes.Status400BadRequest);

                testimonial.Author_name = req.Author_name;
                testimonial.Comment = req.Comment;
                testimonial.Designation = req.Designation;
                testimonial.Rating = req.Rating;
                testimonial.Phone_number = req.Phone_number;
                testimonial.Email = req.Email;
                testimonial.Treatment = req.Treatment;
                testimonial.Modified_date = CurrentTime;

                await repoTestimonial.Update(testimonial);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, TESTIMONIAL));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Approve or unapprove a testimonial for public display.
        /// </summary>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id, bool isActive)
        {
            try
            {
                using TestimonialRepository repoTestimonial = new(dbContext);
                Testimonial testimonial = await repoTestimonial.SelectOne(id);

                if (testimonial == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, TESTIMONIAL), statusCode: StatusCodes.Status400BadRequest);

                testimonial.Is_active = isActive;
                testimonial.Modified_date = CurrentTime;

                await repoTestimonial.Update(testimonial);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, TESTIMONIAL));
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
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, TESTIMONIAL), statusCode: StatusCodes.Status400BadRequest);

                using TestimonialRepository repoTestimonial = new(dbContext);
                Testimonial testimonial = await repoTestimonial.SelectOne(id);

                if (testimonial == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, TESTIMONIAL), statusCode: StatusCodes.Status400BadRequest);

                await repoTestimonial.Delete(testimonial);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, TESTIMONIAL));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}