using DapApp.Admin.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace DapApp.Admin.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WebAppController : ControllerBase
    {
        private readonly ILogger<WebAppController> _logger;
        private readonly IFileService _fileService;

        public WebAppController(ILogger<WebAppController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded");
            }

            var extName = Path.GetExtension(file.FileName);
            if (string.Compare(extName, ".zip", true) != 0)
            {
                return BadRequest("File must be in zip format");
            }

            var fileName = Guid.NewGuid().ToString();

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "temp"));
            var tempFilePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "temp", fileName + extName);

            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "WebApp", fileName);
            ZipFile.ExtractToDirectory(tempFilePath, filePath);

            System.IO.File.Delete(tempFilePath);

            //var fileName = Guid.NewGuid().ToString();
            //var filePath = DateTime.Now.ToString("yyyyMM");

            //await _fileService.Upload(file, "dapr-admin", $"{filePath}_{fileName}");

            return Ok($"File {fileName} has been uploaded successfully.");
        }
    }
}