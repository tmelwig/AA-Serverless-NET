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
    }
}
