using System.Windows;

namespace _11._Ariketa
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Onartu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Datuak gorde dira.");
        }

        private void Begiratu_Click(object sender, RoutedEventArgs e)
        {
            lblBienvenida.Content = "Ongietorri sistemara\n" + "\n" +
                                    txtIzena.Text + " " +
                                    txtAbizena1.Text + " " +
                                    txtAbizena2.Text + "\n" +
                                    txtNan.Text;

            PanelFormulario.Visibility = Visibility.Collapsed;
            PanelVisualizar.Visibility = Visibility.Visible;
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            PanelVisualizar.Visibility = Visibility.Collapsed;
            PanelFormulario.Visibility = Visibility.Visible;
        }

        private void Atera_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}