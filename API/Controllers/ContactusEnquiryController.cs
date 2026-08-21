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
    [Tags("ContactUs Management")]
    [Route("api/contactusenquiry")]
    [ApiController]
    public class ContactusEnquiryController : BaseController
    {
        private readonly string CONTACTUS_ENQUIRY = "contact us enquiry";

        public ContactusEnquiryController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id, Guid? companyId, short? status)
        {
            try
            {
                using ContactusEnquiryRepository repoContactusEnquiry = new(dbContext);
                string response = await repoContactusEnquiry.SelectAll(id, companyId, status);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Public endpoint - no login required. Used by the website's Contact Us / Enquiry form.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(ContactusEnquiryRequest req)
        {
            try
            {
                ContactusEnquiry contactusEnquiry = mapper.Map<ContactusEnquiryRequest, ContactusEnquiry>(req);
                using ContactusEnquiryRepository repoContactusEnquiry = new(dbContext);

                contactusEnquiry.Status = 0; // Pending
                contactusEnquiry.Created_date = CurrentTime;

                await repoContactusEnquiry.Insert(contactusEnquiry);

                return contactusEnquiry.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, CONTACTUS_ENQUIRY))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, CONTACTUS_ENQUIRY), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update status (e.g. mark as Done) - admin only.
        /// </summary>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(ContactusEnquiryRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, CONTACTUS_ENQUIRY), statusCode: StatusCodes.Status400BadRequest);

                using ContactusEnquiryRepository repoContactusEnquiry = new(dbContext);
                ContactusEnquiry contactusEnquiry = await repoContactusEnquiry.SelectOne(req.Id.Value);

                if (contactusEnquiry == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, CONTACTUS_ENQUIRY), statusCode: StatusCodes.Status400BadRequest);

                contactusEnquiry.Status = req.Status ?? contactusEnquiry.Status;

                await repoContactusEnquiry.Update(contactusEnquiry);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, CONTACTUS_ENQUIRY));
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
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, CONTACTUS_ENQUIRY), statusCode: StatusCodes.Status400BadRequest);

                using ContactusEnquiryRepository repoContactusEnquiry = new(dbContext);
                ContactusEnquiry contactusEnquiry = await repoContactusEnquiry.SelectOne(id);

                if (contactusEnquiry == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, CONTACTUS_ENQUIRY), statusCode: StatusCodes.Status400BadRequest);

                await repoContactusEnquiry.Delete(contactusEnquiry);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, CONTACTUS_ENQUIRY));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}