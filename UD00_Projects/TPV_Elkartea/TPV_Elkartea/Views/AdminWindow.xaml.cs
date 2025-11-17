using Microsoft.Data.Sqlite;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using TPV_Elkartea.Models;

namespace TPV_Elkartea.Views
{
    public partial class AdminWindow : Window
    {
        private string dbProductos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "produktuak.db");
        private string dbUsuarios = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "erabiltzaileak.db");

        private BindingList<Producto> productos = new BindingList<Producto>();
        private BindingList<Erabiltzaileak> usuarios = new BindingList<Erabiltzaileak>();

        public AdminWindow()
        {
            InitializeComponent();
            CargarProductos();
            CargarUsuarios();
            dgProductos.ItemsSource = productos;
            dgUsuarios.ItemsSource = usuarios;
        }

        #region Productos CRUD

        private void CargarProductos()
        {
            productos.Clear();
            if (!File.Exists(dbProductos)) return;

            using var conexion = new SqliteConnection($"Data Source={dbProductos}");
            conexion.Open();
            string query = "SELECT Id, Nombre, Precio, Stock, Img FROM Edariak";
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
                    img = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }
        }

        private void BtnAnadirProducto_Click(object sender, RoutedEventArgs e)
        {
            var nuevo = new Producto { Nombre = "Producto nuevo", Precio = 0, Stock = 0, img = "" };
            using var conexion = new SqliteConnection($"Data Source={dbProductos}");
            conexion.Open();
            string query = "INSERT INTO Edariak (Nombre, Precio, Stock, Img) VALUES (@nombre, @precio, @stock, @img); SELECT last_insert_rowid();";
            using var cmd = new SqliteCommand(query, conexion);
            cmd.Parameters.AddWithValue("@nombre", nuevo.Nombre);
            cmd.Parameters.AddWithValue("@precio", nuevo.Precio);
            cmd.Parameters.AddWithValue("@stock", nuevo.Stock);
            cmd.Parameters.AddWithValue("@img", nuevo.img);
            nuevo.Id = (int)(long)cmd.ExecuteScalar();
            productos.Add(nuevo);
        }

        private void BtnEliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (dgProductos.SelectedItem is Producto p)
            {
                if (MessageBox.Show($"¿Ezabatu {p.Nombre}?", "Konfirmatu", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using var conexion = new SqliteConnection($"Data Source={dbProductos}");
                    conexion.Open();
                    string query = "DELETE FROM Edariak WHERE Id=@id";
                    using var cmd = new SqliteCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.ExecuteNonQuery();
                    productos.Remove(p);
                }
            }
        }

        private void BtnGuardarProductos_Click(object sender, RoutedEventArgs e)
        {
            using var conexion = new SqliteConnection($"Data Source={dbProductos}");
            conexion.Open();

            foreach (var p in productos)
            {
                string query = "UPDATE Edariak SET Nombre=@nombre, Precio=@precio, Stock=@stock, Img=@img WHERE Id=@id";
                using var cmd = new SqliteCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@precio", p.Precio);
                cmd.Parameters.AddWithValue("@stock", p.Stock);
                cmd.Parameters.AddWithValue("@img", p.img);
                cmd.Parameters.AddWithValue("@id", p.Id);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Produktuen aldaketak gorde dira.", "Ondo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region Usuarios CRUD

        private void CargarUsuarios()
        {
            usuarios.Clear();
            if (!File.Exists(dbUsuarios)) return;

            using var conexion = new SqliteConnection($"Data Source={dbUsuarios}");
            conexion.Open();
            string query = "SELECT Id, Izena, Rola, Pasahitza FROM Erabiltzaileak";
            using var cmd = new SqliteCommand(query, conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                usuarios.Add(new Erabiltzaileak
                {
                    Id = reader.GetInt32(0),
                    Izena = reader.GetString(1),
                    Rola = reader.GetString(2),
                    Pasahitza = reader.GetString(3)
                });
            }
        }

        private void BtnAnadirUsuario_Click(object sender, RoutedEventArgs e)
        {
            var nuevo = new Erabiltzaileak { Izena = "Erabiltzaile berria", Rola = "Erabiltzailea", Pasahitza = "Pasahitza" };
            using var conexion = new SqliteConnection($"Data Source={dbUsuarios}");
            conexion.Open();
            string query = "INSERT INTO Erabiltzaileak (Izena, Rola, Pasahitza) VALUES (@izena, @rola, @pasahitza); SELECT last_insert_rowid();";
            using var cmd = new SqliteCommand(query, conexion);
            cmd.Parameters.AddWithValue("@izena", nuevo.Izena);
            cmd.Parameters.AddWithValue("@rola", nuevo.Rola);
            cmd.Parameters.AddWithValue("@pasahitza", nuevo.Pasahitza);
            nuevo.Id = (int)(long)cmd.ExecuteScalar();
            usuarios.Add(nuevo);
        }

        private void BtnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsuarios.SelectedItem is Erabiltzaileak u)
            {
                using var conexion = new SqliteConnection($"Data Source={dbUsuarios}");
                conexion.Open();
                string query = "UPDATE Erabiltzaileak SET Izena=@izena, Rola=@rola, Pasahitza=@pasahitza WHERE Id=@id";
                using var cmd = new SqliteCommand(query, conexion);
                cmd.Parameters.AddWithValue("@izena", u.Izena);
                cmd.Parameters.AddWithValue("@rola", u.Rola);
                cmd.Parameters.AddWithValue("@pasahitza", u.Pasahitza);
                cmd.Parameters.AddWithValue("@id", u.Id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Erabiltzailea eguneratua.", "Ondo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnEliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsuarios.SelectedItem is Erabiltzaileak u)
            {
                if (MessageBox.Show($"¿Ezabatu {u.Izena}?", "Konfirmatu", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using var conexion = new SqliteConnection($"Data Source={dbUsuarios}");
                    conexion.Open();
                    string query = "DELETE FROM Erabiltzaileak WHERE Id=@id";
                    using var cmd = new SqliteCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@id", u.Id);
                    cmd.ExecuteNonQuery();
                    usuarios.Remove(u);
                }
            }
        }

        private void BtnGuardarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            using var conexion = new SqliteConnection($"Data Source={dbUsuarios}");
            conexion.Open();

            foreach (var u in usuarios)
            {
                string query = "UPDATE Erabiltzaileak SET Izena=@izena, Rola=@rola, Pasahitza=@pasahitza WHERE Id=@id";
                using var cmd = new SqliteCommand(query, conexion);
                cmd.Parameters.AddWithValue("@izena", u.Izena);
                cmd.Parameters.AddWithValue("@rola", u.Rola);
                cmd.Parameters.AddWithValue("@pasahitza", u.Pasahitza);
                cmd.Parameters.AddWithValue("@id", u.Id);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Erabiltzaileen aldaketak gorde dira.", "Ondo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
    }
}