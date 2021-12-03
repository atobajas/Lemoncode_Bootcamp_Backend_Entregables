using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Text.Json;
using System.Threading;
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

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Queue Processor!");

            GetQueueMessages();
        }

        private static async System.Threading.Tasks.Task GetQueueMessages()
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
                    // Message in queue is invisible for all processors.
                    QueueMessage message = await queueClient.ReceiveMessageAsync();
                    if (message != null)
                    {
                        Console.WriteLine($"Processing queue message {message.Body}");

                        var heroe = JsonSerializer.Deserialize<Task>(message.Body);

                        Console.WriteLine($"Delete image for {heroe.heroName} and {heroe.alterEgoName}");

                        // Delete Hero imagen
                        await DeleteFileToAzureContainer(heroe.heroName, heroe.heroName);

                        // Delete Alterego imagen
                        await DeleteFileToAzureContainer(heroe.alterEgoName, heroe.alterEgoName);

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

        private static async Task<bool> DeleteFileToAzureContainer(string name, string heroName)
        {
            var fileName = $"{name.Replace(' ', '-').ToLower()}.jpeg";
            if (String.IsNullOrEmpty(fileName)) return false;

            // Get Blob imagen
            var blob = container.GetBlobClient(fileName);

            var exist = await blob.ExistsAsync();
            if (exist)
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
