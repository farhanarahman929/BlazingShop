using BlazingShop.Shared.Models;
using Microsoft.AspNetCore.Hosting;//it provides information about the application's file system paths
using Microsoft.AspNetCore.Mvc;//This provides attributes and classes (like ControllerBase) to create API controllers and actions.
using System;
using System.IO; //This provides functionality for working with files and file paths
using System.Threading.Tasks; //Used for asynchronous programming with Task and async/await.

namespace BlazingShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment env;

        public UploadController(IWebHostEnvironment env)
        {
            this.env = env;
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ImageFile[] files)
        {
            if (files == null || files.Length == 0)
            {
                return BadRequest("No files provided.");
            }

            foreach (var file in files)
            {
                try
                {
                    // Trim any leading or trailing whitespace from base64 data
                    var base64data = file.base64data.Trim();
                    var buf = Convert.FromBase64String(base64data);
                    var filePath = Path.Combine(env.ContentRootPath, Guid.NewGuid().ToString("N") + "-" + file.fileName); //Combines the root path of the application with a unique file name. The Guid.NewGuid().ToString("N") generates a unique identifier to prevent filename collisions.

                    await System.IO.File.WriteAllBytesAsync(filePath, buf);
                }
                catch (FormatException ex)
                {
                    return BadRequest($"Invalid base64 format for file {file.fileName}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Failed to save file {file.fileName}: {ex.Message}");
                }
            }

            return Ok("Files uploaded successfully.");
        }
    }
}
