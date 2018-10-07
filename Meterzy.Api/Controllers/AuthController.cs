using Meterzy.Api.Extension;
using Meterzy.Api.Model.Request.Auth;
using Meterzy.Data;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Meterzy.Api.Controllers
{
    [Route("auth")]
    public class AuthController : BaseApiController
    {
        #region Variable(s)
        private readonly IRepo<AppUser> _appUser;
        #endregion

        #region Constructor(s)
        public AuthController(IRepo<AppUser> appUser, ILogger<BaseApiController> logger) : base(logger)
        {
            _appUser = appUser;
        }
        #endregion

        #region POST Method(s)
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ClientError();
                }

                var newUser = new AppUser
                {
                    DisplayName = $"{registerRequest.FirstName} {registerRequest.LastName}",
                    PasswordHash = registerRequest.Password.ToSha256Hash(),
                    EmailHash = registerRequest.Email.ToSha256Hash(),
                    Status = AppUserStatus.Active
                };
                _appUser.Add(newUser);
                await _appUser.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion
    }
}