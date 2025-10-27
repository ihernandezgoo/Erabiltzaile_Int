using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReservaAsientosWPF
{
    public partial class MainWindow : Window
    {
        List<Zona> zonas = new List<Zona>();
        string archivo = "asientos.json";
        Zona zonaActual;

        public MainWindow()
        {
            InitializeComponent();
            CargarDatos();
            ComboZona.ItemsSource = zonas;
            ComboZona.DisplayMemberPath = "Tipo";
            ComboZona.SelectedIndex = 0;
        }

        private void CargarDatos()
        {
            if (File.Exists(archivo))
            {
                zonas = JsonSerializer.Deserialize<List<Zona>>(File.ReadAllText(archivo));
            }
            else
            {
                zonas.Add(new Zona("Autobús", 10));
                zonas.Add(new Zona("Tren", 10));
                zonas.Add(new Zona("Avión", 10));
            }
        }

        private void GuardarDatos()
        {
            File.WriteAllText(archivo, JsonSerializer.Serialize(zonas));
        }

        private void ComboZona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zonaActual = zonas[ComboZona.SelectedIndex];
            MostrarAsientos();
        }

        private void MostrarAsientos()
        {
            PanelAsientos.Children.Clear();

            foreach (var asiento in zonaActual.Asientos)
            {
                StackPanel stack = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(5) };

                TextBlock txtNumero = new TextBlock
                {
                    Text = asiento.Numero.ToString(),
                    TextAlignment = TextAlignment.Center,
                    Width = 50
                };
                stack.Children.Add(txtNumero);

                TextBlock txtNombre = new TextBlock
                {
                    Text = asiento.ReservadoPor,
                    TextAlignment = TextAlignment.Center,
                    Width = 50
                };
                stack.Children.Add(txtNombre);

                if (asiento.Ocupado)
                {
                    Button btnCancelar = new Button
                    {
                        Content = "Cancelar",
                        Width = 50,
                        Height = 30,
                        Background = Brushes.Red
                    };

                    btnCancelar.Click += (s, e) =>
                    {
                        if (TxtNombre.Text != asiento.ReservadoPor)
                        {
                            MessageBox.Show("No puede cancelar la reserva de otro usuario.");
                            return;
                        }

                        asiento.Ocupado = false;
                        asiento.ReservadoPor = "";
                        GuardarDatos();
                        MostrarAsientos();
                    };

                    stack.Children.Add(btnCancelar);
                }
                else
                {
                    stack.MouseLeftButtonDown += (s, e) =>
                    {
                        if (string.IsNullOrWhiteSpace(TxtNombre.Text))
                        {
                            MessageBox.Show("Ingrese un nombre antes de reservar.");
                            return;
                        }

                        asiento.Ocupado = true;
                        asiento.ReservadoPor = TxtNombre.Text;
                        GuardarDatos();
                        MostrarAsientos();
                    };

                    stack.MouseEnter += (s, e) => stack.Background = Brushes.LightGreen;
                    stack.MouseLeave += (s, e) => stack.Background = Brushes.Transparent;
                }

                PanelAsientos.Children.Add(stack);
            }
        }

        private void BtnReservar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Seleccione un asiento libre haciendo clic sobre él para reservar.");
        }
    }

    public class Asiento
    {
        public int Numero { get; set; }
        public bool Ocupado { get; set; } = false;
        public string ReservadoPor { get; set; } = "";
    }

    public class Zona
    {
        public string Tipo { get; set; }
        public List<Asiento> Asientos { get; set; } = new List<Asiento>();

        public Zona() { }

        public Zona(string tipo, int totalAsientos)
        {
            Tipo = tipo;
            for (int i = 1; i <= totalAsientos; i++)
                Asientos.Add(new Asiento { Numero = i });
        }

        public override string ToString() => Tipo;
    }
}