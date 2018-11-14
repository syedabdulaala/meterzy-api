using Meterzy.Api.Extension;
using Meterzy.Api.Helper;
using Meterzy.Api.Controller.Auth.Model;
using Meterzy.Data;
using Meterzy.Entity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Meterzy.Api.Controller.Auth
{
    [Route("auth")]
    [AllowAnonymous]
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
                    PasswordHash = request.Password.ToSha256Hash(Config.Secrets.Salt),
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
                if (appUser == null || appUser.PasswordHash != request.Password.ToSha256Hash(Config.Secrets.Salt))
                {
                    return ClientError(
                        code: HttpResponse.InvalidCredentials.Key,
                        message: HttpResponse.InvalidCredentials.Value
                    );
                }
                
                return Success(new {
                    appUser.DisplayName,
                    Token = BuildToken(appUser.Id.ToString())
                });
            }
            catch (Exception ex)
            {
                return ServerError(ex);
            }
        }
        #endregion

        #region General Method(s)
        private string BuildToken(string appUserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Config.Secrets.JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, appUserId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}