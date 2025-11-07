namespace _1._Ariketa
{
    public class Hegazkina : Garraioa
    {
        public Hegazkina()
        {
            Mota = "Hegazkina";
            SortuEserlekuak();
        }

        public override void SortuEserlekuak()
        {
            for (int i = 1; i <= 20; i++)
            {
                Eserlekuak.Add(new SeatControl($"{i}A"));
                Eserlekuak.Add(new SeatControl($"{i}B"));
                Eserlekuak.Add(new SeatControl($"{i}C"));
                Eserlekuak.Add(new SeatControl($"{i}D"));
                Eserlekuak.Add(new SeatControl($"{i}E"));
                Eserlekuak.Add(new SeatControl($"{i}F"));
            }
        }
    }
}