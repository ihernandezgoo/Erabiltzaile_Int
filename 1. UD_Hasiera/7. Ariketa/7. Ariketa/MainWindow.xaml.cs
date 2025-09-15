using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;

namespace _7._Ariketa
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                Erantzuna.Text += btn.Content.ToString();
            }
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Erantzuna.Text))
            {
                Erantzuna.Text = Erantzuna.Text.Substring(0, Erantzuna.Text.Length - 1);
            }
        }

        private void CE_Click(object sender, RoutedEventArgs e)
        {
            Erantzuna.Text = string.Empty;
        }

        private void Berdin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expr = Erantzuna.Text.Replace("%", "/100.0*");
                var result = new DataTable().Compute(expr, null);
                Erantzuna.Text = result.ToString();
            }
            catch
            {
                Erantzuna.Text = "Errorea";
            }
        }
    }
}