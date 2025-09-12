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

namespace _6._Ariketa
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is TextBox currentTextBox)
                {
                    string text = currentTextBox.Text;
                    currentTextBox.Text = string.Empty;

                    if (currentTextBox == TextBox1)
                    {
                        TextBox2.Text = text;
                    }
                    else if (currentTextBox == TextBox2)
                    {
                        TextBox3.Text = text;
                    }
                    else if (currentTextBox == TextBox3)
                    {
                        TextBox1.Text = text;
                    }
                }
            }
        }
    }
}