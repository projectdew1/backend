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

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult header()
        {
            var table = _context.Categories;
            var tableType = _context.Typemachines;

            try
            {
                var items = table.OrderBy(r => r.CategoryId).Select(r => new
                {
                    Id = r.CategoryId,
                    Name = r.CategoryName,
                    r.LocalImage,
                    r.FileImage,
                    Items = tableType.Where(row => row.CategoryId == r.CategoryId).Count()
                }).ToList();


                return Ok(new
                {
                    status = 200,
                    message = "success",
                    items,


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


        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult idHeader(string name)
        {
            var table = _context.Categories;
            var tableType = _context.Typemachines;
            var tableMachine = _context.Machines;

            try
            {
                var id = table.Where(r => r.CategoryName == name).Select(r => r.CategoryId).First();
                var seo = table.Where(r => r.CategoryName == name).Select(r => r.Seo).First();
                var ifName = table.Where(r => r.CategoryName == name).Select(r => r.CategoryName).First();
                var items = tableType.Where(r => r.CategoryId == id).Select(r => new
                {
                    r.TypeName,
                    r.TypeSeo,
                    Machine = tableMachine.Where(row => row.TypeId == r.TypeId).Select(row => new
                    {
                        row.MachineName,
                        row.LocalImage,
                        row.FileImage,
                        row.MachineId,
                        row.Price,
                        row.Discount,
                        row.Soldout
                    }).ToList()
                }).ToList();



                return Ok(new
                {
                    status = 200,
                    message = "success",
                    id,
                    seo = seo == "" || seo == null ? ifName : seo,
                    items,


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