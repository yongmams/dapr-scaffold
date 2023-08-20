namespace DapApp.Admin.API.Services
{
    public interface IFileService
    {
        Task Upload(IFormFile file, string directoryName, string fileName);
    }
}
