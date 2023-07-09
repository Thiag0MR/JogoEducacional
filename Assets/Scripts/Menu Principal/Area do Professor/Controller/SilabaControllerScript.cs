using System.IO;

public class SilabaControllerScript : BaseControllerScript
{
    public SilabaControllerScript () : base(
        Path.Combine("Data", "Silabas.txt"), 
        Path.Combine("Data", "Audio", "Silabas"), 
        "Digite o nome da sílaba")
    {
    }
}
