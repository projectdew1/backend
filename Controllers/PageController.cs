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
            var tableMachine = _context.Machines;

            try
            {
                var items = table.OrderBy(r => r.CategoryId).Select(r => new
                {
                    Id = r.CategoryId,
                    Name = r.CategoryName,
                    enID = _service.encoding(r.CategoryId),
                    r.LocalImage,
                    r.FileImage,
                    Items = tableType.Where(row => row.CategoryId == r.CategoryId).Count(),
                    product = (from t1 in tableType.Where(row => row.CategoryId == r.CategoryId)
                               join t2 in tableMachine on t1.TypeId equals t2.TypeId
                               select t2).Count()
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
            var decodeID = _service.decoding(name);

            try
            {
                var cateName = table.Where(r => r.CategoryId == decodeID).Select(r => r.CategoryName).First();
                var seo = table.Where(r => r.CategoryId == decodeID).Select(r => r.Seo).First();
                var ifName = table.Where(r => r.CategoryId == decodeID).Select(r => r.CategoryName).First();
                var items = tableType.Where(r => r.CategoryId == decodeID).Select(r => new
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
                        row.Soldout,
                        id = _service.encoding(row.MachineId)
                    }).ToList()
                }).ToList();



                return Ok(new
                {
                    status = 200,
                    message = "success",
                    name = cateName,
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

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult product()
        {
            var table = _context.Machines;

            try
            {
                var items = table.OrderBy(r => r.MachineId).Select(r => new
                {
                    r.MachineId,
                    id = _service.encoding(r.MachineId)
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
        public IActionResult idProduct(string id)
        {
            var table = _context.Machines; // เครื่องจักร อันเดียว First  = รูปภาพ อันเดียว
            var tableDetail = _context.Detailmachines; // คุณสมบัติ
            var tableDetailTech = _context.Detailtechmachines; // คุณสมบัติทางเทคนิค
            var tableExplain = _context.Explaimmachines; // คำอธิบาย อันเดียว First
            var tableImages = _context.Imagemachines; // รูปภาพ  = รูปภาพ หลาย
            var tableVideos = _context.Videomachines; // วีดีโอ
            var tableManual = _context.Manualmachines; // คู่มือ
            var tableType = _context.Typemachines; // 
            var tableTech = _context.Technicallies; // 
            var decodeID = _service.decoding(id);

            try
            {
                var seo = table.Where(r => r.MachineId == decodeID).Select(r => r.MachineSeo).First();
                var items = table.Where(r => r.MachineId == decodeID).Select(r => new
                {
                    r.CreateDate,
                    r.CreateUser,
                    r.EditDate,
                    r.EditUser,
                    r.Discount,
                    r.FileImage,
                    r.ItemsCode,
                    r.LocalImage,
                    r.MachineId,
                    r.MachineName,
                    r.MachineSeo,
                    r.Price,
                    r.Soldout,
                    r.TypeId,
                    Type = tableType.Where(row => row.TypeId == r.TypeId).Select(r => new
                    {
                        r.Category.CategoryName,
                        idCategory = _service.encoding(r.Category.CategoryId),
                        r.TypeName
                    }).FirstOrDefault(),
                    // DetailTech = tableDetailTech.Where(r => r.MachineId == decodeID).ToList(),
                    DetailTech = (from t1 in tableDetailTech.Where(row => row.MachineId == decodeID)
                                  join t2 in tableTech on t1.TechnicallyId equals t2.TechnicallyId
                                  select new { t1.DetailTech, t1.DetailTechMachineId, TechDetail = t1.TechnicallyId, t2.TechnicallyId, t2.TechnicallyName }).ToList(),
                    Detail = tableDetail.Where(r => r.MachineId == decodeID).ToList(),
                    Explain = tableExplain.Where(r => r.MachineId == decodeID).ToList(),
                    Image = tableImages.Where(r => r.MachineId == decodeID).ToList(),
                    Manual = tableManual.Where(r => r.MachineId == decodeID).ToList(),
                    Video = tableVideos.Where(r => r.MachineId == decodeID).Select(r => new
                    {
                        r.Link,
                        LinkMap = _service.covertLink(r.Link),
                    }).ToList(),

                }).First();


                return Ok(new
                {
                    status = 200,
                    message = "success",
                    items,
                    seo,



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