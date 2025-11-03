using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace _2._Ariketa
{
    public partial class MainWindow : Window
    {
        private List<string> frases = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            InicializarEstado();
        }

        private void InicializarEstado()
        {
            frases.Clear();
            TextBoxFrase.Clear();
            TextBoxFrase.Focus();

            // Frase 1 bakarrik
            Btn_Frase1.IsEnabled = true;
            Btn_Frase2.IsEnabled = false;
            Btn_Frase3.IsEnabled = false;
            Btn_Frase4.IsEnabled = false;
            Btn_Frase5.IsEnabled = false;
            Unir.IsEnabled = false;
        }

        private void AñadirFraseYActualizar(TextBox textBox, Button botonActual, Button botonSiguiente)
        {
            string texto = textBox.Text.Trim();

            if (!string.IsNullOrEmpty(texto))
            {
                frases.Add(texto);
                textBox.Clear();
                textBox.Focus();

                botonActual.IsEnabled = false;

                if (botonSiguiente != null)
                {
                    botonSiguiente.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Mesedez, sartu esaldi bat lehenengo.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Btn_Frase1_Click(object sender, RoutedEventArgs e)
        {
            AñadirFraseYActualizar(TextBoxFrase, Btn_Frase1, Btn_Frase2);
        }

        private void Btn_Frase2_Click(object sender, RoutedEventArgs e)
        {
            AñadirFraseYActualizar(TextBoxFrase, Btn_Frase2, Btn_Frase3);
        }

        private void Btn_Frase3_Click(object sender, RoutedEventArgs e)
        {
            AñadirFraseYActualizar(TextBoxFrase, Btn_Frase3, Btn_Frase4);
        }

        private void Btn_Frase4_Click(object sender, RoutedEventArgs e)
        {
            AñadirFraseYActualizar(TextBoxFrase, Btn_Frase4, Btn_Frase5);
        }

        private void Btn_Frase5_Click(object sender, RoutedEventArgs e)
        {
            AñadirFraseYActualizar(TextBoxFrase, Btn_Frase5, Unir);
        }

        private void Btn_Unir(object sender, RoutedEventArgs e)
        {
            string resultado = string.Join(" ", frases);
            TextBoxFrase.Text = resultado;

            Unir.IsEnabled = false;
        }

        private void Limpiar(object sender, RoutedEventArgs e)
        {
            InicializarEstado();
        }

        private void Salir(object sender, RoutedEventArgs e) 
        {
            Close();
        }
    }
}