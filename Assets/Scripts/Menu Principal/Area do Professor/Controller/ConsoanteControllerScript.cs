using System.IO;

public class ConsoanteControllerScript : BaseControllerScript
{
    public ConsoanteControllerScript () : base(
        Path.Combine("Data", "Consoantes.txt"),
        Path.Combine("Data", "Audio", "Consoantes"),
        "Digite o nome da consoante")
    {
    }
}
