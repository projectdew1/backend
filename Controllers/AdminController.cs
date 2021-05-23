
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

    public class AdminController : ControllerBase
    {

        private IConfiguration _config { get; }

        private readonly ApiDBContext _context;

        private readonly IFunction _service;



        public AdminController(IConfiguration config, ApiDBContext context, IFunction service)
        {
            _config = config;
            _context = context;
            _service = service;


        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            var user = _service.AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = _service.GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }

        [HttpPost("[action]")]
        public IActionResult user(string callBy)
        {
            var table = _context.Users;
            try
            {
                var items = table.Select(r => new
                {
                    r.Username,
                    r.LastLogin,
                    r.CreateDate,
                    r.CreateUser,
                    callBy
                }).ToList();

                return Ok(new
                {
                    status = 200,
                    message = "success",
                    items
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

        [HttpPost("[action]")]
        public IActionResult addUser(string username, string pass, string user)
        {
            var table = _context.Users;
            var salt = _service.PasswordSalt();
            var hash = _service.Encrypt(pass, salt);
            try
            {
                var items = table.Add(
                    new User
                    {

                        Username = username,
                        Password = hash,
                        Salt = salt,
                        CreateDate = DateTime.Now,
                        CreateUser = user
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

        [HttpPut("[action]")]
        public IActionResult change(string username, string pass)
        {
            var table = _context.Users;
            var salt = _service.PasswordSalt();
            var hash = _service.Encrypt(pass, salt);
            try
            {
                var items = table.Where(r => r.Username == username).First();
                items.Password = hash;
                items.Salt = salt;
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


        [HttpPut("[action]")]
        public IActionResult reset(string username)
        {
            var table = _context.Users;
            var salt = _service.PasswordSalt();
            var hash = _service.Encrypt("9999", salt);
            try
            {
                var items = table.Where(r => r.Username == username).First();
                items.Password = hash;
                items.Salt = salt;
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

        public IActionResult deleteUser(string username)
        {
            var table = _context.Users;
            try
            {
                var items = table.Where(r => r.Username == username).First();
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

