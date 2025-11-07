namespace _1._Ariketa
{
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
                Eserlekuak.Add(new SeatControl($"{i}A"));
                Eserlekuak.Add(new SeatControl($"{i}B"));
                Eserlekuak.Add(new SeatControl($"{i}C"));
                Eserlekuak.Add(new SeatControl($"{i}D"));
            }
        }
    }
}