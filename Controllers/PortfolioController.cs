using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using backend.interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers {
    [Authorize]
    [Route("api/[controller]")]

     public class PortfolioController : ControllerBase {
        private IConfiguration _config { get; }
        private readonly ApiDBContext _context;

        private readonly IFunction _service;



        public PortfolioController(IConfiguration config, ApiDBContext context, IFunction service)
        {
            _config = config;
            _context = context;
            _service = service;
        }

            public class FileUpload
        {
            public IFormFile FormFile { get; set; }
        }

        public class FileUploadList
        {
            public IFormFile FormFile { get; set; }
            public List<IFormFile> FormFileMulti { get; set; }

        }

        [HttpPost("")]
        public IActionResult portfolio()
        {
            var table = _context.Portfolios;


            try
            {
                var items = table
            .Include(p => p.Machine) // JOIN ตาราง Machine
            .OrderBy(r => r.PortfolioId)
            .Select(p => new 
            {
                p.PortfolioId,
                p.Seo,
                p.Title,
                p.FileImage,
                p.LocalImage,
                p.CreateUser,
                p.CreateDate,
                p.EditUser,
                p.EditDate,
                MachineName = p.Machine != null ? p.Machine.MachineName : null // ดึงชื่อเครื่องจาก Machine
            })
            .ToList();

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

        [HttpPost("[action]")]
          public IActionResult getPortfolioById(string id)
        {
            var table = _context.Portfolios;
            try
            {
                var items = table.Where(r => r.PortfolioId == id).First();


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


          [HttpPost("[action]")]
          public IActionResult addPortfolio(string machineId,string user,string title,string seo,[FromForm] FileUploadList file)
        {
            var table = _context.Portfolios;
            var tableImages = _context.Imageportfolios;
            try
            {
                var PortfolioFindId = table.OrderByDescending(u => u.PortfolioId).FirstOrDefault();
                var number = _service.GenID(PortfolioFindId != null ? PortfolioFindId.PortfolioId : "", "P");

                 var imageFileName = file.FormFile != null ? number + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolio", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                }

                 table.Add(
                       new Portfolio
                       {
                           MachineId = machineId, 
                           PortfolioId = number, 
                           Seo = seo,
                           Title = title,
                           CreateDate = DateTime.Now,
                           CreateUser = user,
                           FileImage = imageFileName,
                           LocalImage = file.FormFile != null ? "/portfolio/" + imageFileName : null
                       }
                   );
                _context.SaveChanges();

                 ///////////////////////////// portfolio  รูปภาพ List /////////////////////// 


                if (file.FormFileMulti != null)
                {

                    foreach (var item in file.FormFileMulti)
                    {
                        var imageID = tableImages.OrderByDescending(u => u.ImageId).FirstOrDefault();
                        var numberImage = _service.GenID(imageID != null ? imageID.ImageId : "", "A");

                        var imageFileMultiName = numberImage + DateTime.Now.ToString("ddMMyy_HHmmss") + item.FileName.Substring(item.FileName.LastIndexOf("."));

                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolioList", imageFileMultiName);
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        tableImages.Add(
                           new Imageportfolio
                           {
                               PortfolioId = number,
                               ImageId = numberImage,
                               FileName = imageFileMultiName,
                               Local = "/portfolioList/" + imageFileMultiName
                           }
                       );
                        _context.SaveChanges();
                    }
                }


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


        [HttpDelete("[action]")]
        public IActionResult deletePortfolio(string id){
            var table = _context.Portfolios;
            var tableImages = _context.Imageportfolios;

            try
            {

            var items = table.Where(r => r.PortfolioId == id).First();
            var itemsImages = tableImages.Where(r => r.PortfolioId == id).ToList();

             if (items.FileImage != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolio", items.FileImage);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

              if (itemsImages.Count() > 0)
                {
                    foreach (var item in itemsImages)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolioList", item.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }

                 table.Remove(items);
                 if (itemsImages.Count() > 0)
                {

                    foreach (var item in itemsImages)
                    {
                        tableImages.Remove(item);
                    }
                }

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


        [HttpPost("[action]")]
         public IActionResult updatePortfolio(string id,string machineId,string user,string title,string seo,[FromForm] FileUploadList file){

            var table = _context.Portfolios;
            var tableImages = _context.Imageportfolios;

            try
            {
                var items = table.Where(r => r.PortfolioId == id).First();

                 var imageFileName = file.FormFile != null ? id + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (items.FileImage != null && file.FormFile != null)
                {
                    string path_delete = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolio", items.FileImage);
                    if (System.IO.File.Exists(path_delete))
                    {
                        System.IO.File.Delete(path_delete);
                    }

                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolio", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/portfolio/" + imageFileName;
                    }
                }
                else if (items.FileImage == null && file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolio", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/portfolio/" + imageFileName;
                    }
                }

                items.MachineId = machineId;
                items.Seo = seo;
                items.Title = title;
                items.EditDate = DateTime.Now;
                items.EditUser = user;
                _context.SaveChanges();

                 if (tableImages.Count() > 0)
                {
                    foreach (var item in tableImages)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolioList", item.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }

                _context.SaveChanges();


                 ///////////////////////////// machine  รูปภาพ List /////////////////////// 


                if (file.FormFileMulti != null)
                {

                    foreach (var item in file.FormFileMulti)
                    {
                        var imageID = tableImages.OrderByDescending(u => u.ImageId).FirstOrDefault();
                        var numberImage = _service.GenID(imageID != null ? imageID.ImageId : "", "A");

                        var imageFileMultiName = numberImage + DateTime.Now.ToString("ddMMyy_HHmmss") + item.FileName.Substring(item.FileName.LastIndexOf("."));

                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/portfolioList", imageFileMultiName);
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        tableImages.Add(
                           new Imageportfolio
                           {
                               PortfolioId = id,
                               ImageId = numberImage,
                               FileName = imageFileMultiName,
                               Local = "/portfolioList/" + imageFileMultiName
                           }
                       );
                        _context.SaveChanges();
                    }
                }

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

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult portfolioGroup()
        {
        try
        {
        var groupedItems = _context.Portfolios
            .Include(p => p.Machine) // JOIN ตาราง Machine
            .Where(p => p.Machine != null) // ป้องกัน Machine เป็น null
            .GroupBy(p => p.MachineId) // Group By MachineId
            .Select(g => new
            {
                MachineId = g.Key, // ใช้ Key เป็น MachineId
                g.First().Machine.MachineName, // ดึงชื่อเครื่องจาก Machine
                g.First().Machine.LocalImage,
                  g.First().Machine.FileImage,
                enID = _service.encoding(g.Key),

            });

             return Ok(new
            {
            status = 200,
            message = "success",
            items = groupedItems
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

         public IActionResult portfolioByMachineId(string id){
            var table = _context.Portfolios;
            var decodeID = _service.decoding(id);
            try
            {
                  var items = table
                  .Include(p => p.Machine)
                  .Where(r => r.MachineId == decodeID)
                  .OrderBy(r => r.PortfolioId)
                  .Select(p => new 
            {
                p.Title,
                p.FileImage,
                p.LocalImage,
                 enID = _service.encoding(p.PortfolioId),
                MachineName = p.Machine != null ? p.Machine.MachineName : null // ดึงชื่อเครื่องจาก Machine
            })
                  .ToList();

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

         public IActionResult portfolioById(string id){
            var table = _context.Portfolios;
            var tableImage = _context.Imageportfolios;
            var decodeID = _service.decoding(id);
            try
            {
                  var items = table
                  .Where(r => r.PortfolioId == decodeID)
                  .OrderBy(r => r.PortfolioId)
                  .Select(p => new 
            {
                p.Seo,
                p.Title,
                p.FileImage,
                p.LocalImage,
                imageList = tableImage.Where(r => r.PortfolioId == decodeID).ToList(),
             
            })
                  .ToList();

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



        }
}