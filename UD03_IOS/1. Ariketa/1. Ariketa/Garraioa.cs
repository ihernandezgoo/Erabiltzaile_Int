using System.Collections.Generic;

public abstract class Garraioa
{
    public List<Eserlekua> Eserlekuak { get; protected set; }
    public string Mota { get; protected set; }

    public Garraioa()
    {
        Eserlekuak = new List<Eserlekua>();
    }

    public abstract void SortuEserlekuak();
}