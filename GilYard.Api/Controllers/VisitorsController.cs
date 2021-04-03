using System;
using System.Collections.Generic;
using System.Linq;
using GilYard.Api.Data;
using GilYard.Api.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var visitorsFromRepo = _context.Visitors.Where(v => v.LeaveDate == null).ToList();
                
                TypeAdapterConfig<Visitor, VisitorForTable>.NewConfig().Map(dest => dest.UserName, src => src.Guardian.Name);
                var visitors = visitorsFromRepo.Adapt<List<VisitorForTable>>();

                return Ok(visitors);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


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


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] VisitorAdd request)
        {
            try
            {
                var visitorsFromRepo = _context.Visitors.Find(id);
                if(visitorsFromRepo == null) return NotFound("Nie można odnaleźć gościa.");

                if(!string.IsNullOrEmpty(request.Name)) visitorsFromRepo.Name = request.Name;
                if(!string.IsNullOrEmpty(request.Phone)) visitorsFromRepo.Phone = request.Phone;
                if(!string.IsNullOrEmpty(request.CarPlate)) visitorsFromRepo.CarPlate = request.CarPlate;
                if(visitorsFromRepo.GuardianId != request.GuardianId) visitorsFromRepo.GuardianId = request.GuardianId;

                _context.SaveChanges();

                return Ok(new { VisitorId = id });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var visitorsFromRepo = _context.Visitors.Find(id);
                if(visitorsFromRepo == null) return NotFound("Nie można odnaleźć gościa.");

                _context.Visitors.Remove(visitorsFromRepo);
                _context.SaveChanges();

                return Ok();
            }
            catch (System.Exception ex)
            {
                 return BadRequest(ex.Message);
            }
        }
        

        [HttpPut("comein/{id}")]
        public IActionResult ComeIn(int id)
        {
            try
            {
                var visitorsFromRepo = _context.Visitors.Find(id);
                if(visitorsFromRepo == null) return NotFound("Nie można odnaleźć gościa.");

                visitorsFromRepo.ArriveDate = DateTime.Now;

                _context.SaveChanges();

                return Ok(new { VisitorId = id });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("comeout/{id}")]
        public IActionResult ComeOut(int id)
        {
            try
            {
                var visitorsFromRepo = _context.Visitors.Find(id);
                if(visitorsFromRepo == null) return NotFound("Nie można odnaleźć gościa.");
                if(visitorsFromRepo.ArriveDate == null) return BadRequest("Nie mozna wypuścić gości, który nie został jeszcze wpuszczony.");

                visitorsFromRepo.LeaveDate = DateTime.Now;

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