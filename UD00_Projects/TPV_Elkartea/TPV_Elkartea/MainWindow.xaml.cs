using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TPV_WPF
{
    public class Producto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        private double precio;
        public double Precio { get => precio; set { precio = value; OnPropertyChanged(nameof(Precio)); } }
        private int stock;
        public int Stock { get => stock; set { stock = value; OnPropertyChanged(nameof(Stock)); } }
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
        public List<Producto> Comida { get; set; } = new List<Producto>();
        public List<Producto> Bebida { get; set; } = new List<Producto>();
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
    }

    public partial class MainWindow : Window
    {
        private ProductosCategoria categorias;
        private BindingList<CarritoItem> carrito = new BindingList<CarritoItem>();
        private List<Usuario> usuarios = new List<Usuario>();

        public MainWindow()
        {
            InitializeComponent();
            CargarProductos();
            CargarUsuarios();
            dgCarrito.ItemsSource = carrito;

            cbCategoria.ItemsSource = new List<string> { "Comida", "Bebida" };
            cbCategoria.SelectedIndex = 0;
            ActualizarProductos();
        }

        private void CargarProductos()
        {
            if (!File.Exists("productos.json"))
            {
                categorias = new ProductosCategoria();
                return;
            }
            string json = File.ReadAllText("productos.json");
            categorias = JsonSerializer.Deserialize<ProductosCategoria>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new ProductosCategoria();
        }

        private void CargarUsuarios()
        {
            if (!File.Exists("usuarios.json"))
            {
                usuarios = new List<Usuario>();
                return;
            }
            string json = File.ReadAllText("usuarios.json");
            usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Usuario>();
        }

        private void ActualizarProductos()
        {
            if (cbCategoria.SelectedIndex == 0) icProductos.ItemsSource = categorias.Comida;
            else icProductos.ItemsSource = categorias.Bebida;
        }

        private void CbCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e) => ActualizarProductos();

        // Doble clic agrega automáticamente 1 unidad del producto al carrito
        private void icProductos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (icProductos.SelectedItem is Producto seleccionado)
            {
                int cantidad = 1; // siempre agrega 1 al carrito
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
            tbTotal.Text = $"Subtotal: ${subtotal:F2} | IVA 21%: ${iva:F2} | TOTAL: ${total:F2}";
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
            foreach (var item in carrito) ticket += $"{item.Cantidad,-5} {item.Producto.Nombre,-20} ${item.Total,10:F2}\n";
            ticket += new string('-', 37) + "\n";
            ticket += $"{"Subtotal:",-25} ${subtotal,10:F2}\n";
            ticket += $"{"IVA 21%:",-25} ${iva,10:F2}\n";
            ticket += $"{"TOTAL:",-25} ${total,10:F2}\n";

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

        private void GuardarProductos() => File.WriteAllText("productos.json", JsonSerializer.Serialize(categorias, new JsonSerializerOptions { WriteIndented = true }));

        // Métodos para guardar JSON de productos y usuarios desde los TextBox del editor
        private void BtnGuardarProductos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tbJsonProductos.Text))
                {
                    categorias = JsonSerializer.Deserialize<ProductosCategoria>(tbJsonProductos.Text) ?? new ProductosCategoria();
                    GuardarProductos();
                    MessageBox.Show("Productos guardados correctamente.");
                    ActualizarProductos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar productos: {ex.Message}");
            }
        }

        private void BtnGuardarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tbJsonUsuarios.Text))
                {
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(tbJsonUsuarios.Text) ?? new List<Usuario>();
                    File.WriteAllText("usuarios.json", JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true }));
                    MessageBox.Show("Usuarios guardados correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuarios: {ex.Message}");
            }
        }
    }
}