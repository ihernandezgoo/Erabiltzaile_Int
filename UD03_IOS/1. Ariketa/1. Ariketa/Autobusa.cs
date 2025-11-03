public class Autobusa : Garraioa
{
    public Autobusa()
    {
        Mota = "Autobusa";
        SortuEserlekuak();
    }

    public override void SortuEserlekuak()
    {
        for (int i = 1; i <= 10; i++)
        {
            Eserlekuak.Add(new Eserlekua($"{i}A", "Lehioa"));
            Eserlekuak.Add(new Eserlekua($"{i}B", "Pasilloa"));
            Eserlekuak.Add(new Eserlekua($"{i}C", "Pasilloa"));
            Eserlekuak.Add(new Eserlekua($"{i}D", "Lehioa"));
        }
    }
}