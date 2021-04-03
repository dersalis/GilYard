using System.Linq;
using GilYard.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using GilYard.Api.Models;

namespace GilYard.Api.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : ControllerBase
    {
        private readonly GilYardContext _context;

        public ReportsController(GilYardContext context)
        {
            _context = context;
        }


        [HttpGet("myvisitors/{userid}")]
        public IActionResult MyVisitors(int userid)
        {
            try
            {
                var visitorsFromDB = _context.Visitors.Where(v => v.GuardianId == userid).ToList();
                var visitors = visitorsFromDB.Adapt<ReportMyVisitors>();
                return Ok(visitors);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}