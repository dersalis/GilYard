using System.Linq;
using GilYard.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using GilYard.Api.Models;
using GilYard.Api.Services;

namespace GilYard.Api.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IVisitorsService _visitorsService;

        public ReportsController(IVisitorsService visitorsService)
        {
            _visitorsService = visitorsService;
        }


        /// <summary>
        /// Zwraca gości użytkownika
        /// </summary>
        /// <param name="userid">Id użytkownika</param>
        /// <returns>Goście</returns>
        [HttpGet("myvisitors/{userid}")]
        public IActionResult MyVisitors([FromRoute] int userid)
        {
            try
            {
                var visitors = _visitorsService.GetByUserId(userid);
                return StatusCode(200, visitors); // OK
            }
            catch (System.Exception ex)
            {
                return StatusCode(400, ex.Message); // Bad Request
            }
        }

    }
}