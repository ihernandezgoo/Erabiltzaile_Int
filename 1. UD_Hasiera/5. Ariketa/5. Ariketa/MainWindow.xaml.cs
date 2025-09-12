using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;

namespace _5._Ariketa
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComicsSans(object sender, RoutedEventArgs e)
        {
            Testua1.FontFamily = new FontFamily("Comic Sans MS");
        }

        private void Negrita(object sender, RoutedEventArgs e)
        {
            Testua1.FontWeight = FontWeights.Bold;
        }

        private void Tachado(object sender, RoutedEventArgs e)
        {
            Testua1.TextDecorations = TextDecorations.Strikethrough;
        }

        private void TamañoMas(object sender, RoutedEventArgs e)
        {
            Testua1.FontSize += 2;
        }

        private void TamañoMenos(object sender, RoutedEventArgs e)
        {
            if (Testua1.FontSize > 2)
                Testua1.FontSize -= 2;
        }

        private void Courtier(object sender, RoutedEventArgs e)
        {
            Testua1.FontFamily = new FontFamily("Courier New");
        }

        private void Cursiva(object sender, RoutedEventArgs e)
        {
            Testua1.FontStyle = FontStyles.Italic;
        }

        private void Subrayado(object sender, RoutedEventArgs e)
        {
            Testua1.TextDecorations = TextDecorations.Underline;
        }
    }
}