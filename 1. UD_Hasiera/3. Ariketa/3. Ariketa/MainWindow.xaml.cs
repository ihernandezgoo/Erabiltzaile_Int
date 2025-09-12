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

namespace _3._Ariketa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<int> numbers = new List<int>();
        private int currentIndex = 1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Hurrengoa(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Testua.Text, out int number))
            {
                numbers.Add(number);
                Testua.Clear();

                if (currentIndex < 4)
                {
                    currentIndex++;
                    Zenbakiak.Content = $"{currentIndex}. Zenbakia";

                } else
                {
                    resultWindow result = new resultWindow();
                    result.setResult(CalculateResult());
                    result.Show();
                    this.Close();
                }
            } else
            {
                MessageBox.Show("Mesedez, sartu zenbaki bat.");
            }
        }

        private double CalculateResult()
        {
            return (numbers[0] + (numbers[0] * numbers[1]) + (numbers[1] * numbers[2]) + (numbers[2] * numbers[3])) / 4.0;
        }

        private void Atera(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}