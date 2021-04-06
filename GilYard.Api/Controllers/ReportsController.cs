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
        public IActionResult MyVisitors(int userid)
        {
            try
            {
                var visitors = _visitorsService.GetByUserId(userid);
                return Ok(visitors);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}