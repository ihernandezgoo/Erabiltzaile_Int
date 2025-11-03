using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class DatuenKudeatzailea
{
    private readonly string _filePath;

    public DatuenKudeatzailea(string fileName)
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    }

    public async Task<List<Erreserba>> KargatuErreserbakAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Erreserba>();
        }

        try
        {
            string jsonString = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrEmpty(jsonString))
            {
                return new List<Erreserba>();
            }
            return JsonSerializer.Deserialize<List<Erreserba>>(jsonString);
        }
        catch
        {
            return new List<Erreserba>();
        }
    }

    public async Task GordeErreserbakAsync(List<Erreserba> erreserbak)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(erreserbak, options);
        await File.WriteAllTextAsync(_filePath, jsonString);
    }
}