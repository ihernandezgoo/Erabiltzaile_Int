public class Hegazkina : Garraioa
{
    public Hegazkina()
    {
        Mota = "Hegazkina";
        SortuEserlekuak();
    }

    public override void SortuEserlekuak()
    {
        for (int i = 1; i <= 30; i++)
        {
            string zona;
            if (i <= 4) zona = "Business";
            else if (i <= 10) zona = "Aurreko aldea";
            else zona = "Atzeko aldea";

            Eserlekuak.Add(new Eserlekua($"{i}A", zona));
            Eserlekuak.Add(new Eserlekua($"{i}B", zona));
            Eserlekuak.Add(new Eserlekua($"{i}C", zona));
            Eserlekuak.Add(new Eserlekua($"{i}D", zona));
            Eserlekuak.Add(new Eserlekua($"{i}E", zona));
            Eserlekuak.Add(new Eserlekua($"{i}F", zona));
        }
    }
}