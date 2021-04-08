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
                int id = _visitorsService.Add(request);

                return StatusCode(201, id); // Created
            }
            catch (System.Exception ex)
            {
                return StatusCode(400, ex.Message); // Bad Request
            }
        }


        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] VisitorAdd request)
        {
            try
            {
                var uId = _visitorsService.Update(id, request);

                return StatusCode(200, uId); // Ok
            }
            catch (System.Exception ex)
            {
                return StatusCode(400, ex.Message); // Bad Request
            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                int uId = _visitorsService.Delete(id);

                return StatusCode(200, uId); // Ok
            }
            catch (System.Exception ex)
            {
                 return StatusCode(400, ex.Message); // Bad Request
            }
        }
        

        [Authorize]
        [HttpPut("comein/{id}")]
        public IActionResult ComeIn([FromRoute] int id)
        {
            try
            {
                var uId = _visitorsService.ComeIn(id);

                return StatusCode(200, uId); // Ok
            }
            catch (System.Exception ex)
            {
                return StatusCode(400, ex.Message); // Bad Request
            }
        }


        [Authorize]
        [HttpPut("comeout/{id}")]
        public IActionResult ComeOut([FromRoute] int id)
        {
            try
            {
                var uId = _visitorsService.ComeOut(id);

                return StatusCode(200, uId); // Ok
            }
            catch (System.Exception ex)
            {
                return StatusCode(400, ex.Message); // Bad Request
            }
        }
    }
}