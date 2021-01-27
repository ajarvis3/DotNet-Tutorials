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
    public class FileUploadController: ControllerBase
    {
        private readonly string _filePath;

        public FileUploadController(IConfiguration config) {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(),
                config["StoredFilesPath"]);
            if (!Directory.Exists(_filePath)) {
                Directory.CreateDirectory(_filePath);
            }
        }

        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            Console.WriteLine(size);
            Console.WriteLine(files);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(_filePath, 
                        Path.GetRandomFileName());
                    Console.WriteLine(filePath);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size });
        }
    }
}