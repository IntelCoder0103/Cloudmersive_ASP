using Cloudmersive.Entities.DTO;
using Cloudmersive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cloudmersive.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly StorageService _storageService;
        public FilesController(StorageService storageService)
        {
            this._storageService = storageService;
        }
        [HttpPost]
        [Route("create-file")]
        public async Task<IActionResult> CreateFile([FromForm]FileCreateDTO dto)
        {
            try
            {
                byte[] fileContents;
                using (var ms = new MemoryStream())
                {
                    dto.FileContents.CopyTo(ms);
                    fileContents = ms.ToArray();
                }
                var uri = await this._storageService.UploadFileAsync(dto.ConnectionName, dto.FileName, dto.FilePath, fileContents);
                return Ok(uri);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("download-file")]
        public async Task<IActionResult> DownloadFile([FromQuery]FileDownloadDTO dto)
        {
            try
            {
                byte[] data = await this._storageService.DownloadFileAsync(dto.ConnectionName, dto.FileName, dto.FilePath);

                return File(data, "plain/text");
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("delete-file")]
        public async Task<IActionResult> DeleteFile([FromQuery]FileDeleteDTO dto)
        {
            try
            {
                var res = await this._storageService.DeleteFileAsync(dto.ConnectionName, dto.FileName, dto.FilePath);
                return Ok(res);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
