namespace tp4;


// using System;
// using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public class ImageResizer
{
    public static void ResizeOneImage(int factor, string pathToImg)
    {
        try
        {
            using (Image image = Image.Load(pathToImg))
            {
                image.Mutate(x => x.Resize(image.Width / factor, image.Height / factor));
                string outputPath = Path.Combine(Path.GetDirectoryName(pathToImg), $"resized_{Path.GetFileName(pathToImg)}");
                image.Save(outputPath);
                Console.WriteLine($"Resized image saved: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resizing image {pathToImg}: {ex.Message}");
        }
    }

    public static void ResizeMultipleImages(int factor, string[] pathToImgs)
    {
        Parallel.ForEach(pathToImgs, path =>
        {
            ResizeOneImage(factor, path);
        });
    }
}