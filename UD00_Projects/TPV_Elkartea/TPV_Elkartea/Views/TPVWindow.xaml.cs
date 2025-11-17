using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Data.Sqlite;

namespace TPV_Elkartea.Views
{
    public class Producto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        private double precio;
        public double Precio { get => precio; set { precio = value; OnPropertyChanged(nameof(Precio)); } }
        private int stock;
        public int Stock { get => stock; set { stock = value; OnPropertyChanged(nameof(Stock)); } }
        private string imagen;
        public string img { get => imagen; set { imagen = value; OnPropertyChanged(nameof(img)); } }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    public class CarritoItem : INotifyPropertyChanged
    {
        public Producto Producto { get; set; }
        private int cantidad;
        public int Cantidad { get => cantidad; set { cantidad = value; OnPropertyChanged(nameof(Cantidad)); OnPropertyChanged(nameof(Total)); } }
        public double Total => Producto.Precio * Cantidad;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    public class ProductosCategoria
    {
        public List<Producto> Edariak { get; set; } = new List<Producto>();
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
    }

    public partial class TPVWindow : Window
    {
        private ProductosCategoria categorias;
        private BindingList<CarritoItem> carrito = new BindingList<CarritoItem>();
        private List<Usuario> usuarios = new List<Usuario>();
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "produktuak.db");

        public TPVWindow()
        {
            InitializeComponent();
            CargarProductos();
            CargarUsuarios();
            dgCarrito.ItemsSource = carrito;
            ActualizarProductos();
        }

        private void CargarProductos()
        {
            categorias = new ProductosCategoria();
            if (!File.Exists(dbPath))
            {
                MessageBox.Show("No se encontró la base de datos SQLite.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using var conexion = new SqliteConnection($"Data Source={dbPath}");
            conexion.Open();
            string query = "SELECT Id, Nombre, Precio, Stock, img FROM Edariak";
            using var cmd = new SqliteCommand(query, conexion);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var p = new Producto
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Precio = reader.GetDouble(2),
                    Stock = reader.GetInt32(3),
                    img = reader.IsDBNull(4) ? "https://via.placeholder.com/100x100.png?text=No+Image" : reader.GetString(4)
                };
                categorias.Edariak.Add(p);
            }
        }

        private void CargarUsuarios()
        {
            string usuariosFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "erabiltzaileak.json");
            if (!File.Exists(usuariosFile)) { usuarios = new List<Usuario>(); return; }
            string json = File.ReadAllText(usuariosFile);
            usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Usuario>();
        }

        private void ActualizarProductos() => icProductos.ItemsSource = categorias.Edariak;

        private void icProductos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (icProductos.SelectedItem is Producto seleccionado)
            {
                int cantidad = 1;
                var existente = carrito.FirstOrDefault(x => x.Producto == seleccionado);
                if (existente != null) existente.Cantidad += cantidad;
                else carrito.Add(new CarritoItem { Producto = seleccionado, Cantidad = cantidad });
                ActualizarCarrito();
            }
        }

        private void ActualizarCarrito()
        {
            dgCarrito.Items.Refresh();
            double subtotal = carrito.Sum(i => i.Total);
            double iva = subtotal * 0.21;
            double total = subtotal + iva;
            tbTotal.Text = $"Subtotal: {subtotal:F2} € | IVA 21%: {iva:F2} € | TOTAL: {total:F2} €";
        }

        private void BtnEditarCantidadFila_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CarritoItem seleccionado)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese nueva cantidad para {seleccionado.Producto.Nombre}", "Editar Cantidad", seleccionado.Cantidad.ToString());
                if (int.TryParse(input, out int nuevaCantidad) && nuevaCantidad > 0)
                {
                    seleccionado.Cantidad = nuevaCantidad;
                    ActualizarCarrito();
                }
            }
        }

        private void BtnEliminarProductoFila_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CarritoItem seleccionado)
            {
                if (MessageBox.Show($"¿Desea eliminar {seleccionado.Producto.Nombre} del carrito?", "Eliminar Producto", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    carrito.Remove(seleccionado);
                    ActualizarCarrito();
                }
            }
        }

        private void BtnIrTicket_Click(object sender, RoutedEventArgs e)
        {
            if (!carrito.Any()) { MessageBox.Show("Carrito vacío."); return; }
            GenerarTicket();
            tabControl.SelectedItem = tabTicket;
        }

        private void GenerarTicket()
        {
            double subtotal = carrito.Sum(i => i.Total);
            double iva = subtotal * 0.21;
            double total = subtotal + iva;
            string ticket = "===== TICKET =====\n";
            ticket += $"{"Cant.",-5} {"Producto",-20} {"Total",10}\n";
            ticket += new string('-', 37) + "\n";
            foreach (var item in carrito) ticket += $"{item.Cantidad,-5} {item.Producto.Nombre,-20} {item.Total,10:F2} €\n";
            ticket += new string('-', 37) + "\n";
            ticket += $"{"Subtotal:",-25} {subtotal,10:F2} €\n";
            ticket += $"{"IVA 21%:",-25} {iva,10:F2} €\n";
            ticket += $"{"TOTAL:",-25} {total,10:F2} €\n";
            tbTicket.Text = ticket;
        }

        private void BtnConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in carrito)
            {
                item.Producto.Stock -= item.Cantidad;
                if (item.Producto.Stock < 0) item.Producto.Stock = 0;
            }
            GuardarProductos();
            MessageBox.Show("Pedido confirmado.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
            carrito.Clear();
            ActualizarProductos();
            ActualizarCarrito();
            tabControl.SelectedItem = tabTPV;
        }

        private void BtnVolverTPV_Click(object sender, RoutedEventArgs e) => tabControl.SelectedItem = tabTPV;

        private void GuardarProductos()
        {
            if (!File.Exists(dbPath)) { MessageBox.Show("No se encontró la base de datos SQLite.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            using var conexion = new SqliteConnection($"Data Source={dbPath}");
            conexion.Open();
            foreach (var p in categorias.Edariak)
            {
                using var cmd = new SqliteCommand("UPDATE Edariak SET Stock=@stock WHERE Id=@id", conexion);
                cmd.Parameters.AddWithValue("@stock", p.Stock);
                cmd.Parameters.AddWithValue("@id", p.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}