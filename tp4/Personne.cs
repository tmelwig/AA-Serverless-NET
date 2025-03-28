namespace tp4;


class Personne
{
    public string Nom { get; set; }
    public int Age { get; set; }

    public Personne(string nom, int age) {
        Nom = nom;
        Age = age;
    }

    public void Hello(bool isLowerCase) {
        string message = $"hello {Nom}, you are {Age}";
        Console.WriteLine(isLowerCase ? message.ToLower() : message.ToUpper());
    }
}
