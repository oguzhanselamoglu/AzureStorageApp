using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using StorageService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageService.Services
{
    public class BlobStorage : IBlobStorage
    {

        private readonly BlobServiceClient blobServiceClient;

        public BlobStorage()
        {
            blobServiceClient= new BlobServiceClient(ConnectionStrings.AzureStorageConnectionString);
        }

       

        public string BlobUrl => ConnectionStrings.BlobUrl;

        public async Task DeleteAsync(string fileName, EContainerName eContainerName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(eContainerName.ToString());
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.None);
        }

        public async Task<Stream> DownloadAsync(string fileName, EContainerName eContainerName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(eContainerName.ToString());
            var blobClient = containerClient.GetBlobClient(fileName);

            var info = await blobClient.DownloadAsync();

            return info.Value.Content;
        }

        public async Task<List<string>> GetLogsAsync(string fileName)
        {
            List<string> logs = new List<string>();

            var containerClient = blobServiceClient.GetBlobContainerClient(EContainerName.logs.ToString());
            var appendBlob = containerClient.GetAppendBlobClient(fileName);
            await appendBlob.CreateIfNotExistsAsync();

            var info = await appendBlob.DownloadAsync();
            using (StreamReader sr = new StreamReader(info.Value.Content))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    logs.Add(line);
                }
            }
            

            return logs;
        }

        public List<string> GetNames(EContainerName eContainerName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(eContainerName.ToString());
            var list = new List<string>();
            var blobs = containerClient.GetBlobs();
            
            foreach (var blob in blobs)
                list.Add(blob.Name);


            return list;
        }

        public async Task SetLogAsync(string text, string fileName)
        {

            var containerClient = blobServiceClient.GetBlobContainerClient(EContainerName.logs.ToString());

            var appendBlobClient = containerClient.GetAppendBlobClient($"test/{fileName}");
            await appendBlobClient.CreateIfNotExistsAsync();

            using(MemoryStream ms = new MemoryStream())
	        {
                using(StreamWriter sw = new StreamWriter(ms))
	            {
                    sw.Write($"{DateTime.Now}: {text}/n");
                    sw.Flush();
                    ms.Position = 0;

                    await appendBlobClient.AppendBlockAsync(ms);
	            }
	        }
            
        }

        public async Task UploadAsync(Stream stream, string fileName, EContainerName eContainerName)
        {

            var containerClient = blobServiceClient.GetBlobContainerClient(eContainerName.ToString());


            await containerClient.CreateIfNotExistsAsync();

            await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);


            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(stream);
          
        }
    }
}
