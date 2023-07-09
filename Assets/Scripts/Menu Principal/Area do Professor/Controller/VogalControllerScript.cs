using System.IO;

public class VogalControllerScript : BaseControllerScript
{
    public VogalControllerScript () : base(
        Path.Combine("Data", "Vogais.txt"),
        Path.Combine("Data", "Audio", "Vogais"),
        "Digite o nome da vogal")
    {
    }
}
