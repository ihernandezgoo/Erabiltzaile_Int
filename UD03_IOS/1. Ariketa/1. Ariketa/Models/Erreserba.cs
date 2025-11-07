using System;
using System.Collections.Generic;

namespace _1._Ariketa
{
    public class Erreserba
    {
        public string ErabiltzaileIzena { get; set; }
        public string GarraioMota { get; set; }
        public DateTime ErreserbaData { get; set; }
        public List<string> EserlekuakId { get; set; } = new List<string>();
    }
}