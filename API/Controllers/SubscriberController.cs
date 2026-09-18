using Core.Request;
using AutoMapper;
using Core.Domain;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Tags("Subscriber Management")]
    [Route("api/subscriber")]
    [ApiController]
    public class SubscriberController : BaseController
    {
        public SubscriberController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        /// <summary>
        /// Public endpoint - anyone can subscribe to the newsletter.
        /// </summary>
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(SubscriberRequest req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req.Email))
                    return Problem("Email is required.", statusCode: StatusCodes.Status400BadRequest);

                Subscriber subscriber = mapper.Map<SubscriberRequest, Subscriber>(req);
                subscriber.Is_active = true;
                subscriber.Created_date = CurrentTime;

                using SubscriberRepository repoSubscriber = new(dbContext);
                await repoSubscriber.Insert(subscriber);

                return Ok("Thanks for subscribing!");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Admin only - view all subscribers (for your future admin panel).
        /// </summary>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                using SubscriberRepository repoSubscriber = new(dbContext);
                string response = await repoSubscriber.SelectAll();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}