using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1._Ariketak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Operar(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener los valores de los cuadros de texto
                double a = double.Parse(PrimerNumero.Text, CultureInfo.InvariantCulture);
                double b = double.Parse(SegundoNumero.Text, CultureInfo.InvariantCulture);
                double c = double.Parse(TercerNumero.Text, CultureInfo.InvariantCulture);
                double d = double.Parse(CuartoNumero.Text, CultureInfo.InvariantCulture);

                // Calcular el resultado
                double resultado = (a + 2 * b + 3 * c + 4 * d) / 4;

                // Mostrar el resultado en el cuadro de texto "RESULTADO"
                Resultado.Text = resultado.ToString(CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, introduzca valores numéricos válidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Limpiar(object sender, RoutedEventArgs e)
        {
            // Limpiar los cuadros de texto
            PrimerNumero.Text = string.Empty;
            SegundoNumero.Text = string.Empty;
            TercerNumero.Text = string.Empty;
            CuartoNumero.Text = string.Empty;
            Resultado.Text = string.Empty;

            // Colocar el foco en el primer cuadro de texto
            PrimerNumero.Focus();
        }

        private void Salir(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ValidarEntradaNumerica(object sender, TextCompositionEventArgs e)
        {
            // Validar que solo se puedan introducir números y un punto decimal
            e.Handled = !double.TryParse(((TextBox)sender).Text + e.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _);
        }
    }
}