using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private IUserService _UserService;
        private IConfiguration _config;

        public AuthenController(IUserService service, IConfiguration config)
        {
            _UserService = service;
            _config = config;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult LogIn(LogInRequest UserModel)
        {
            try
            {
                BaseRespone<LoginRespone> respone = new BaseRespone<LoginRespone>();
                var model = _UserService.LogInUser(UserModel);
                if (model == null)
                {
                    respone.Type = "Error";
                    respone.Message = "Thông tin tài khoản mật khẩu không chính xác";
                    return Ok(respone);
                }
                respone.Type = "Success";
                respone.Message = "Đăng nhập thành công";
                var jwt = new JwtService(_config);

                LoginRespone DataUser = new LoginRespone();
                DataUser.UserId = model.UserId;
                DataUser.UserName = model.UserName;
                DataUser.Token = jwt.GenerateSecurityToken(model.UserName, model.UserId);
                respone.Data = DataUser;
                return Ok(respone);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
