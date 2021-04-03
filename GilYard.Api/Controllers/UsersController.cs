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

namespace GilYard.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly GilYardContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public UsersController(GilYardContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegister request)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje w bazie
                var userFromDB = _context.Users.SingleOrDefault(u => u.Email == request.Email);
                // Jeśli juz istnieje
                if (userFromDB != null) return BadRequest($"Uzytkonwnik {request.Email} juz istnieje.");

                // Jeśli nie istnieje to utworz
                TypeAdapterConfig<UserRegister, User>.NewConfig()
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.Password, src => SecurePasswordHasherHelper.Hash(src.Password));
                var newUser = request.Adapt<User>();
                // newUser.Password = SecurePasswordHasherHelper.Hash(request.Password);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin request)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje
                var userFromDB = _context.Users.FirstOrDefault(u => u.Email == request.Email);
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


        [Authorize]
        [HttpPut("changepassword")]
        public IActionResult ChangePassword([FromBody] UserChangePassword request)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje
                var userFromDB = _context.Users.FirstOrDefault(u => u.Email == request.Email);

                // Jeśli uzytkownik nie istnieje lub stare hasło jest nie poprawne
                if (userFromDB == null || !SecurePasswordHasherHelper.Verify(request.OldPassword, userFromDB.Password)) return NotFound("Nie można odnaleźć konta z taką nazwą użytkownika.");

                // Zmień hasło
                userFromDB.Password = SecurePasswordHasherHelper.Hash(request.NewPassword);

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("deactivation/{id}")]
        public IActionResult Deactivation(int id)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje
                var userFromDB = _context.Users.Find(id);

                // Jeśli uzytkownik nie istnieje lub stare hasło jest nie poprawne
                if (userFromDB == null) return NotFound("Nie można odnaleźć konta z taką nazwą użytkownika.");

                // Zmień hasło
                userFromDB.IsActive = false;

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("activation/{id}")]
        public IActionResult Activation(int id)
        {
            try
            {
                // Sprawdz czy uzytkownik istnieje
                var userFromDB = _context.Users.Find(id);

                // Jeśli uzytkownik nie istnieje lub stare hasło jest nie poprawne
                if (userFromDB == null) return NotFound("Nie można odnaleźć konta z taką nazwą użytkownika.");

                // Zmień hasło
                userFromDB.IsActive = true;

                _context.SaveChanges();

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
                var usersFromDB = _context.Users.Where(u => u.IsActive == true).ToList();
                var users = usersFromDB.Adapt<List<UserForList>>();

                return Ok(users);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}