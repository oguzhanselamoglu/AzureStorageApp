using StorageService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageService
{
    public interface IBlobStorage
    {

        public string BlobUrl { get; }
        Task UploadAsync(Stream stream, string fileName, EContainerName eContainerName);
        Task<Stream> DownloadAsync(string fileName, EContainerName eContainerName); 
        Task DeleteAsync(string fileName, EContainerName eContainerName);

        //Loglama amaclı
        Task SetLogAsync(string text, string fileName);
        Task<List<string>> GetLogsAsync(string fileName);
        List<string> GetNames(EContainerName eContainerName);

    }
}
