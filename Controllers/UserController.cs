using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _UserService;
        public UserController(IUserService service)
        {
            _UserService = service;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var Users = _UserService.GetUsersList();
                if (Users == null) return NotFound();
                return Ok(Users);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("[action]/id")]
        public IActionResult GetUsersById(int id)
        {
            try
            {
                var Users = _UserService.GetUserDetailsById(id);
                if (Users == null) return NotFound();
                return Ok(Users);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveUsers(Users UserModel)
        {
            try
            {
                var model = _UserService.SaveUser(UserModel);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var model = _UserService.DeleteUser(id);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }

}
