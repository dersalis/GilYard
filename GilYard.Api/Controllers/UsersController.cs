using GilYard.Api.Data;
using GilYard.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GilYard.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly GilYardContext _context;

        public UsersController(GilYardContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public IActionResult Register()
        {
            try
            {
                var temp = new User(){
                    Name = "Olga Spokojna",
                    Email = "olgas@wp.pl",
                    Password = "xxx",
                    Role = "User",
                    Phone = "123345566"
                };

                _context.Users.Add(temp);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}