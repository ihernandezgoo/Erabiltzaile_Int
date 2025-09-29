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
                string nombre = selectedItem.Content.ToString();
                string fileName = "";

                switch (nombre)
                {
                    case "1. Argazkia": fileName = "argazkia1.jpg"; break;
                    case "2. Argazkia": fileName = "argazkia2.jpg"; break;
                    case "3. Argazkia": fileName = "argazkia3.jpg"; break;
                }

                SeleccionImagen.Source = CargarImagen(fileName);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Fila2Imagen1.Source = CheckImagen4.IsChecked == true ? CargarImagen("argazkia4.jpg") : null;
            Fila2Imagen2.Source = CheckImagen5.IsChecked == true ? CargarImagen("argazkia5.jpg") : null;
            Fila2Imagen3.Source = CheckImagen6.IsChecked == true ? CargarImagen("argazkia6.jpg") : null;
        }

        // Método auxiliar para cargar imágenes desde /argazkiak/
        private BitmapImage CargarImagen(string nombreArchivo)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "argazkiak", nombreArchivo);
            if (File.Exists(ruta))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(ruta, UriKind.Absolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                return bmp;
            }
            return null;
        }

        // Botón salir
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}