using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageService.Models;

namespace StorageService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {

        private readonly IBlobStorage _blobStorage;

        public BlobsController(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        [HttpGet]
        public async Task<IActionResult> GetNames()
        {
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            return Ok(names);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile picture)
        {
            await _blobStorage.SetLog("Upload methoduna giriş yapıldı", "controller.txt");

            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

            await _blobStorage.UploadAsync(picture.OpenReadStream(), newFileName, EContainerName.pictures);

            await _blobStorage.SetLog("Upload methodundan çıkış yapıldı", "controller.txt");
            return Ok();
        }
    }
}
