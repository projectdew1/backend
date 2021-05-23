using System;
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

    public class ContactController : ControllerBase
    {

        private IConfiguration _config { get; }
        private readonly ApiDBContext _context;

        private readonly IFunction _service;



        public ContactController(IConfiguration config, ApiDBContext context, IFunction service)
        {
            _config = config;
            _context = context;
            _service = service;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult addContact(string name, string mail, string tel, string title, string detail)
        {
            var table = _context.Contacts;

            try
            {
                var items = table.Add(
                    new Contact
                    {
                        ContactName = name,
                        ContactMail = mail,
                        ContactTel = tel,
                        ContactTitle = title,
                        ContactDetail = detail,
                        Status = 0,
                        CreateDate = DateTime.Now,
                    }
                );
                _context.SaveChanges();
                return Ok(new
                {
                    status = 200,
                    message = "success",
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

        [HttpPost("")]
        public IActionResult contact()
        {
            var table = _context.Contacts;

            try
            {
                var items = table.Select(r => new
                {
                    r.ContactId,
                    r.ContactName,
                    r.ContactTitle,
                    r.ContactDetail,
                    r.ContactMail,
                    r.ContactTel,
                    r.Status,
                    r.CreateDate,
                    Statusname = r.Status == 0 ? "ยังไม่อ่าน" : "อ่านแล้ว",
                }).ToList();

                var datanoRead = table.Select(r => new
                {
                    r.ContactId,
                    r.ContactName,
                    r.ContactTitle,
                    r.ContactDetail,
                    r.ContactMail,
                    r.ContactTel,
                    r.Status,
                    r.CreateDate,
                    Statusname = r.Status == 0 ? "ยังไม่อ่าน" : "อ่านแล้ว",
                }).Where(r => r.Status == 0).ToList();

                var dataRead = table.Select(r => new
                {
                    r.ContactId,
                    r.ContactName,
                    r.ContactTitle,
                    r.ContactDetail,
                    r.ContactMail,
                    r.ContactTel,
                    r.Status,
                    r.CreateDate,
                    Statusname = r.Status == 0 ? "ยังไม่อ่าน" : "อ่านแล้ว",
                }).Where(r => r.Status == 1).ToList();

                var CnoRead = table.Where(r => r.Status == 0).Count();
                var CRead = table.Where(r => r.Status == 1).Count();

                return Ok(new
                {
                    status = 200,
                    message = "success",
                    items = new
                    {
                        data = items,
                        dataRead,
                        datanoRead,
                        read = CRead,
                        noRead = CnoRead,
                    }
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


        [HttpPut("[action]")]
        public IActionResult updateRead(int id, int status)
        {
            var table = _context.Contacts;

            try
            {
                var items = table.Where(r => r.ContactId == id).First();

                items.Status = status;
                _context.SaveChanges();

                return Ok(new
                {
                    status = 200,
                    message = "success"
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


        [HttpDelete("[action]")]

        public IActionResult deleteContact(int id)
        {
            var table = _context.Contacts;
            try
            {
                var items = table.Where(r => r.ContactId == id).First();
                table.Remove(items);
                _context.SaveChanges();
                return Ok(new
                {
                    status = 200,
                    message = "success"
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



        [HttpGet("")]
        // [AllowAnonymous]
        public IActionResult GetTModel()
        {
            return Ok("authorization success");
        }
    }
}