using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPV_Elkartea.Models;
using TPV_Elkartea.Services;
using TPV_Elkartea.Controls;

namespace TPV_Elkartea.Views
{
    public partial class TPVWindow : Window
    {
        private ProductosCategoria categorias;
        private BindingList<CarritoItem> carrito = new BindingList<CarritoItem>();
        private List<Usuario> usuarios = new List<Usuario>();

        private readonly ProductoService productoService;
        private readonly UsuarioService usuarioService;
        
        private string usuarioActual;
        private List<Mahaia> mahaiak = new List<Mahaia>();

        public TPVWindow() : this("Gonbidatua")
        {
        }

        public TPVWindow(string erabiltzaileIzena)
        {
            InitializeComponent();

            usuarioActual = erabiltzaileIzena;
            tbUsuarioActual.Text = erabiltzaileIzena;

            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            productoService = new ProductoService(System.IO.Path.Combine(baseDir, "produktuak.db"));
            usuarioService = new UsuarioService(baseDir);

            CargarProductos();
            CargarUsuarios();
            InicializarSelectorHora();
            InicializarMahaiak();

            dgCarrito.ItemsSource = carrito;
            ActualizarProductos();
        }

        private void InicializarSelectorHora()
        {
            // Orduak bete (12:00 - 23:00)
            for (int i = 12; i <= 23; i++)
            {
                cbHoras.Items.Add(i.ToString("D2"));
            }
            cbHoras.SelectedIndex = 0;

            // Minutuak bete (00, 15, 30, 45)
            cbMinutos.Items.Add("00");
            cbMinutos.Items.Add("15");
            cbMinutos.Items.Add("30");
            cbMinutos.Items.Add("45");
            cbMinutos.SelectedIndex = 0;
        }

        private void InicializarMahaiak()
        {
            // 12 mahaia sortu
            for (int i = 1; i <= 12; i++)
            {
                var mahaia = new Mahaia($"M{i}");
                mahaia.MahaiaKlikatu += Mahaia_Klikatu;
                mahaiak.Add(mahaia);
                wpMahaiak.Children.Add(mahaia);
            }
        }

        private void Mahaia_Klikatu(object? sender, string mahaiaId)
        {
            if (sender is Mahaia mahaia)
            {
                if (mahaia.Egoera == Mahaia.EgoeraMota.Librea)
                {
                    // Hautatutako ordua lortu
                    string hora = cbHoras.SelectedItem?.ToString() ?? "12";
                    string minutos = cbMinutos.SelectedItem?.ToString() ?? "00";
                    string ordua = $"{hora}:{minutos}";

                    // Mahaia erreserbatu
                    var result = MessageBox.Show(
                        $"{mahaiaId} mahaia erreserbatu nahi duzu {usuarioActual} izenarekin {ordua}(e)tan?",
                        "Erreserbatu mahaia",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        mahaia.Erreserbatu(usuarioActual, ordua);
                        MessageBox.Show($"{mahaiaId} mahaia erreserbatu da {ordua}(e)tan!", "Erreserbatuta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else if (mahaia.Egoera == Mahaia.EgoeraMota.Okupatua)
                {
                    // Erreserbaren informazioa erakutsi eta askatzeko aukera eman
                    var result = MessageBox.Show(
                        $"{mahaiaId} mahaia {mahaia.ErreserbaIzena}-(e)k erreserbatuta dago {mahaia.ErreserbaOrdua}(e)tan.\nAskatu nahi duzu?",
                        "Mahaia okupatuta",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);

                    if (result == MessageBoxResult.Yes)
                    {
                        mahaia.Askatu();
                        MessageBox.Show($"{mahaiaId} mahaia askatu da!", "Askatuta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void BtnGarbituMahaiak_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Mahaia guztiak askatu nahi dituzu?",
                "Garbitu mahaiak",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var mahaia in mahaiak)
                {
                    mahaia.Askatu();
                }
                MessageBox.Show("Mahaia guztiak askatu dira!", "Garbituta", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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