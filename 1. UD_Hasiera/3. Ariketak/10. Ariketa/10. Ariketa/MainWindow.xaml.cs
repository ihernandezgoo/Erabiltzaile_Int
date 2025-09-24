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

namespace _10._Ariketa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Seleccionamos por defecto el segundo item (Imagen 2)
            ImagenComboBox.SelectedIndex = 1;
        }

        private void Atera(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Fila1ComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (ImagenComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selected = selectedItem.Content.ToString();

                string imagePath = "";

                switch (selected)
                {
                    case "Imagen 1":
                        imagePath = "argazkiak/imagen1.jpg";
                        break;
                    case "Imagen 2":
                        imagePath = "argazkiak/imagen2.jpg";
                        break;
                    case "Imagen 3":
                        imagePath = "argazkiak/imagen3.jpg";
                        break;
                }

                if (!string.IsNullOrEmpty(imagePath))
                {
                    Uri uri = new Uri(imagePath, UriKind.Relative);
                    BitmapImage bitmap = new BitmapImage(uri);
                    SeleccionImagen.Source = bitmap;
                }
            }
        }
    }
}