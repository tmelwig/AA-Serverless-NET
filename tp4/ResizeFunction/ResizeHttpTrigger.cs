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

            // Check if the request body is empty
            if (req.Body == null || req.ContentLength == 0)
            {
                return new BadRequestObjectResult("Request body is empty.");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await req.Body.CopyToAsync(memoryStream);
                    if (memoryStream.Length == 0)
                    {
                        return new BadRequestObjectResult("Request body is empty.");
                    }
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
            catch (SixLabors.ImageSharp.UnknownImageFormatException ex)
            {
                _logger.LogError($"Invalid image format: {ex.Message}");
                return new BadRequestObjectResult("The provided file is not a valid image.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error processing image: {ex.Message}");
                // Return 400 Bad Request for unexpected errors to avoid status 500
                return new BadRequestObjectResult("An error occurred while processing the image. Please ensure that the file is a valid image and try again.");
            }
        }
    }
}
