public enum EserlekuEgoera
{
    Librea,
    Okupatua,
    Hautatua
}

public class Eserlekua
{
    public string Id { get; set; }
    public EserlekuEgoera Egoera { get; set; }
    public string Zona { get; set; }

    public Eserlekua(string id, string zona)
    {
        Id = id;
        Egoera = EserlekuEgoera.Librea;
        Zona = zona;
    }
}