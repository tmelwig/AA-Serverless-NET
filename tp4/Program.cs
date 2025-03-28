namespace tp4;

using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        Personne personne = new Personne("thomas", 24);
        string stringifyPersonne = JsonConvert.SerializeObject(personne, Formatting.Indented);
        personne.Hello(true);
        Console.WriteLine(stringifyPersonne);
        ImageResizer.ResizeOneImage(10, "./images/resizedPhotocs.jpeg")
        ImageResizer.ResizeMultipleImages(10, ["./images/photocs.jpeg", "./images/resizedPhotocs.jpeg"]);
    }
}
