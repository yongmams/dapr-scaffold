using DapApp.Admin.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DapApp.Admin.API.Controllers
{
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

            // var fileName = Path.GetFileName(file.FileName);
            // var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            var fileName = Guid.NewGuid().ToString();
            var filePath = DateTime.Now.ToString("yyyyMM");

            await _fileService.Upload(file, "dapr-admin", $"{filePath}_{fileName}");

            return Ok($"File {fileName} has been uploaded successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> Test(string name)
        {
            return await Task.FromResult(Ok());
        }
    }
}