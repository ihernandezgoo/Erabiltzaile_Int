using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _9._Ariketa
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lstAmigos.Items.Add("Ana");
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void lstAmigos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstAmigos.SelectedItem != null)
            {
                txtAmigoSeleccionado.Text = lstAmigos.SelectedItem.ToString();
            }
        }

        private void BtnAñadir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAmigoNuevo.Text))
            {
                MessageBox.Show("Izen bat sartu gehitzeko.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            lstAmigos.Items.Add(txtAmigoNuevo.Text);
            txtAmigoNuevo.Clear();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (lstAmigos.SelectedItem == null)
            {
                MessageBox.Show("Lagun bat aukeratu borratzeko.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            lstAmigos.Items.Remove(lstAmigos.SelectedItem);
            txtAmigoSeleccionado.Clear();
        }

        private void BtnBorrarLista_Click(object sender, RoutedEventArgs e)
        {
            lstAmigos.Items.Clear();
            txtAmigoSeleccionado.Clear();
        }

        private void Atera(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}