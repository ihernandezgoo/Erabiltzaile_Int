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

namespace _4._Ariketa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
       
        {
        String erabiltzailea = "admin";
        String pasahitza = "admin";


        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Garbitu(object sender, RoutedEventArgs e)
        {
            ErabiltzaileTxt.Clear();
            PasahitzaTxt.Clear();
        }

        private void Atera(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Sartu(object sender, RoutedEventArgs e)
        {
            String erabiltzailea = ErabiltzaileTxt.Text;
            String pasahitza = PasahitzaTxt.Password;

            if (!erabiltzailea.Equals("admin") || !pasahitza.Equals("admin"))
            {
                Emaitza.Text = "Erabiltzailea edo pasahitza gaizki daude, berriro saiatu";
            }
            else
            {
                Emaitza.Text = "Ongi etorri: " + erabiltzailea;
            }
        }
    }
}