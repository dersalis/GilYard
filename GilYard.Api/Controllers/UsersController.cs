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
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _user;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public UsersController(IUserService user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }


        /// <summary>
        /// Dodaje użytkownika
        /// </summary>
        /// <param name="request">Parametry nowego użytkownika</param>
        /// <returns>Status HTTP</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add([FromBody] UserRegister request)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje w bazie
                var userFromDB = _user.GetByEmail(request.Email);
                // Jeśli juz istnieje
                if (userFromDB != null) return BadRequest($"Uzytkonwnik {request.Email} juz istnieje.");

                var hashedPassword = SecurePasswordHasherHelper.Hash(request.Password);
                // Jeśli nie istnieje to utworz
                _user.Add(request, hashedPassword);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Zmiania hasło użytkownika
        /// </summary>
        /// <param name="request">Parametry zmiany</param>
        /// <returns>Status HTTP</returns>
        [Authorize]
        [HttpPatch("changepassword")]
        public IActionResult ChangePassword([FromBody] UserChangePassword request)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje
                var userFromDB = _user.GetByEmail(request.Email);

                // Jeśli uzytkownik nie istnieje lub stare hasło jest nie poprawne
                if (userFromDB == null || !SecurePasswordHasherHelper.Verify(request.OldPassword, userFromDB.Password)) return NotFound("Nie można odnaleźć konta z taką nazwą użytkownika.");

                // Zmień hasło
                var hashedPassword = SecurePasswordHasherHelper.Hash(request.NewPassword);

                _user.ChangePassword(request.Email, hashedPassword);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        
        [Authorize(Roles = "Admin")]
        [HttpPatch("changestatus")]
        public IActionResult ChangeStatus(int id, bool setActive)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje
                var userFromDB = _user.GetById(id);

                // Jeśli uzytkownik nie istnieje lub stare hasło jest nie poprawne
                if (userFromDB == null) return NotFound("Nie można odnaleźć konta z taką nazwą użytkownika.");

                // Zmień status
                _user.ChangeStatus(id, setActive);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("getforlist")]
        public IActionResult GetForList()
        {
            try
            {
                var users = _user.GetForList();

                return Ok(users);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}