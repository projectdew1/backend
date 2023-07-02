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

    public class BlogController : ControllerBase
    {

        private IConfiguration _config { get; }
        private readonly ApiDBContext _context;

        private readonly IFunction _service;



        public BlogController(IConfiguration config, ApiDBContext context, IFunction service)
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
            public List<IFormFile> FormFileMulti { get; set; }

        }

        [HttpPost("[action]")]
        // [AllowAnonymous]
        public IActionResult addPhoto([FromForm] FileUploadList file)
        {
            var tableImages = _context.Photoalls;
            try
            {
                if (file.FormFileMulti != null)
                {

                    foreach (var item in file.FormFileMulti)
                    {
                        var imageID = tableImages.OrderByDescending(u => u.IdPhoto).FirstOrDefault();
                        var numberImage = _service.GenID(imageID != null ? imageID.IdPhoto : "", "A");

                        var imageFileMultiName = numberImage + DateTime.Now.ToString("ddMMyy_HHmmss") + item.FileName.Substring(item.FileName.LastIndexOf("."));

                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/photo", imageFileMultiName);
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        tableImages.Add(
                           new Photoall
                           {
                               IdPhoto = numberImage,
                               FileName = imageFileMultiName,
                               Local = "/photo/" + imageFileMultiName
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
        // [AllowAnonymous]
        public IActionResult deletePhoto(string id)
        {
            var tableImages = _context.Photoalls;
            try
            {
                var items = tableImages.Where(r => r.IdPhoto == id).First();

                if (items.FileName != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/photo", items.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                tableImages.Remove(items);
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
        public IActionResult photoall()
        {
            var table = _context.Photoalls;

            try
            {
                var items = table.OrderByDescending(r => r.IdPhoto).ToList();

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
        // [AllowAnonymous]
        public IActionResult contentBlog()
        {
            var table = _context.Blogs;
            var tableImage = _context.Imageblogs;


            try
            {
                var items = table.OrderByDescending(r => r.CreateDate).Select(r => new
                {
                    r.BlogId,
                    r.Title,
                    r.BlogSeo,
                    // r.Content,
                    r.CreateDate,
                    r.CreateUser,
                    r.EditDate,
                    r.EditUser,
                    enID = _service.encoding(r.BlogId),
                    Local = tableImage.Where(row => row.BlogId == r.BlogId).Select(i => i.Local).First(),
                    FileName = tableImage.Where(row => row.BlogId == r.BlogId).Select(i => i.FileName).First(),
                    ImageId = tableImage.Where(row => row.BlogId == r.BlogId).Select(i => i.ImageId).First(),
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
        // [AllowAnonymous]
        public IActionResult idBlog(string id)
        {
            var table = _context.Blogs;
            var tableImage = _context.Imageblogs;
            var deID = _service.decoding(id);


            try
            {
                var items = table.Where(r => r.BlogId == deID).Select(r => new
                {
                    r.BlogId,
                    r.Title,
                    r.BlogSeo,
                    r.Content,
                    r.CreateDate,
                    r.CreateUser,
                    r.EditDate,
                    r.EditUser,
                    Local = tableImage.Where(row => row.BlogId == r.BlogId).Select(i => i.Local).First(),
                    FileName = tableImage.Where(row => row.BlogId == r.BlogId).Select(i => i.FileName).First(),
                    ImageId = tableImage.Where(row => row.BlogId == r.BlogId).Select(i => i.ImageId).First(),
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


        [HttpDelete("[action]")]
        // [AllowAnonymous]
        public IActionResult deleteBlog(string id)
        {
            var tableBlogs = _context.Blogs;
            var tableImages = _context.Imageblogs;
            try
            {
                var itemsImages = tableImages.Where(r => r.BlogId == id).First();
                var items = tableBlogs.Where(r => r.BlogId == id).First();
                if (itemsImages.FileName != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", itemsImages.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                tableImages.Remove(itemsImages);
                tableBlogs.Remove(items);
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
        public IActionResult addBlog(
            string seo,
            string user,
            string title,
            string content,
            [FromForm] FileUpload file
            )
        {
            var tableBlogs = _context.Blogs;
            var tableImages = _context.Imageblogs;
            try
            {
                var id = tableBlogs.OrderByDescending(u => u.BlogId).FirstOrDefault();
                var number = _service.GenID(id != null ? id.BlogId : "", "B");

                var imageFileName = file.FormFile != null ? number + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }

                    var idImages = tableImages.OrderByDescending(u => u.ImageId).FirstOrDefault();
                    var numberImages = _service.GenID(idImages != null ? idImages.ImageId : "", "C");
                    //content blog images
                    var itemsImage = tableImages.Add(new Imageblog
                    {
                        BlogId = number,
                        ImageId = numberImages,
                        FileName = imageFileName,
                        Local = "/blog/" + imageFileName
                    });
                }

                var items = tableBlogs.Add(
                       new Blog
                       {
                           BlogId = number,
                           Title = title,
                           Content = content,
                           BlogSeo = seo,
                           CreateDate = DateTime.Now,
                           CreateUser = user,
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
        // [AllowAnonymous]
        public IActionResult updateBlog(
           string id,
           string seo,
           string user,
           string title,
           string content,
           [FromForm] FileUpload file
           )
        {
            var tableBlogs = _context.Blogs;
            var tableImages = _context.Imageblogs;
            try
            {
                var imageFileName = file.FormFile != null ? id + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;
                var checkImage = tableImages.Where(r => r.BlogId == id).ToArray().Length;



                if (checkImage > 0 && file.FormFile != null)
                {
                    var itemsImage = tableImages.Where(r => r.BlogId == id).First();
                    string path_delete = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", itemsImage.FileName);
                    if (System.IO.File.Exists(path_delete))
                    {
                        System.IO.File.Delete(path_delete);
                    }

                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        itemsImage.FileName = imageFileName;
                        itemsImage.Local = "/blog/" + imageFileName;
                    }
                }
                else if (checkImage == 0 && file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/blog", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }

                    var idImages = tableImages.OrderByDescending(u => u.ImageId).FirstOrDefault();
                    var numberImages = _service.GenID(idImages != null ? idImages.ImageId : "", "C");
                    //content blog images
                    var itemImage = tableImages.Add(new Imageblog
                    {
                        BlogId = id,
                        ImageId = numberImages,
                        FileName = imageFileName,
                        Local = "/blog/" + imageFileName
                    });
                }

                var items = tableBlogs.Where(r => r.BlogId == id).First();
                items.Title = title;
                items.BlogSeo = seo;
                items.Content = content;
                items.EditDate = DateTime.Now;
                items.EditUser = user;

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



        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult GetTModels()
        {
            return Ok("authorization success");
        }
    }
}