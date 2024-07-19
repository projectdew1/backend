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
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{

    [Authorize]
    [Route("api/[controller]")]

    public class NewsController : ControllerBase
    {

        private IConfiguration _config { get; }
        private readonly ApiDBContext _context;

        private readonly IFunction _service;



        public NewsController(IConfiguration config, ApiDBContext context, IFunction service)
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

         public class DataUploadFile
        {
            public string Content { get; set; }
            public IFormFile FormFile { get; set; }
            public List<IFormFile> FormFileMulti { get; set; }

        }


        [HttpGet("[action]")]
        public IActionResult getTypeNews()
        {
            var table = _context.Typenews;

            try
            {
                var items = table.OrderBy(r => r.TypeNewsId).Select(r => new
                {
                    r.TypeNewsId,
                    TypeNews = r.TypeNews1
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


        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult getNews(int pageNumber = 1, int pageSize = 10)
        {
            var table = _context.News;
            var tableType = _context.Typenews;
            try
            {
                var totalItems = table.Count();
                var items = table.OrderBy(r => r.NewsId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Select(r => new
                {
                    id = _service.encoding(r.NewsId),
                    r.Title,
                    r.Content,
                    r.TypeNewsId,
                    TypeNews = tableType.Where(row => row.TypeNewsId == r.TypeNewsId).Select(e => e.TypeNews1).First(),
                    r.CreateDate,
                    r.CreateUser,
                    r.EditDate,
                    r.EditUser,
                    r.LocalImage,
                    r.FileImage,
                })
                .ToList();

                return Ok(new
                {
                    status = 200,
                    message = "success",
                    items,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
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
        public IActionResult deleteNews(string id)
        {
            var table = _context.News;
            var tableImages = _context.Imagenews;

            var items = table.Where(r => r.NewsId == id).First();
            var itemsImages = tableImages.Where(r => r.NewsId == id).ToList();

            try
            {
                if (items.FileImage != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", items.FileImage);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                if (itemsImages.Count() > 0)
                {
                    foreach (var item in itemsImages)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", item.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        tableImages.Remove(item);
                    }


                }

                table.Remove(items);
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
        public IActionResult getNewsAll()
        {
            var table = _context.News;
            var tableType = _context.Typenews;

            try
            {
                var items = table.OrderBy(r => r.NewsId).Select(r => new
                {
                    r.NewsId,
                    id = _service.encoding(r.NewsId),
                    r.Title,
                    r.Content,
                    r.TypeNewsId,
                    r.NewsSeo,
                    TypeNews = tableType.Where(row => row.TypeNewsId == r.TypeNewsId).Select(e => e.TypeNews1).First(),
                    r.CreateDate,
                    r.CreateUser,
                    r.EditDate,
                    r.EditUser,
                    r.LocalImage,
                    r.FileImage,
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

        [HttpPost("[action]")]
        
        public IActionResult addNews(string seo, string typeNewsId, string title, string user, [FromForm] DataUploadFile file)
        {
            var table = _context.News;
            var tableType = _context.Typenews;
            var tableImages = _context.Imagenews;
            try
            {
                if (seo.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูล SEO !",
                    });
                }

                if (title.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูลหัวข้อ !",
                    });
                }

                if (file.Content.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูลเนื้อหา !",
                    });
                }

                if (typeNewsId.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูลประเภท !",
                    });
                }

                if(file.FormFile == null){
                      return Ok(new
                    {
                        status = 200,
                        message = "กรุณาเพิ่มรูปภาพปก !",
                    });
                }

                var newsID = table.OrderByDescending(u => u.NewsId).FirstOrDefault();

                var number = _service.GenID(newsID != null ? newsID.NewsId : "", "N");

                var imageFileName = file.FormFile != null ? number + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                table.Add(
                   new News
                   {
                       NewsId = number,
                       NewsSeo = seo,
                       Title = title,
                       Content = file.Content,
                       TypeNewsId = typeNewsId,
                       CreateDate = DateTime.Now,
                       CreateUser = user,
                       FileImage = imageFileName,
                       LocalImage = file.FormFile != null ? "/blog/" + imageFileName : null
                   }
               );

                if (file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                }

                _context.SaveChanges();

                if (file.FormFileMulti != null)
                {

                    foreach (var item in file.FormFileMulti)
                    {
                        var imageID = tableImages.OrderByDescending(u => u.ImagenewsId).FirstOrDefault();
                        var numberImage = _service.GenID(imageID != null ? imageID.ImagenewsId : "", "D");

                        var imageFileMultiName = numberImage + DateTime.Now.ToString("ddMMyy_HHmmss") + item.FileName.Substring(item.FileName.LastIndexOf("."));

                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileMultiName);
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        tableImages.Add(
                           new Imagenews
                           {
                               NewsId = number,
                               ImagenewsId = numberImage,
                               FileName = imageFileMultiName,
                               Local = "/blog/" + imageFileMultiName
                           }
                       );
                        _context.SaveChanges();
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

        [HttpPut("[action]")]
        public IActionResult updateNews(string id, string seo, string typeNewsId, string title, string user, [FromForm] DataUploadFile file)
        {
            var table = _context.News;
            var tableType = _context.Typenews;
            var tableImages = _context.Imagenews;

            try
            {
                var items = table.Where(r => r.NewsId == id).First();
                var itemsImages = tableImages.Where(r => r.NewsId == id).ToList();

                if (seo.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูล SEO !",
                    });
                }

                if (title.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูลหัวข้อ !",
                    });
                }

                if (file.Content.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูลเนื้อหา !",
                    });
                }

                if (typeNewsId.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกข้อมูลประเภท !",
                    });
                }

                if (items.FileImage == null && file.FormFile == null){
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณาเพิ่มรูปภาพปก !",
                    });
                }

                var imageFileName = file.FormFile != null ? id + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (items.FileImage != null && file.FormFile != null)
                {
                    string path_delete = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", items.FileImage);
                    if (System.IO.File.Exists(path_delete))
                    {
                        System.IO.File.Delete(path_delete);
                    }

                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/blog/" + imageFileName;
                    }
                }
                else if (items.FileImage == null && file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/blog/" + imageFileName;
                    }
                }

                items.NewsSeo = seo;
                items.Title = title;
                items.Content = file.Content;
                items.TypeNewsId = typeNewsId;
                items.EditDate = DateTime.Now;
                items.EditUser = user;
                _context.SaveChanges();

                /// image
                if (itemsImages.Count() > 0)
                {
                    foreach (var item in itemsImages)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", item.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        tableImages.Remove(item);
                    }
                    _context.SaveChanges();
                }

                if (file.FormFileMulti != null)
                {

                    foreach (var item in file.FormFileMulti)
                    {
                        var imageID = tableImages.OrderByDescending(u => u.ImagenewsId).FirstOrDefault();
                        var numberImage = _service.GenID(imageID != null ? imageID.ImagenewsId : "", "D");

                        var imageFileMultiName = numberImage + DateTime.Now.ToString("ddMMyy_HHmmss") + item.FileName.Substring(item.FileName.LastIndexOf("."));

                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileMultiName);
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        tableImages.Add(
                           new Imagenews
                           {
                               NewsId = id,
                               ImagenewsId = numberImage,
                               FileName = imageFileMultiName,
                               Local = "/blog/" + imageFileMultiName
                           }
                       );
                        _context.SaveChanges();
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
        // [AllowAnonymous]
        public IActionResult findNews(string id)
        {
            var decodeID = id;
            var table = _context.News;
            var tableType = _context.Typenews;
            var tableImage = _context.Imagenews;

            try
            {
                var items = table.Where(r => r.NewsId == decodeID)
                .Select(r =>
                new
                {
                    r.Title,
                    r.Content,
                    r.TypeNewsId,
                    r.NewsSeo,
                    TypeNews = tableType.Where(row => row.TypeNewsId == r.TypeNewsId).Select(e => e.TypeNews1).First(),
                    r.CreateDate,
                    r.CreateUser,
                    r.EditDate,
                    r.EditUser,
                    r.LocalImage,
                    r.FileImage,
                    Image = tableImage.Where(r => r.NewsId == decodeID).ToList(),
                }).First();
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
        public IActionResult findNewsIdShow()

        {
            var table = _context.News;
            try
            {
                var items = table.Select(r =>
                new
                {
                    id = _service.encoding(r.NewsId),

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


        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult findNewsShow(string id)
        {
            var decodeID = _service.decoding(id);
            var table = _context.News;
            var tableType = _context.Typenews;
            var tableImage = _context.Imagenews;

            try
            {
                var items = table.Where(r => r.NewsId == decodeID)
                .Select(r =>
                new
                {
                    r.Title,
                    r.Content,
                    r.TypeNewsId,
                    r.NewsSeo,
                    TypeNews = tableType.Where(row => row.TypeNewsId == r.TypeNewsId).Select(e => e.TypeNews1).First(),
                    r.CreateDate,
                    r.CreateUser,
                    r.EditDate,
                    r.EditUser,
                    r.LocalImage,
                    r.FileImage,
                    Image = tableImage.Where(r => r.NewsId == decodeID).ToList(),
                }).First();
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