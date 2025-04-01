using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using Azure.Identity;
using Azure.Core;

namespace AccessBlob
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Start");

            DefaultAzureCredential credential = new DefaultAzureCredential();

            Console.WriteLine($"{credential.GetToken(new TokenRequestContext(new[] { "https://cognizantvmpoc1storage.blob.core.windows.net" }), CancellationToken.None).Token}");

            var blobServiceClient = new BlobServiceClient(
                                    new Uri("https://cognizantvmpoc1storage.blob.core.windows.net"),
                                    credential);//new DefaultAzureCredential());

            //Create a unique name for the container
            string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

            // Create the container and return a container client object
            //BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("");

            // Create a local file in the ./data/ directory for uploading and downloading
            string localPath = "data";
            Directory.CreateDirectory(localPath);
            string fileName = "quickstart" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            Console.WriteLine("Storage container created.");

            // Write text to the file
            await File.WriteAllTextAsync(localFilePath, "Hello, World!");

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Upload data from the local file, overwrite the blob if it already exists
            await blobClient.UploadAsync(localFilePath, true);

            Console.WriteLine("End");
            while (Console.ReadLine() != null) { }
        }
    }
}
