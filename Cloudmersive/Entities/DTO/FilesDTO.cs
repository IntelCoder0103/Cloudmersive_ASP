namespace Cloudmersive.Entities.DTO
{
    public class FileDTOBase
    {
        public string ConnectionName { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
    public class FileCreateDTO: FileDTOBase
    {
        public IFormFile FileContents { get; set; }
    }
    public class FileDownloadDTO: FileDTOBase
    {
    }

    public class FileDeleteDTO: FileDTOBase { }
}
