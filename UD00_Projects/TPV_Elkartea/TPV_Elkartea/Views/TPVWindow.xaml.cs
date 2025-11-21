using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPV_Elkartea.Models;
using TPV_Elkartea.Services;

namespace TPV_Elkartea.Views
{
    public partial class TPVWindow : Window
    {
        private ProductosCategoria categorias;
        private BindingList<CarritoItem> carrito = new BindingList<CarritoItem>();
        private List<Usuario> usuarios = new List<Usuario>();

        private readonly ProductoService productoService;
        private readonly UsuarioService usuarioService;

        public TPVWindow()
        {
            InitializeComponent();

            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            productoService = new ProductoService(System.IO.Path.Combine(baseDir, "produktuak.db"));
            usuarioService = new UsuarioService(baseDir);

            CargarProductos();
            CargarUsuarios();

            dgCarrito.ItemsSource = carrito;
            ActualizarProductos();
        }

        private void CargarProductos()
        {
            categorias = new ProductosCategoria();
            categorias.Edariak.AddRange(productoService.CargarProductos());
        }

        private void CargarUsuarios() => usuarios = usuarioService.CargarUsuarios();

        private void ActualizarProductos() => icProductos.ItemsSource = categorias.Edariak;

        private void icProductos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (icProductos.SelectedItem is Producto seleccionado)
            {
                int cantidad = 1;
                var existente = carrito.FirstOrDefault(x => x.Producto == seleccionado);
                if (existente != null)
                    existente.Cantidad += cantidad;
                else
                    carrito.Add(new CarritoItem { Producto = seleccionado, Cantidad = cantidad });

                ActualizarCarrito();
            }
        }

        private void ActualizarCarrito()
        {
            dgCarrito.Items.Refresh();
            double subtotal = carrito.Sum(i => i.Total);
            double iva = subtotal * 0.21;
            double total = subtotal + iva;
            tbTotal.Text = $"Subtotala: {subtotal:F2} € | BEZ 21%: {iva:F2} € | TOTALA: {total:F2} €";
        }

        private void BtnEditarCantidadFila_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is CarritoItem seleccionado)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Sartu kantitate berria {seleccionado.Producto.Nombre}",
                    "Kantitate berria sartu",
                    seleccionado.Cantidad.ToString()
                );
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
                if (MessageBox.Show(
                        $"Korritotik {seleccionado.Producto.Nombre} produktua ezabatu dezakezu?",
                        "Produktua ezabatu",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    carrito.Remove(seleccionado);
                    ActualizarCarrito();
                }
            }
        }

        private void BtnIrTicket_Click(object sender, RoutedEventArgs e)
        {
            if (!carrito.Any()) { MessageBox.Show("Karritoa hutsik dago"); return; }
            GenerarTicket();
            tabControl.SelectedItem = tabTicket;
        }

        private void GenerarTicket()
        {
            double subtotal = carrito.Sum(i => i.Total);
            double iva = subtotal * 0.21;
            double total = subtotal + iva;

            string ticket = "===== TICKETA =====\n";
            ticket += $"{"Kantitatea.",-5} {"Produktua",-20} {"Totala",10}\n";
            ticket += new string('-', 37) + "\n";
            foreach (var item in carrito)
                ticket += $"{item.Cantidad,-5} {item.Producto.Nombre,-20} {item.Total,10:F2} €\n";
            ticket += new string('-', 37) + "\n";
            ticket += $"{"Subtotala:",-25} {subtotal,10:F2} €\n";
            ticket += $"{"BEZ 21%:",-25} {iva,10:F2} €\n";
            ticket += $"{"TOTALA:",-25} {total,10:F2} €\n";

            tbTicket.Text = ticket;
        }

        private void BtnConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in carrito)
            {
                item.Producto.Stock -= item.Cantidad;
                if (item.Producto.Stock < 0) item.Producto.Stock = 0;
            }

            productoService.GuardarProductos(categorias.Edariak);

            MessageBox.Show("Zure ordera konfirmatu.", "Azkeneko pausuoa", MessageBoxButton.OK, MessageBoxImage.Information);
            carrito.Clear();
            ActualizarProductos();
            ActualizarCarrito();
            tabControl.SelectedItem = tabTPV;
        }

        private void BtnVolverTPV_Click(object sender, RoutedEventArgs e) => tabControl.SelectedItem = tabTPV;
    }
}