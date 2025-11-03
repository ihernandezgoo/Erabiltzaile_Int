using System;
using System.Collections.Generic;

public class Erreserba
{
    public Guid ErreserbaId { get; set; } // Identifikatzaile unikoa
    public string ErabiltzaileIzena { get; set; }
    public string GarraioMota { get; set; }
    public List<string> EserlekuakId { get; set; }
    public DateTime ErreserbaData { get; set; }

    public Erreserba()
    {
        ErreserbaId = Guid.NewGuid();
        EserlekuakId = new List<string>();
    }
}