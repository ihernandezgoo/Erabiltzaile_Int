using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;
using TPV_Elkartea.Models;

namespace TPV_Elkartea.Services
{
    public class ProductoService
    {
        private readonly string dbPath;

        public ProductoService(string databasePath)
        {
            dbPath = databasePath;
        }

        public List<Producto> CargarProductos()
        {
            var productos = new List<Producto>();
            if (!File.Exists(dbPath))
                throw new FileNotFoundException("No se encontró la base de datos SQLite.", dbPath);

            using var conexion = new SqliteConnection($"Data Source={dbPath}");
            conexion.Open();
            string query = "SELECT Id, Nombre, Precio, Stock, img FROM Edariak";

            using var cmd = new SqliteCommand(query, conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                productos.Add(new Producto
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Precio = reader.GetDouble(2),
                    Stock = reader.GetInt32(3),
                    img = reader.IsDBNull(4) ? "https://via.placeholder.com/100x100.png?text=No+Image" : reader.GetString(4)
                });
            }

            return productos;
        }

        public void GuardarProductos(IEnumerable<Producto> productos)
        {
            if (!File.Exists(dbPath))
                throw new FileNotFoundException("No se encontró la base de datos SQLite.", dbPath);

            using var conexion = new SqliteConnection($"Data Source={dbPath}");
            conexion.Open();

            foreach (var p in productos)
            {
                using var cmd = new SqliteCommand("UPDATE Edariak SET Stock=@stock WHERE Id=@id", conexion);
                cmd.Parameters.AddWithValue("@stock", p.Stock);
                cmd.Parameters.AddWithValue("@id", p.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}