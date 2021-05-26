using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using backend.interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {

        private IConfiguration _config { get; }
        private readonly ApiDBContext _context;

        private readonly IFunction _service;



        public PageController(IConfiguration config, ApiDBContext context, IFunction service)
        {
            _config = config;
            _context = context;
            _service = service;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult header()
        {
            var table = _context.Categories;

            try
            {
                var items = table.OrderBy(r => r.CategoryId).Select(r => new
                {
                    Id = r.CategoryId,
                    Name = r.CategoryName,
                }).ToList();
                var len = items.Count();

                return Ok(new
                {
                    status = 200,
                    message = "success",
                    items,
                    length = len,

                });
            }
            catch (Exception ex)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), new
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message
                });
            }
        }


    }
}