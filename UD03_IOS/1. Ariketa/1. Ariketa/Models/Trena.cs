namespace _1._Ariketa
{
    public class Trena : Garraioa
    {
        public Trena()
        {
            Mota = "Trena";
            SortuEserlekuak();
        }

        public override void SortuEserlekuak()
        {
            for (int i = 1; i <= 12; i++)
            {
                Eserlekuak.Add(new SeatControl($"{i}A"));
                Eserlekuak.Add(new SeatControl($"{i}B"));
                Eserlekuak.Add(new SeatControl($"{i}C"));
                Eserlekuak.Add(new SeatControl($"{i}D"));
            }
        }
    }
}