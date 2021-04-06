using System.Linq;
using AuthenticationPlugin;
using GilYard.Api.Data;
using GilYard.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mapster;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using GilYard.Api.Services;

namespace GilYard.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _user;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public AuthController(IUserService user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        /// <summary>
        /// Logowanie uzytkownika
        /// </summary>
        /// <param name="request">Parametry logowania</param>
        /// <returns>Token uzytkownika</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin request)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje
                var userFromDB = _user.GetByEmail(request.Email);
                // Jeśli uzytkownik nie istnieje
                if (userFromDB == null) return NotFound("Nie można odnaleźć konta z taką nazwą użytkownika.");

                // Sprawdz hasło
                if (!SecurePasswordHasherHelper.Verify(request.Password, userFromDB.Password))
                {
                    return Unauthorized();
                }

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Email, userFromDB.Email),
                new Claim(ClaimTypes.Email, userFromDB.Email),
                new Claim(ClaimTypes.Role, userFromDB.Role),
            };
                var token = _auth.GenerateAccessToken(claims);

                return new ObjectResult(new
                {
                    access_token = token.AccessToken,
                    expires_in = token.ExpiresIn,
                    token_type = token.TokenType,
                    creation_Time = token.ValidFrom,
                    expiration_Time = token.ValidTo,
                    user_id = userFromDB.Id,
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}