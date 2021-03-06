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
        private readonly long _maxFileSize;
        private readonly string[] _allowedExtensions;

        public FileUploadController(IConfiguration config) {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(),
                config["StoredFilesPath"]);
            _maxFileSize = Int64.Parse(config["MaxFileSize"]);
            _allowedExtensions = config.GetSection("AllowedFileExtensions")
                .GetChildren()
                .ToArray()
                .Select(s => s.Value)
                .ToArray();
            if (!Directory.Exists(_filePath)) {
                Directory.CreateDirectory(_filePath);
            }
        }

        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                var extension = Path.GetExtension(formFile.FileName);
                if (formFile.Length > 0 
                    && formFile.Length <= _maxFileSize
                    && _allowedExtensions.Contains(extension))
                {
                    var randomName = Path.GetRandomFileName();
                    randomName = Path.ChangeExtension(randomName, extension);
                    var filePath = Path.Combine(_filePath, 
                        randomName);

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