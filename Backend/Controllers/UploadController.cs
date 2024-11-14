
using System.Web;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;
        public UploadController(IWebHostEnvironment environment, DataContext context)
        {
            _env = environment;
            _context = context;
        }
        [HttpPost("{folder}/{id}")]
        public async Task<IActionResult> uploadImg(Guid id, string folder)
        {
            // Request's .Form.Files property will
            // contain QUploader's files.
            var files = this.Request.Form.Files;
            try
            {
                foreach (var file in files)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    // Do something with the file.
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(file.FileName);
                    fileName = fileName.Replace(' ', '_');

                    var fileSize = file.Length;
                    var path = Path.Combine(_env.ContentRootPath, "wwwroot/" + id + "/" + folder + "/"); //WebRootPath
                    // save to server...
                    // ...
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path,true);
                    }
                    Directory.CreateDirectory(path);

                    var destinationPath = Path.Combine(path, fileName);

                    if (System.IO.File.Exists(destinationPath))
                    {
                        System.IO.File.Delete(destinationPath);
                    }

                    var room = _context.RoomList!.FirstOrDefault(x => x.Id == id);
                    room.Img = Path.Combine("https://localhost:7022/" + id + "/" + folder + "/", fileName);
                    _context.Update(room);
                    _context.SaveChanges();
                    await using FileStream fs = new(destinationPath, FileMode.Create);
                    await file.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpGet("{folder}/{id}")]
        public async Task<IActionResult> load(string id, string folder)
        {
            var path = Path.Combine(_env.ContentRootPath, "src/" + id + "/" + folder + "/");
            string[] files = Directory.GetFiles(path);
            return Ok(files);

        }
    }
}