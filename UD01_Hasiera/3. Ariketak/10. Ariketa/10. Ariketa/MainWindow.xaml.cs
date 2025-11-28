using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace _10._Ariketa
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ImagenComboBox.SelectedIndex = 0;
        }

        private void ImagenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImagenComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string izena = selectedItem.Content.ToString();
                string fitxategiIzena = "";

                switch (izena)
                {
                    case "1. Argazkia": fitxategiIzena = "argazkia1.jpg"; break;
                    case "2. Argazkia": fitxategiIzena = "argazkia2.jpg"; break;
                    case "3. Argazkia": fitxategiIzena = "argazkia3.jpg"; break;
                }

                SeleccionImagen.Source = KargatuIrudia(fitxategiIzena);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Fila2Imagen1.Source = CheckImagen4.IsChecked == true ? KargatuIrudia("argazkia4.jpg") : null;
            Fila2Imagen2.Source = CheckImagen5.IsChecked == true ? KargatuIrudia("argazkia5.jpg") : null;
            Fila2Imagen3.Source = CheckImagen6.IsChecked == true ? KargatuIrudia("argazkia6.jpg") : null;
        }

        // Metodo laguntzailea irudiak /argazkiak/-tik kargatzeko
        private BitmapImage KargatuIrudia(string fitxategiIzena)
        {
            string bidea = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "argazkiak", fitxategiIzena);
            if (File.Exists(bidea))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(bidea, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                return bmp;
            }
            return null;
        }

        // Irten botoia
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}