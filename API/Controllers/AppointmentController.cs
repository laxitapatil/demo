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
    [Tags("Appointment Management")]
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : BaseController
    {
        private readonly string APPOINTMENT = "appointment";

        public AppointmentController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        /// <summary>
        /// Retrieve information about a specific / all Appointment based on the Appointment ID.
        /// </summary>
        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id, Guid? companyId)
        {
            try
            {
                using AppointmentRepository repoAppointment = new(dbContext);
                string response = await repoAppointment.SelectAll(id, companyId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new Appointment. Public endpoint — used by the website's Contact/Book form.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(AppointmentRequest req)
        {
            try
            {
                if (req.EndTime <= req.StartTime)
                    return Problem("EndTime must be after StartTime.", statusCode: StatusCodes.Status400BadRequest);

                using AppointmentRepository repoAppointment = new(dbContext);

                Appointment appointment = mapper.Map<AppointmentRequest, Appointment>(req);
                appointment.CreatedDate = CurrentTime;

                await repoAppointment.Insert(appointment);

                return appointment.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, APPOINTMENT))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, APPOINTMENT), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Modify the details of an existing Appointment.
        /// </summary>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put(AppointmentRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, APPOINTMENT), statusCode: StatusCodes.Status400BadRequest);

                if (req.EndTime <= req.StartTime)
                    return Problem("EndTime must be after StartTime.", statusCode: StatusCodes.Status400BadRequest);

                using AppointmentRepository repoAppointment = new(dbContext);
                Appointment appointment = await repoAppointment.SelectOne(req.Id.Value);

                if (appointment == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, APPOINTMENT), statusCode: StatusCodes.Status400BadRequest);

                appointment.Candidate_name = req.Candidate_name;
                appointment.Mobile_num = req.Mobile_num;
                appointment.Email = req.Email;
                appointment.StartTime = req.StartTime;
                appointment.EndTime = req.EndTime;
                appointment.Note = req.Note;
                appointment.ModifiedBy = (short?)TokenUserId;
                appointment.ModifiedDate = CurrentTime;

                await repoAppointment.Update(appointment);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, APPOINTMENT));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete an existing Appointment.
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, APPOINTMENT), statusCode: StatusCodes.Status400BadRequest);

                using AppointmentRepository repoAppointment = new(dbContext);
                Appointment appointment = await repoAppointment.SelectOne(id);

                if (appointment == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, APPOINTMENT), statusCode: StatusCodes.Status400BadRequest);

                await repoAppointment.Delete(appointment);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, APPOINTMENT));
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