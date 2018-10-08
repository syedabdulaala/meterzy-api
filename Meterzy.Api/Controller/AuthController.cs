using Meterzy.Api.Extension;
using Meterzy.Api.Helper;
using Meterzy.Api.Model.Request.Auth;
using Meterzy.Data;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller
{
    [Route("auth")]
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        #region Variable(s)
        private readonly IRepo<AppUser> _appUser;
        #endregion

        #region Constructor(s)
        public AuthController(IRepo<AppUser> appUser) : base()
        {
            _appUser = appUser;
        }
        #endregion

        #region POST Method(s)
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (await _appUser.DataSet.Where(x => x.EmailHash == request.Email.ToSha256Hash(null)).AnyAsync())
                {
                    return ClientError(
                        code: HttpResponse.EmailAddressAlreadyExist.Key,
                        message: HttpResponse.EmailAddressAlreadyExist.Value
                    );
                }

                var newUser = new AppUser
                {
                    DisplayName = $"{request.FirstName} {request.LastName}",
                    PasswordHash = request.Password.ToSha256Hash(Config.Secrets.PasswordSalt),
                    EmailHash = request.Email.ToSha256Hash(),
                    Status = AppUserStatus.Active
                };
                _appUser.Add(newUser);
                await _appUser.SaveAsync();
                return Success();
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var appUser = await _appUser.DataSet.Where(x => x.EmailHash == request.Email.ToSha256Hash(null)).FirstOrDefaultAsync();
                if (appUser == null || appUser.PasswordHash != request.Password.ToSha256Hash(Config.Secrets.PasswordSalt))
                {
                    return ClientError(
                        code: HttpResponse.InvalidCredentials.Key,
                        message: HttpResponse.InvalidCredentials.Value
                    );
                }

                var claims = new List<Claim> { new Claim("usr", appUser.Id.ToString().ToSha256Hash(null)) };
                Response.Headers.Add(Const.HttpHeaderKey.Authorization, BuildToken(claims));

                return Success();
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion

        #region General Method(s)
        private string BuildToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Secrets.EncryptionKeys.Jwt));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Config.AppSettings.Jwt.Authority,
                audience: Config.AppSettings.Jwt.Audience,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds,
                claims: claims
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}