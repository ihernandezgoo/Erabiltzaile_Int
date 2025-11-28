using System.Windows;
using System.Linq;

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
            MessageBox.Show("Datuak gorde dira (aukerakoa).");
        }

        private void Begiratu_Click(object sender, RoutedEventArgs e)
        {
            // Eremu guztiak beteta daudela egiaztatu
            if (!BalidatuEremuak())
            {
                MessageBox.Show("Eremu guztiak bete behar dira.", 
                               "Errorea", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                return;
            }

            // NAN-aren balioztapena
            if (!BalidatuNan(txtNan.Text))
            {
                MessageBox.Show("NAN-ak gehienez 7 karaktere izan behar ditu eta gutxienez zenbaki bat izan behar du.", 
                               "Errorea", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                return;
            }

            // Ongi etorri mezua eraikitzen dugu erabiltzailearen datuekin
            lblBienvenida.Content = "Ongietorri sistemara\n" + "\n" +
                                    txtIzena.Text + " " +
                                    txtAbizena1.Text + " " +
                                    txtAbizena2.Text + "\n" +
                                    txtNan.Text;

            // Formulario panela ezkutatu eta bistaratze panela erakutsi
            PanelFormulario.Visibility = Visibility.Collapsed;
            PanelVisualizar.Visibility = Visibility.Visible;
        }

        // Irten botoiaren klik event-a - Formulariora itzuli
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            // Bistaratze panela ezkutatu eta formulario panela erakutsi
            PanelVisualizar.Visibility = Visibility.Collapsed;
            PanelFormulario.Visibility = Visibility.Visible;
        }

        // Atera botoiaren klik event-a - Aplikazioa itxi
        private void Atera_Click(object sender, RoutedEventArgs e)
        {
            // Aplikazioa itxi
            this.Close();
        }

        private void txtNan_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Gehienez 7 karaktere baimentzen dugu
            if (txtNan.Text.Length > 7)
            {
                txtNan.Text = txtNan.Text.Substring(0, 7);
                txtNan.CaretIndex = txtNan.Text.Length;
            }
        }

        // Eremu guztiak beteta daudela balioztatu
        private bool BalidatuEremuak()
        {
            // Izena hutsik
            if (string.IsNullOrWhiteSpace(txtIzena.Text))
                return false;

            // Lehen abizena hutsik
            if (string.IsNullOrWhiteSpace(txtAbizena1.Text))
                return false;

            // Bigarren abizena hutsik
            if (string.IsNullOrWhiteSpace(txtAbizena2.Text))
                return false;

            // NAN hutsik
            if (string.IsNullOrWhiteSpace(txtNan.Text))
                return false;

            return true;
        }

        // NAN-a balioztatu: gehienez 7 karaktere eta gutxienez zenbaki bat
        private bool BalidatuNan(string nan)
        {
            // Hutsik badago
            if (string.IsNullOrWhiteSpace(nan))
                return false;

            // 7 karaktere baino gehiago baditu
            if (nan.Length > 7)
                return false;

            if (!nan.Any(char.IsDigit))
                return false;

            return true;
        }
    }
}