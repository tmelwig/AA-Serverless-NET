using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace tp4
{
    public class ResizeHttpTrigger
    {
        private readonly ILogger<ResizeHttpTrigger> _logger;

        public ResizeHttpTrigger(ILogger<ResizeHttpTrigger> logger)
        {
            _logger = logger;
        }

        [Function("ResizeHttpTrigger")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Validate and parse query parameters
            if (!int.TryParse(req.Query["w"], out int width) || width <= 0 ||
                !int.TryParse(req.Query["h"], out int height) || height <= 0)
            {
                return new BadRequestObjectResult("Invalid or missing width (w) or height (h) parameters.");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await req.Body.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    using (Image image = Image.Load(memoryStream))
                    {
                        image.Mutate(x => x.Resize(width, height));

                        using (var outputStream = new MemoryStream())
                        {
                            image.SaveAsJpeg(outputStream);
                            return new FileContentResult(outputStream.ToArray(), "image/jpeg");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing image: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
