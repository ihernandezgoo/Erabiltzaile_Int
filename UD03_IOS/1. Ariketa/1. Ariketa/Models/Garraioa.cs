using System.Collections.Generic;

namespace _1._Ariketa
{
    public abstract class Garraioa
    {
        public string Mota { get; protected set; }
        public List<SeatControl> Eserlekuak { get; set; } = new List<SeatControl>();
        public abstract void SortuEserlekuak();
    }
}