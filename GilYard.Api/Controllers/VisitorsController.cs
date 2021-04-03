using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using GilYard.Api.Data;
using GilYard.Api.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GilYard.Api.Controllers
{
    [ApiController]
    [Route("visitors")]
    public class VisitorsController : ControllerBase
    {
        private readonly GilYardContext _context;

        public VisitorsController(GilYardContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var visitorsFromDB = _context.Visitors.Where(v => v.LeaveDate == null).ToList();
                
                TypeAdapterConfig<Visitor, VisitorForTable>.NewConfig().Map(dest => dest.UserName, src => src.Guardian.Name);
                var visitors = visitorsFromDB.Adapt<List<VisitorForTable>>();

                return Ok(visitors);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPost]
        public IActionResult Add([FromBody] VisitorAdd request)
        {
            try
            {
                var visitor = request.Adapt<Visitor>();
                _context.Visitors.Add(visitor);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] VisitorAdd request)
        {
            try
            {
                var visitorsFromDB = _context.Visitors.Find(id);
                if(visitorsFromDB == null) return NotFound("Nie można odnaleźć gościa.");

                if(!string.IsNullOrEmpty(request.Name)) visitorsFromDB.Name = request.Name;
                if(!string.IsNullOrEmpty(request.Phone)) visitorsFromDB.Phone = request.Phone;
                if(!string.IsNullOrEmpty(request.CarPlate)) visitorsFromDB.CarPlate = request.CarPlate;
                if(visitorsFromDB.GuardianId != request.GuardianId) visitorsFromDB.GuardianId = request.GuardianId;

                _context.SaveChanges();

                return Ok(new { VisitorId = id });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var visitorsFromDB = _context.Visitors.Find(id);
                if(visitorsFromDB == null) return NotFound("Nie można odnaleźć gościa.");

                _context.Visitors.Remove(visitorsFromDB);
                _context.SaveChanges();

                return Ok();
            }
            catch (System.Exception ex)
            {
                 return BadRequest(ex.Message);
            }
        }
        

        [Authorize]
        [HttpPut("comein/{id}")]
        public IActionResult ComeIn(int id)
        {
            try
            {
                var visitorsFromDB = _context.Visitors.Find(id);
                if(visitorsFromDB == null) return NotFound("Nie można odnaleźć gościa.");

                visitorsFromDB.ArriveDate = DateTime.Now;

                _context.SaveChanges();

                return Ok(new { VisitorId = id });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPut("comeout/{id}")]
        public IActionResult ComeOut(int id)
        {
            try
            {
                var visitorsFromDB = _context.Visitors.Find(id);
                if(visitorsFromDB == null) return NotFound("Nie można odnaleźć gościa.");
                if(visitorsFromDB.ArriveDate == null) return BadRequest("Nie mozna wypuścić gości, który nie został jeszcze wpuszczony.");

                visitorsFromDB.LeaveDate = DateTime.Now;

                _context.SaveChanges();

                return Ok(new { VisitorId = id });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}