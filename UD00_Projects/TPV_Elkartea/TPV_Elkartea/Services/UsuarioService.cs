using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TPV_Elkartea.Models;

namespace TPV_Elkartea.Services
{
    public class UsuarioService
    {
        private readonly string usuariosFile;

        public UsuarioService(string baseDirectory)
        {
            usuariosFile = Path.Combine(baseDirectory, "erabiltzaileak.json");
        }

        public List<Usuario> CargarUsuarios()
        {
            if (!File.Exists(usuariosFile))
                return new List<Usuario>();

            string json = File.ReadAllText(usuariosFile);
            return JsonSerializer.Deserialize<List<Usuario>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Usuario>();
        }
    }
}