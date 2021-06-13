using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using backend.interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]

    public class ProductController : ControllerBase
    {
        private IConfiguration _config { get; }
        private readonly ApiDBContext _context;
        private readonly IFunction _service;
        public static IWebHostEnvironment _environment;
        public ProductController(IConfiguration config, ApiDBContext context, IFunction service, IWebHostEnvironment environment)
        {
            _config = config;
            _context = context;
            _service = service;
            _environment = environment;
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

        public class TechnicalTtype
        {
            public string tech { get; set; }
            public string name { get; set; }
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult get()
        {
            return Ok(new
            {
                name = "xxx.png",
                status = "done",
                url = "xxx",
                thumbUrl = "xxx"
            }
            );
        }

        [HttpPost("")]
        public IActionResult product()
        {
            var table = _context.Categories;


            try
            {
                var items = table.OrderBy(r => r.CategoryId).ToList();

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
        public IActionResult idCategory(string id)
        {
            var table = _context.Categories;
            try
            {
                var items = table.Where(r => r.CategoryId == id).First();


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
        public IActionResult addCategory(string categoryName, string user, string seo, [FromForm] FileUpload file)
        {
            var table = _context.Categories;

            try
            {
                var id = table.OrderByDescending(u => u.CategoryId).FirstOrDefault();

                var number = _service.GenID(id != null ? id.CategoryId : "", "C");

                var duplicate = table.Where(r => r.CategoryName == categoryName).ToArray().Length;

                if (categoryName == null)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรอกชื่อประเภทผลิตภัณฑ์ !",
                    });
                }


                if (duplicate > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "ชื่อประเภทผลิตภัณฑ์ซ้ำ !",
                    });
                }

                var imageFileName = file.FormFile != null ? number + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (file.FormFile != null)
                {
                    // int index = file.FormFile.FileName.LastIndexOf(".");
                    // string typefile = file.FormFile.FileName.Substring(index);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/category", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                }

                var items = table.Add(
                       new Category
                       {
                           CategoryId = number,
                           CategoryName = categoryName,
                           Seo = seo,
                           CreateDate = DateTime.Now,
                           CreateUser = user,
                           FileImage = imageFileName,
                           LocalImage = file.FormFile != null ? "/category/" + imageFileName : null
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

        [HttpDelete("[action]")]
        public IActionResult deleteCategory(string id)
        {
            var table = _context.Categories;
            var Typemachine = _context.Typemachines;
            try
            {


                var items = table.Where(r => r.CategoryId == id).First();
                var check = Typemachine.Where(r => r.CategoryId == id).ToArray().Length;

                if (check == 0)
                {

                    if (items.FileImage != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/category", items.FileImage);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
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
                else
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณาลบหมวดหมู่ในประเภทนี้ทั้งหมด !",
                    });
                }
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
        public IActionResult update(string id, string categoryName, string user, string seo, [FromForm] FileUpload file)
        {
            var table = _context.Categories;

            try
            {
                var items = table.Where(r => r.CategoryId == id).First();
                var duplicate = table.Where(r => r.CategoryName == categoryName).ToArray().Length;

                if (categoryName == null)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรอกชื่อประเภทผลิตภัณฑ์ !",
                    });
                }

                if (items.CategoryName != categoryName)
                {
                    if (duplicate > 0)
                    {
                        return Ok(new
                        {
                            status = 200,
                            message = "ชื่อประเภทผลิตภัณฑ์ซ้ำ !",
                        });
                    }
                }

                var imageFileName = file.FormFile != null ? id + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (items.FileImage != null && file.FormFile != null)
                {
                    string path_delete = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/category", items.FileImage);
                    if (System.IO.File.Exists(path_delete))
                    {
                        System.IO.File.Delete(path_delete);
                    }

                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/category", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/category/" + imageFileName;
                    }
                }
                else if (items.FileImage == null && file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/category", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/category/" + imageFileName;
                    }
                }


                items.CategoryName = categoryName;
                items.Seo = seo;
                items.EditDate = DateTime.Now;
                items.EditUser = user;

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

        ///////////////// GROUP //////////////////////
        [HttpPost("[action]")]
        public IActionResult type()
        {
            var table = _context.Typemachines;
            var category = _context.Categories;
            var machine = _context.Machines;
            var Explaintype = _context.Explaintypes;


            try
            {
                var items = table.OrderBy(r => r.TypeId).Select(r =>
                    new
                    {
                        r.CategoryId,
                        r.CreateDate,
                        r.CreateUser,
                        r.EditDate,
                        r.EditUser,
                        r.FileImage,
                        r.LocalImage,
                        r.TypeId,
                        r.TypeName,
                        r.TypeSeo,
                        category = category.Where(row => row.CategoryId == r.CategoryId).Select(e => e.CategoryName).First(),
                        explaintypes = Explaintype.Where(row => row.TypeId == r.TypeId).Select(e => new
                        {
                            e.ExplainTypeId,
                            e.ExplainDetail
                        }).ToList(),
                        machines = machine.Where(row => row.TypeId == r.TypeId).Select(e => new
                        {
                            e.MachineId,
                            e.MachineName
                        }).ToList(),
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
        public IActionResult addType(string seo, string typeName, string categoryID, string user, [FromForm] FileUpload file)
        {
            var table = _context.Typemachines;

            try
            {

                var id = table.OrderByDescending(u => u.TypeId).FirstOrDefault();

                var number = _service.GenID(id != null ? id.TypeId : "", "G");

                var duplicate = table.Where(r => r.CategoryId == categoryID && r.TypeName == typeName).ToArray().Length;


                if (duplicate > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "ชื่อหมวดหมู่ผลิตภัณฑ์ซ้ำ !",
                    });
                }

                var imageFileName = file.FormFile != null ? number + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/group", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                }

                var items = table.Add(
                       new Typemachine
                       {
                           TypeId = number,
                           CategoryId = categoryID,
                           TypeName = typeName,
                           TypeSeo = seo,
                           CreateDate = DateTime.Now,
                           CreateUser = user,
                           FileImage = imageFileName,
                           LocalImage = file.FormFile != null ? "/group/" + imageFileName : null
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

        [HttpDelete("[action]")]
        public IActionResult deleteType(string id)
        {
            var table = _context.Typemachines;
            var machine = _context.Machines;
            try
            {
                var items = table.Where(r => r.TypeId == id).First();
                var check = machine.Where(r => r.TypeId == id).ToArray().Length;

                if (check == 0)
                {

                    if (items.FileImage != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/group", items.FileImage);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
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
                else
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณาลบเครื่องจักรในหใวดหมู่นี้ทั้งหมด !",
                    });
                }
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
        public IActionResult idType(string id)
        {
            var table = _context.Typemachines;
            try
            {
                var items = table.Where(r => r.TypeId == id).First();


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

        [HttpPut("[action]")]
        public IActionResult updateType(string seo, string typeName, string categoryID, string user, string id, [FromForm] FileUpload file)
        {
            var table = _context.Typemachines;

            try
            {

                var items = table.Where(r => r.TypeId == id).First();
                var duplicate = table.Where(r => r.CategoryId == categoryID && r.TypeName == typeName).ToArray().Length;


                if (items.TypeName != typeName && duplicate > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "ชื่อหมวดหมู่ผลิตภัณฑ์ซ้ำ !",
                    });
                }

                var imageFileName = file.FormFile != null ? id + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (items.FileImage != null && file.FormFile != null)
                {
                    string path_delete = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/group", items.FileImage);
                    if (System.IO.File.Exists(path_delete))
                    {
                        System.IO.File.Delete(path_delete);
                    }

                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/group", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/group/" + imageFileName;
                    }
                }
                else if (items.FileImage == null && file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/group", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/group/" + imageFileName;
                    }
                }



                items.TypeSeo = seo;
                items.TypeName = typeName;
                items.CategoryId = categoryID;
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



        ////////////// MACHNIE //////////////////////

        [HttpPost("[action]")]
        public IActionResult machine()
        {
            var table = _context.Machines;
            var tableType = _context.Typemachines;
            var tableCategory = _context.Categories;



            try
            {
                var items = table.OrderBy(r => r.MachineId).Select(r =>
               new
               {
                   r.MachineId,
                   r.MachineName,
                   r.ItemsCode,
                   r.MachineSeo,
                   r.Soldout,
                   r.Price,
                   r.Discount,
                   r.CreateDate,
                   r.CreateUser,
                   r.EditDate,
                   r.EditUser,
                   r.LocalImage,
                   r.FileImage,
                   r.TypeId,
                   TypeName = tableType.Where(row => row.TypeId == r.TypeId).Select(e => e.TypeName).First(),
                   CategoryName = tableCategory.Where(row => row.CategoryId == tableType.Where(row => row.TypeId == r.TypeId).Select(e => e.CategoryId).First()).Select(e => e.CategoryName).First(),
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
        public IActionResult addMachine(string seo, string typeID, string machineName,
        string machineModels, int price, int discount, bool soldout, string user, string explain, string detail, string manual, string videos, string technical, [FromForm] FileUploadList file
        )
        {
            var table = _context.Machines; // เครื่องจักร
            var tableDetail = _context.Detailmachines; // คุณสมบัติ
            var tableDetailTech = _context.Detailtechmachines; // คุณสมบัติทางเทคนิค
            var tableExplain = _context.Explaimmachines; // คำอธิบาย
            var tableImages = _context.Imagemachines; // รูปภาพ
            var tableVideos = _context.Videomachines; // วีดีโอ
            var tableManual = _context.Manualmachines; // คู่มือ

            try
            {


                var machineID = table.OrderByDescending(u => u.MachineId).FirstOrDefault();

                var number = _service.GenID(machineID != null ? machineID.MachineId : "", "M");

                var duplicate = table.Where(r => r.TypeId == typeID && r.MachineName == machineName).ToArray().Length;


                if (duplicate > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "ชื่อสินค้าหรือเครื่องจักรซ้ำ !",
                    });
                }

                ///////////////////////////// machine ///////////////////////
                var imageFileName = file.FormFile != null ? number + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machine", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                }

                table.Add(
                       new Machine
                       {
                           MachineId = number,
                           MachineSeo = seo,
                           MachineName = machineName,
                           Price = price,
                           Discount = discount,
                           Soldout = Convert.ToSByte(soldout),
                           ItemsCode = machineModels,
                           TypeId = typeID,
                           CreateDate = DateTime.Now,
                           CreateUser = user,
                           FileImage = imageFileName,
                           LocalImage = file.FormFile != null ? "/machine/" + imageFileName : null
                       }
                   );
                _context.SaveChanges();

                ///////////////////////////// machine Explain คำอธิบาย ///////////////////////

                var explainID = tableExplain.OrderByDescending(u => u.ExplainMachineId).FirstOrDefault();
                var numberExplain = _service.GenID(explainID != null ? explainID.ExplainMachineId : "", "X");

                tableExplain.Add(
                    new Explaimmachine
                    {
                        MachineId = number,
                        ExplainMachineId = numberExplain,
                        ExplainDetail = explain,
                        CreateDate = DateTime.Now,
                        CreateUser = user,
                    }
                );
                _context.SaveChanges();

                ///////////////////////////// machine Detail คุณสมบัติ List ///////////////////////               

                if (detail != null)
                {
                    var list = detail.Split(new char[] { ',' }).ToList();
                    foreach (var item in list)
                    {

                        var detailID = tableDetail.OrderByDescending(u => u.DetailMachineId).FirstOrDefault();
                        var numberDetail = _service.GenID(detailID != null ? detailID.DetailMachineId : "", "D");

                        tableDetail.Add(
                            new Detailmachine
                            {
                                MachineId = number,
                                DetailMachineId = numberDetail,
                                Detail = item,
                                CreateDate = DateTime.Now,
                                CreateUser = user,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine Manual คู่มือ List ///////////////////////               

                if (manual != null)
                {
                    var listmanual = manual.Split(new char[] { ',' }).ToList();
                    foreach (var item in listmanual)
                    {
                        var manualID = tableManual.OrderByDescending(u => u.ManualMachineId).FirstOrDefault();
                        var numberManual = _service.GenID(manualID != null ? manualID.ManualMachineId : "", "N");

                        tableManual.Add(
                            new Manualmachine
                            {
                                MachineId = number,
                                ManualMachineId = numberManual,
                                Manual = item,
                                CreateDate = DateTime.Now,
                                CreateUser = user,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine Videos วีดีโอ List ///////////////////////               

                if (videos != null)
                {
                    var listvideos = videos.Split(new char[] { ',' }).ToList();
                    foreach (var item in listvideos)
                    {
                        var videosID = tableVideos.OrderByDescending(u => u.VideoMachineId).FirstOrDefault();
                        var numberVideos = _service.GenID(videosID != null ? videosID.VideoMachineId : "", "V");

                        tableVideos.Add(
                            new Videomachine
                            {
                                MachineId = number,
                                VideoMachineId = numberVideos,
                                Link = item,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine Technical คุณสมบัติทางเทคนิค List /////////////////////// 

                if (technical != null)
                {
                    List<TechnicalTtype> parts = new List<TechnicalTtype>();
                    var stepA = technical.Split(new char[] { ',' }).ToList();
                    for (int i = 0; i < stepA.ToArray().Length; i++)
                    {
                        var item = stepA[i];
                        var stepB = item.Split(new char[] { '-' }).ToList();
                        var tech = stepB[0].ToString().Substring(5, stepB[0].Length - 5);
                        var name = stepB[1].ToString().Substring(5, stepB[1].Length - 5);
                        parts.Add(new TechnicalTtype() { tech = tech, name = name });
                    }

                    foreach (var item in parts)
                    {
                        var technicalID = tableDetailTech.OrderByDescending(u => u.DetailTechMachineId).FirstOrDefault();
                        var numberTechnical = _service.GenID(technicalID != null ? technicalID.DetailTechMachineId : "", "D");


                        tableDetailTech.Add(
                            new Detailtechmachine
                            {
                                MachineId = number,
                                DetailTechMachineId = numberTechnical,
                                DetailTech = item.name,
                                TechnicallyId = item.tech,
                                CreateDate = DateTime.Now,
                                CreateUser = user,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine  รูปภาพ List /////////////////////// 


                if (file.FormFileMulti != null)
                {

                    foreach (var item in file.FormFileMulti)
                    {
                        var imageID = tableImages.OrderByDescending(u => u.ImageMachineId).FirstOrDefault();
                        var numberImage = _service.GenID(imageID != null ? imageID.ImageMachineId : "", "I");

                        var imageFileMultiName = numberImage + DateTime.Now.ToString("ddMMyy_HHmmss") + item.FileName.Substring(item.FileName.LastIndexOf("."));

                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machineImage", imageFileMultiName);
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        tableImages.Add(
                           new Imagemachine
                           {
                               MachineId = number,
                               ImageMachineId = numberImage,
                               FileName = imageFileMultiName,
                               Local = "/machineImage/" + imageFileMultiName
                           }
                       );
                        _context.SaveChanges();
                    }
                }

                ///////////// RETURN /////////////
                return Ok(new
                {
                    status = 200,
                    message = "success",
                });
                ///////////// RETURN /////////////
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
        public IActionResult deleteMachine(string id)

        {
            var table = _context.Machines; // เครื่องจักร อันเดียว First  = รูปภาพ อันเดียว
            var tableDetail = _context.Detailmachines; // คุณสมบัติ
            var tableDetailTech = _context.Detailtechmachines; // คุณสมบัติทางเทคนิค
            var tableExplain = _context.Explaimmachines; // คำอธิบาย อันเดียว First
            var tableImages = _context.Imagemachines; // รูปภาพ  = รูปภาพ หลาย
            var tableVideos = _context.Videomachines; // วีดีโอ
            var tableManual = _context.Manualmachines; // คู่มือ
            try
            {
                // First อันเดียว
                var items = table.Where(r => r.MachineId == id).First();
                var itemsExplain = tableExplain.Where(r => r.MachineId == id).First();

                // ToList หลาย
                var itemsDetail = tableDetail.Where(r => r.MachineId == id).ToList();
                var itemsDetailTech = tableDetailTech.Where(r => r.MachineId == id).ToList();
                var itemsImages = tableImages.Where(r => r.MachineId == id).ToList();
                var itemsVideos = tableVideos.Where(r => r.MachineId == id).ToList();
                var itemsManual = tableManual.Where(r => r.MachineId == id).ToList();





                if (items.FileImage != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machine", items.FileImage);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                if (itemsImages.Count() > 0)
                {
                    foreach (var item in itemsImages)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machineImage", item.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }

                // First อันเดียว
                table.Remove(items);
                tableExplain.Remove(itemsExplain);

                // ToList หลาย
                if (itemsDetail.Count() > 0)
                {

                    foreach (var item in itemsDetail)
                    {
                        tableDetail.Remove(item);
                    }
                }

                if (itemsDetailTech.Count() > 0)
                {

                    foreach (var item in itemsDetailTech)
                    {
                        tableDetailTech.Remove(item);
                    }
                }

                if (itemsImages.Count() > 0)
                {

                    foreach (var item in itemsImages)
                    {
                        tableImages.Remove(item);
                    }
                }

                if (itemsVideos.Count() > 0)
                {

                    foreach (var item in itemsVideos)
                    {
                        tableVideos.Remove(item);
                    }
                }

                if (itemsManual.Count() > 0)
                {

                    foreach (var item in itemsManual)
                    {
                        tableManual.Remove(item);
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
        public IActionResult idMachine(string id)
        {

            var table = _context.Machines; // เครื่องจักร อันเดียว First  = รูปภาพ อันเดียว
            var tableDetail = _context.Detailmachines; // คุณสมบัติ
            var tableDetailTech = _context.Detailtechmachines; // คุณสมบัติทางเทคนิค
            var tableExplain = _context.Explaimmachines; // คำอธิบาย อันเดียว First
            var tableImages = _context.Imagemachines; // รูปภาพ  = รูปภาพ หลาย
            var tableVideos = _context.Videomachines; // วีดีโอ
            var tableManual = _context.Manualmachines; // คู่มือ

            try
            {
                var items = table.Where(r => r.MachineId == id).Select(r => new
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
                    DetailTech = tableDetailTech.Where(r => r.MachineId == id).ToList(),
                    Detail = tableDetail.Where(r => r.MachineId == id).ToList(),
                    Explain = tableExplain.Where(r => r.MachineId == id).ToList(),
                    Image = tableImages.Where(r => r.MachineId == id).ToList(),
                    Manual = tableManual.Where(r => r.MachineId == id).ToList(),
                    Video = tableVideos.Where(r => r.MachineId == id).ToList(),

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


        [HttpPost("[action]")]
        public IActionResult updateMachine(string seo, string typeID, string machineName,
                string machineModels, int price, int discount, bool soldout, string user, string explain, string detail, string manual, string videos, string technical, string id, [FromForm] FileUploadList file
                )
        {
            var table = _context.Machines; // เครื่องจักร อันเดียว First  = รูปภาพ อันเดียว
            var tableDetail = _context.Detailmachines; // คุณสมบัติ
            var tableDetailTech = _context.Detailtechmachines; // คุณสมบัติทางเทคนิค
            var tableExplain = _context.Explaimmachines; // คำอธิบาย อันเดียว First
            var tableImages = _context.Imagemachines; // รูปภาพ  = รูปภาพ หลาย
            var tableVideos = _context.Videomachines; // วีดีโอ
            var tableManual = _context.Manualmachines; // คู่มือ
            try
            {

                var items = table.Where(r => r.MachineId == id).First();
                var duplicate = table.Where(r => r.TypeId == typeID && r.MachineName == machineName).ToArray().Length;


                if (items.MachineName != machineName && duplicate > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "ชื่อสินค้าผลิตภัณฑ์ซ้ำ !",
                    });
                }

                var imageFileName = file.FormFile != null ? id + DateTime.Now.ToString("ddMMyy_HHmmss") + file.FormFile.FileName.Substring(file.FormFile.FileName.LastIndexOf(".")) : null;

                if (items.FileImage != null && file.FormFile != null)
                {
                    string path_delete = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machine", items.FileImage);
                    if (System.IO.File.Exists(path_delete))
                    {
                        System.IO.File.Delete(path_delete);
                    }

                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machine", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/machine/" + imageFileName;
                    }
                }
                else if (items.FileImage == null && file.FormFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machine", imageFileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                        items.FileImage = imageFileName;
                        items.LocalImage = "/machine/" + imageFileName;
                    }
                }



                items.MachineSeo = seo;
                items.MachineName = machineName;
                items.Price = price;
                items.Discount = discount;
                items.Soldout = Convert.ToSByte(soldout);
                items.ItemsCode = machineModels;
                items.TypeId = typeID;
                items.EditDate = DateTime.Now;
                items.EditUser = user;
                _context.SaveChanges();

                ////////// explain /////////
                var itemsExplain = tableExplain.Where(r => r.MachineId == id).First();
                itemsExplain.ExplainDetail = explain;
                itemsExplain.EditDate = DateTime.Now;
                itemsExplain.EditUser = user;
                _context.SaveChanges();

                ////////// remove /////////
                // ToList หลาย
                var itemsDetail = tableDetail.Where(r => r.MachineId == id).ToList();
                var itemsDetailTech = tableDetailTech.Where(r => r.MachineId == id).ToList();
                var itemsImages = tableImages.Where(r => r.MachineId == id).ToList();
                var itemsVideos = tableVideos.Where(r => r.MachineId == id).ToList();
                var itemsManual = tableManual.Where(r => r.MachineId == id).ToList();

                /// image
                if (itemsImages.Count() > 0)
                {
                    foreach (var item in itemsImages)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machineImage", item.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }

                if (itemsDetail.Count() > 0)
                {

                    foreach (var item in itemsDetail)
                    {
                        tableDetail.Remove(item);
                    }
                }

                if (itemsDetailTech.Count() > 0)
                {

                    foreach (var item in itemsDetailTech)
                    {
                        tableDetailTech.Remove(item);
                    }
                }

                if (itemsImages.Count() > 0)
                {

                    foreach (var item in itemsImages)
                    {
                        tableImages.Remove(item);
                    }
                }

                if (itemsVideos.Count() > 0)
                {

                    foreach (var item in itemsVideos)
                    {
                        tableVideos.Remove(item);
                    }
                }

                if (itemsManual.Count() > 0)
                {

                    foreach (var item in itemsManual)
                    {
                        tableManual.Remove(item);
                    }
                }

                _context.SaveChanges();
                ////////// remove /////////

                ///////////////////////////// machine Detail คุณสมบัติ List ///////////////////////               

                if (detail != null)
                {
                    var list = detail.Split(new char[] { ',' }).ToList();
                    foreach (var item in list)
                    {

                        var detailID = tableDetail.OrderByDescending(u => u.DetailMachineId).FirstOrDefault();
                        var numberDetail = _service.GenID(detailID != null ? detailID.DetailMachineId : "", "D");

                        tableDetail.Add(
                            new Detailmachine
                            {
                                MachineId = id,
                                DetailMachineId = numberDetail,
                                Detail = item,
                                EditDate = DateTime.Now,
                                EditUser = user,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine Manual คู่มือ List ///////////////////////               

                if (manual != null)
                {
                    var listmanual = manual.Split(new char[] { ',' }).ToList();
                    foreach (var item in listmanual)
                    {
                        var manualID = tableManual.OrderByDescending(u => u.ManualMachineId).FirstOrDefault();
                        var numberManual = _service.GenID(manualID != null ? manualID.ManualMachineId : "", "N");

                        tableManual.Add(
                            new Manualmachine
                            {
                                MachineId = id,
                                ManualMachineId = numberManual,
                                Manual = item,
                                EditDate = DateTime.Now,
                                EditUser = user,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine Videos วีดีโอ List ///////////////////////               

                if (videos != null)
                {
                    var listvideos = videos.Split(new char[] { ',' }).ToList();
                    foreach (var item in listvideos)
                    {
                        var videosID = tableVideos.OrderByDescending(u => u.VideoMachineId).FirstOrDefault();
                        var numberVideos = _service.GenID(videosID != null ? videosID.VideoMachineId : "", "V");

                        tableVideos.Add(
                            new Videomachine
                            {
                                MachineId = id,
                                VideoMachineId = numberVideos,
                                Link = item,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine Technical คุณสมบัติทางเทคนิค List /////////////////////// 

                if (technical != null)
                {
                    List<TechnicalTtype> parts = new List<TechnicalTtype>();
                    var stepA = technical.Split(new char[] { ',' }).ToList();
                    for (int i = 0; i < stepA.ToArray().Length; i++)
                    {
                        var item = stepA[i];
                        var stepB = item.Split(new char[] { '-' }).ToList();
                        var tech = stepB[0].ToString().Substring(5, stepB[0].Length - 5);
                        var name = stepB[1].ToString().Substring(5, stepB[1].Length - 5);
                        parts.Add(new TechnicalTtype() { tech = tech, name = name });
                    }

                    foreach (var item in parts)
                    {
                        var technicalID = tableDetailTech.OrderByDescending(u => u.DetailTechMachineId).FirstOrDefault();
                        var numberTechnical = _service.GenID(technicalID != null ? technicalID.DetailTechMachineId : "", "D");


                        tableDetailTech.Add(
                            new Detailtechmachine
                            {
                                MachineId = id,
                                DetailTechMachineId = numberTechnical,
                                DetailTech = item.name,
                                TechnicallyId = item.tech,
                                EditDate = DateTime.Now,
                                EditUser = user,
                            }
                        );
                        _context.SaveChanges();
                    }
                }

                ///////////////////////////// machine  รูปภาพ List /////////////////////// 


                if (file.FormFileMulti != null)
                {

                    foreach (var item in file.FormFileMulti)
                    {
                        var imageID = tableImages.OrderByDescending(u => u.ImageMachineId).FirstOrDefault();
                        var numberImage = _service.GenID(imageID != null ? imageID.ImageMachineId : "", "I");

                        var imageFileMultiName = numberImage + DateTime.Now.ToString("ddMMyy_HHmmss") + item.FileName.Substring(item.FileName.LastIndexOf("."));

                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/machineImage", imageFileMultiName);
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        tableImages.Add(
                           new Imagemachine
                           {
                               MachineId = id,
                               ImageMachineId = numberImage,
                               FileName = imageFileMultiName,
                               Local = "/machineImage/" + imageFileMultiName
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


        ////////////////////////////////////// คุณสมบัติทางเทคนิค //////////////////////////////////////

        [HttpPost("[action]")]
        public IActionResult technical()
        {
            var table = _context.Technicallies;



            try
            {
                var items = table.OrderBy(r => r.TechnicallyId).ToList();

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
        public IActionResult addTechnical(string name)
        {
            var table = _context.Technicallies;

            try
            {

                var id = table.OrderByDescending(u => u.TechnicallyId).FirstOrDefault();

                var number = _service.GenID(id != null ? id.TechnicallyId : "", "T");

                var duplicate = table.Where(r => r.TechnicallyName == name).ToArray().Length;


                if (name.Trim() == "")
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณากรอกชื่อคุณสมบัติทางเทคนิค !",
                    });
                }


                if (duplicate > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "ชื่อคุณสมบัติทางเทคนิคซ้ำ !",
                    });
                }

                var items = table.Add(
                       new Technically
                       {
                           TechnicallyId = number,
                           TechnicallyName = name,

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
        public IActionResult updateTechnical(string id, string name)
        {
            var table = _context.Technicallies;

            try
            {
                var items = table.Where(r => r.TechnicallyId == id).First();
                var duplicate = table.Where(r => r.TechnicallyName == name).ToArray().Length;


                if (items.TechnicallyName != name)
                {
                    if (duplicate > 0)
                    {
                        return Ok(new
                        {
                            status = 200,
                            message = "ชื่อคุณสมบัติทางเทคนิคซ้ำ !",
                        });
                    }
                }





                items.TechnicallyName = name;
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

        [HttpDelete("[action]")]
        public IActionResult deleteTechnical(string id)
        {
            var table = _context.Technicallies;
            var detail = _context.Detailtechmachines;

            try
            {
                var items = table.Where(r => r.TechnicallyId == id).First();
                var check = detail.Where(r => r.TechnicallyId == id).ToArray().Length;

                if (check > 0)
                {
                    return Ok(new
                    {
                        status = 200,
                        message = "กรุณาลบคุณสมบัติเทคนิคนี้ในสินค้าทั้งหมด !",
                    });
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
        [AllowAnonymous]

        public IActionResult GetTEST([FromForm] FileUploadList file)
        {



            return Ok(new
            {
                status = 200,
                ori = file,
                // message = step_a,
            });

        }

    }

}