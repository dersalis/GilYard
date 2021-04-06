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
using GilYard.Api.Services;

namespace GilYard.Api.Controllers
{
    [ApiController]
    [Route("visitors")]
    public class VisitorsController : ControllerBase
    {
        private readonly IVisitorsService _visitorsService;

        public VisitorsController(IVisitorsService visitorsService)
        {
            _visitorsService = visitorsService;
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var visitors = _visitorsService.GetAll();

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
                _visitorsService.Add(request);

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
                var uId = _visitorsService.Update(id, request);

                return Ok(new { VisitorId = uId });
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
                _visitorsService.Delete(id);

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
                var uId = _visitorsService.ComeIn(id);

                return Ok(new { VisitorId = uId });
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
                var uId = _visitorsService.ComeOut(id);

                return Ok(new { VisitorId = uId });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}