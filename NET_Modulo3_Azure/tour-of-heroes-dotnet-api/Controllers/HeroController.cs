using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tour_of_heroes_api.Models;
using tour_of_heroes_api.Modesl;

namespace tour_of_heroes_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly HeroContext _context;

        public HeroController(HeroContext context)
        {
            _context = context;
        }

        // GET: api/Hero
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hero>>> GetHeroes()
        {
            return await _context.Heroes.ToListAsync();
        }

        // GET: api/Hero/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHero(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);

            if (hero == null)
            {
                return NotFound();
            }

            return hero;
        }

        // PUT: api/Hero/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHero(int id, Hero hero)
        {
            if (id != hero.Id)
            {
                return BadRequest();
            }

            _context.Entry(hero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hero
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hero>> PostHero(Hero hero)
        {
            _context.Heroes.Add(hero);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHero), new { id = hero.Id }, hero);
        }

        // DELETE: api/Hero/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHero(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }

            try
            {
                /*********** Background processs (We have to delete the hero and alterego images) *************/
                // Get the connection string from app settings
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                var queueClient = new QueueClient(connectionString, "picstodelete");

                // Create a queue
                await queueClient.CreateIfNotExistsAsync();

                // Create a dynamic object to hold the message
                var message = new
                {
                    heroName = hero.Name,
                    alterEgoName = hero.AlterEgo
                };

                // Send the message
                await queueClient.SendMessageAsync(JsonSerializer.Serialize(message).ToString());

                /*********** End Background processs *************/
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HeroExists(int id)
        {
            return _context.Heroes.Any(e => e.Id == id);
        }

        // GET: api/hero/alteregopic/5
        [HttpGet("alteregopic/{id}")]
        public async Task<IActionResult> GetAlterEgoPic(int id)
        {
            var hero = await _context.Heroes.FirstOrDefaultAsync(h => h.Id == id);

            if (hero == null)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(hero.AlterEgo))
            {
                //Get image from Azure Storage
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

                // Create a BlobServiceClient object which will be used to create a container client
                var blobServiceClient = new BlobServiceClient(connectionString);

                //Get container client
                var containerClient = blobServiceClient.GetBlobContainerClient("alteregos");

                //Get blob client
                var blob = containerClient.GetBlobClient($"{hero.AlterEgo.ToLower().Replace(' ', '-')}.png");

                //Get image from blob
                var exist = await blob.ExistsAsync();
                if (exist)
                {
                    var image = await blob.DownloadStreamingAsync();

                    //return image
                    return File(image.Value.Content, "image/png");
                }
            }

            return NotFound();
        }

        // GET: api/hero/alteregopic/sas
        [HttpGet("alteregopic/sas/{imgName}")]
        public async Task<IActionResult> GetAlterEgoPicSas(string imgName)
        {
            if (!String.IsNullOrEmpty(imgName))
            {
                //Get image from Azure Storage
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

                // Create a BlobServiceClient object which will be used to create a container client
                var blobServiceClient = new BlobServiceClient(connectionString);

                //Get container client
                var containerClient = blobServiceClient.GetBlobContainerClient("picsToDelete");

                //Get blob client
                var blobClient = containerClient.GetBlobClient(imgName);

                var exist = await blobClient.ExistsAsync();
                if (exist)
                {
                    var sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = "alteregos",
                        BlobName = imgName,
                        Resource = "b"
                    };

                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(3);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

                    Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

                    Console.WriteLine($"SAS Uri for blob is: {sasUri}");

                    //return image
                    return Ok($"{blobServiceClient.Uri}{sasUri.Query.ToString()}");
                }
            }
            return NotFound();
        }
    }
}
