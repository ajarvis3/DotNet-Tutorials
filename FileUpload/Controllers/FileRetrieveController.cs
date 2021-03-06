using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FileUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileRetrieveController: ControllerBase
    {
        private readonly string _filePath;

        public FileRetrieveController(IConfiguration config)
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(),
                config["StoredFilesPath"]);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPicture(string name)
        {
            var fullPath = Path.Combine(_filePath, name);
            var extension = Path.GetExtension(name);
            var mime = extension == ".png" ? "image/png" : "image/jpeg";
            if (!System.IO.File.Exists(fullPath)) {
                return NotFound();
            }
            return PhysicalFile(fullPath, mime);
        }
    }
}