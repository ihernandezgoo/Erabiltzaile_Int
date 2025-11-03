public class Trena : Garraioa
{
    public Trena()
    {
        Mota = "Trena";
        SortuEserlekuak();
    }

    public override void SortuEserlekuak()
    {
        for (int i = 1; i <= 20; i++)
        {
            string zona = (i <= 5) ? "Lehen maila" : "Turista";
            Eserlekuak.Add(new Eserlekua($"{i}A", zona));
            Eserlekuak.Add(new Eserlekua($"{i}B", zona));
            Eserlekuak.Add(new Eserlekua($"{i}C", zona));
            Eserlekuak.Add(new Eserlekua($"{i}D", zona));
        }
    }
}