using Api.Provider;
using AutoMapper;
using Core.Domain;
using Core.Enumeration;
using Core.Request;
using Core.Response;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Tags("Menu Management")]
    [Route("api/menu")]
    [ApiController]
    public class MenuController : BaseController
    {
        private readonly string MENU = "menu";

        public MenuController(IConfiguration configuration, IMapper mapper, DBContext dbContext)
            : base(configuration, mapper, dbContext) { }

        /// <summary>
        /// Retrieve information about a specific / all Menu based on the Menu ID.
        /// </summary>
        /// <param name="id">if null then all, else specific</param>
        /// <returns>200 - Menu detail, 401 Unauthorized, 500 Internal Server Error - Error message</returns>
        [Authorize]
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(MenuType? menuType, int? id)
        {
            try
            {
                using MenuRepository repoMenu = new(dbContext);
                string response = await repoMenu.SelectAll(id, menuType == null ? null : Convert.ToInt16(menuType));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("role/{menuType}")]
        public async Task<IActionResult> GetByRole(MenuType menuType, int? parentId)
        {
            try
            {
                using MenuRepository repoMenu = new(dbContext);
                string response = await repoMenu.SelectByRole(TokenRole.ToString(), Convert.ToInt16(menuType), parentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new Menu.
        /// </summary>
        /// <param name="req">Id Optional</param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Post(MenuRequest req)
        {
            try
            {
                Menu menu = mapper.Map<MenuRequest, Menu>(req);
                using MenuRepository repoMenu = new(dbContext);

                menu.Modified_by = TokenName;
                menu.Modified_date = CurrentTime;

                await repoMenu.Insert(menu);

                return menu.Id > 0
                    ? Ok(string.Format(MessageProvider.INSERT_SUCCESS, MENU))
                    : Problem(string.Format(MessageProvider.INSERT_FAILED, MENU), statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Modify the details of an existing Menu.
        /// </summary>
        /// <param name="req"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Put(MenuRequest req)
        {
            try
            {
                if (!req.Id.HasValue || req.Id < 1)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MENU), statusCode: StatusCodes.Status400BadRequest);

                using MenuRepository repoMenu = new(dbContext);
                Menu menu = await repoMenu.SelectOne(req.Id.Value);

                if (menu == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MENU), statusCode: StatusCodes.Status400BadRequest);

                menu.Name = req.Name;
                menu.Caption = req.Caption;
                menu.Parent_id = req.Parent_id;
                menu.Menu_type = req.Menu_type;
                menu.Url = req.Url;
                menu.Css_class = req.Css_class;
                menu.Order_by = req.Order_by;
                menu.Modified_by = TokenName;
                menu.Modified_date = CurrentTime;

                await repoMenu.Update(menu);
                return Ok(string.Format(MessageProvider.UPDATE_SUCCESS, MENU));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete an existing Menu.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 - Success message, 401 Unauthorized, 400 Bad request - Error message, 500 Internal Server Error - Error message</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1) return Problem(string.Format(MessageProvider.NOT_FOUND, MENU), statusCode: StatusCodes.Status400BadRequest);

                using MenuRepository repoMenu = new(dbContext);
                Menu menu = await repoMenu.SelectOne(id);

                if (menu == null)
                    return Problem(string.Format(MessageProvider.NOT_FOUND, MENU), statusCode: StatusCodes.Status400BadRequest);

                await repoMenu.Delete(menu);
                return Ok(string.Format(MessageProvider.DELETE_SUCCESS, MENU));
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