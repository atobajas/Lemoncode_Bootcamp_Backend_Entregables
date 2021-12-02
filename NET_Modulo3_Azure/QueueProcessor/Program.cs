using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace QueueProcessor
{
    class Task
    {
        public string heroName { get; set; }
        public string alterEgoName { get; set; }
    }

    class Program
    {
        // Create a Blob service client
        static BlobServiceClient blobClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"));

        // Get Blob Container
        static BlobContainerClient container = blobClient.GetBlobContainerClient("heroes");

        static async void Main(string[] args)
        {
            Console.WriteLine("Hello Queue Processor!");

            try
            {
                /*********** Background processs (We have to delete the hero and alterego images) *************/
                // Get the connection string from app settings
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                var queueClient = new QueueClient(connectionString, "picstodelete");

                // Create a queue
                await queueClient.CreateIfNotExistsAsync();

                while (true)
                {
                    QueueMessage message = await queueClient.ReceiveMessageAsync();
                    if (message != null)
                    {
                        Console.WriteLine($"Processing queue message {message.Body}");

                        var heroe = JsonSerializer.Deserialize<Task>(message.Body);

                        Console.WriteLine($"Delete image for {heroe.heroName} and {heroe.alterEgoName}");

                        // Delete Hero imagen
                        var fileName = $"{heroe.heroName.Replace(' ', '-').ToLower()}.jpeg";
                        await DeleteFileToAzureContainer(fileName, heroe.heroName);

                        // Delete Alterego imagen
                        fileName = $"{heroe.alterEgoName.Replace(' ', '-').ToLower()}.jpeg";
                        await DeleteFileToAzureContainer(fileName, heroe.alterEgoName);
                    };
                }
                /*********** End Background processs *************/
            }
            catch (Exception e)
            {
                //if (!HeroExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
                throw;
            }
        }

        private static async Task<bool> DeleteFileToAzureContainer(string fileName, string heroName)
        {
            
            //TODO use async method.
            // Get Hero imagen
            var blob = container.GetBlobClient(fileName);

            if (blob.Exists())
            {
                await blob.DeleteAsync();
                Console.WriteLine($"Heroe: {heroName} deleted.");
                return true;
            }
            Console.WriteLine($"File {fileName} not exist");
            return false;
        }
    }
}
