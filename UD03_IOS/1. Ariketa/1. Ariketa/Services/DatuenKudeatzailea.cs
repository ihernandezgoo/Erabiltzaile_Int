using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace _1._Ariketa
{
    public class DatuenKudeatzailea
    {
        private readonly string _fitxategia;

        public DatuenKudeatzailea(string fitxategia)
        {
            _fitxategia = fitxategia;
        }

        public async Task<List<Erreserba>> KargatuErreserbakAsync()
        {
            if (!File.Exists(_fitxategia))
                return new List<Erreserba>();

            string json = await File.ReadAllTextAsync(_fitxategia);
            return JsonSerializer.Deserialize<List<Erreserba>>(json) ?? new List<Erreserba>();
        }

        public async Task GordeErreserbakAsync(List<Erreserba> erreserbak)
        {
            var json = JsonSerializer.Serialize(erreserbak, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_fitxategia, json);
        }
    }
}