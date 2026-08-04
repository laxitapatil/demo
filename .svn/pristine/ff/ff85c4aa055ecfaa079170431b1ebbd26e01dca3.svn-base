using Api.Provider;
using AutoMapper;
using Core.Domain;
using Core.Request;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Tags("Menu Rights Management")]
    [Authorize(Roles = "Admin")]
    [Route("api/menu-rights")]
    [ApiController]
    public class MenuRightsController : BaseController
    {
        private readonly string MENU_RIGHTS = "menu rights";

        public MenuRightsController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        /// <summary>
        /// Retrieve information about a specific / all Menu rights based on the Menu rights ID.
        /// </summary>
        /// <param name="roleId">if null then all, else specific</param>
        /// <param name="menuId">if null then all, else specific</param>
        /// <param name="id">if null then all, else specific</param>
        /// <returns>200 - Menu detail, 401 Unauthorized, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get(string? roleId, int? menuId, int? id)
        {
            try
            {
                return Ok(await new MenuRightsRepository(dbContext).SelectAll(id, roleId, menuId));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new Menu rights.
        /// </summary>
        /// <param name="req">Id Optional</param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post(MenuRightsRequest req)
        {
            try
            {
                MenuRights rights = mapper.Map<MenuRightsRequest, MenuRights>(req);
                using MenuRightsRepository repoRights = new(dbContext);

                rights.Modified_by = TokenName;
                rights.Modified_date = CurrentTime;

                await repoRights.Insert(rights);

                return rights.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, MENU_RIGHTS))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, MENU_RIGHTS), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("uk_menu_rights"))
                    return Problem(string.Format(MessageProvider.ALREADY_EXISTS, MENU_RIGHTS), statusCode: StatusCodes.Status400BadRequest);

                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Modify the details of an existing Menu rights.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Put(MenuRightsRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MENU_RIGHTS), statusCode: StatusCodes.Status400BadRequest);

                using MenuRightsRepository repoRights = new(dbContext);
                MenuRights rights = await repoRights.SelectOne(req.Id.Value);

                if (rights == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MENU_RIGHTS), statusCode: StatusCodes.Status400BadRequest);

                rights.Role_id = req.Role_id;
                rights.Menu_id = req.Menu_id;
                rights.Modified_by = TokenName;
                rights.Modified_date = CurrentTime;

                await repoRights.Update(rights);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, MENU_RIGHTS));
            }
            catch (Exception ex)
            {
                if (ex.ToString().ToLower().Contains("uk_menu_rights"))
                    return Problem(string.Format(MessageProvider.ALREADY_EXISTS, MENU_RIGHTS), statusCode: StatusCodes.Status400BadRequest);

                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete an existing Menu rights.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, MENU_RIGHTS), statusCode: StatusCodes.Status400BadRequest);

                using MenuRightsRepository repoRights = new(dbContext);
                MenuRights rights = await repoRights.SelectOne(id);

                if (rights == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MENU_RIGHTS), statusCode: StatusCodes.Status400BadRequest);

                await repoRights.Delete(rights);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, MENU_RIGHTS));
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