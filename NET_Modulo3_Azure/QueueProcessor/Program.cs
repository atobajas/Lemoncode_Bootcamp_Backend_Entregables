using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Text.Json;
using System.Threading;

namespace QueueProcessor
{
    class Message
    {
        public string heroName { get; set; }
        public string alterEgoName { get; set; }
    }

    class Program
    {
        // Create a Blob service client
        static BlobServiceClient blobClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"));

        // Get Blob Container
        static BlobContainerClient containerHeroes = blobClient.GetBlobContainerClient("heroes");
        static BlobContainerClient containerAlteregos = blobClient.GetBlobContainerClient("alteregos");

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Queue Processor!");

            await GetQueueMessages();
        }

        private static async Task GetQueueMessages()
        {
            try
            {
                // Get the connection string from app settings
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                var queueClient = new QueueClient(connectionString, "picstodelete");

                // Create a queue
                await queueClient.CreateIfNotExistsAsync();

                while (true)
                {
                    // Message in queue is invisible for rest of processors.
                    QueueMessage message = await queueClient.ReceiveMessageAsync();
                    if (message != null)
                    {
                        Console.WriteLine($"Processing queue message {message.Body}");

                        var heroe = JsonSerializer.Deserialize<Message>(message.Body);

                        Console.WriteLine($"Delete image for {heroe.heroName} and {heroe.alterEgoName}");

                        var fileName = "";

                        // Delete Hero imagen
                        if (!String.IsNullOrEmpty(heroe.heroName))
                        {
                            fileName = $"{heroe.heroName.Replace(' ', '-').ToLower()}.jpeg";
                            await DeleteFileToAzureContainer(fileName, heroe.heroName, containerHeroes);
                        }

                        // Delete Alterego imagen
                        if (!String.IsNullOrEmpty(heroe.alterEgoName))
                        {
                            fileName = $"{heroe.alterEgoName.Replace(' ', '-').ToLower()}.png";
                            await DeleteFileToAzureContainer(fileName, heroe.alterEgoName, containerAlteregos);
                        }

                        // Delete message from queue
                        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                    }
                    else
                    {
                        Console.WriteLine("Waiting 5 seconds");
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }
        }

        private static async Task<bool> DeleteFileToAzureContainer(string fileName, string heroName, BlobContainerClient container)
        {
            if (String.IsNullOrEmpty(fileName)) return false;

            // Get Blob imagen
            var blob = container.GetBlobClient(fileName);

            var exist = await blob.ExistsAsync();
            if (exist)
            {
                await blob.DeleteAsync();
                Console.WriteLine($"File: {fileName} deleted.");
                return true;
            }
            Console.WriteLine($"File {fileName} not exist");
            return false;
        }
    }
}
