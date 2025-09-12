using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _3._Ariketa
{
    /// <summary>
    /// Interaction logic for resultWindow.xaml
    /// </summary>
    public partial class resultWindow : Window
    {
        public resultWindow()
        {
            InitializeComponent();
        }

        public void setResult(double result)
        {
            txtResultado.Text = result.ToString();
        }

        private void Garbitu2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Atera2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
