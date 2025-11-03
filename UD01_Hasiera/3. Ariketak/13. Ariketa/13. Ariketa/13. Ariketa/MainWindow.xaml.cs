using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TestuEditorea
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, (s, e) => editor.Cut()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (s, e) => editor.Copy()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (s, e) => editor.Paste()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, (s, e) => editor.SelectedText = ""));
        }

        private void Irten_Click(object sender, RoutedEventArgs e) => Close();

        private void Moztu_Click(object sender, RoutedEventArgs e) => editor.Cut();
        private void Kopiatu_Click(object sender, RoutedEventArgs e) => editor.Copy();
        private void Itsatsi_Click(object sender, RoutedEventArgs e) => editor.Paste();
        private void Ezabatu_Click(object sender, RoutedEventArgs e) => editor.SelectedText = "";

        private void Arial_Click(object sender, RoutedEventArgs e) => editor.FontFamily = new FontFamily("Arial");
        private void Courier_Click(object sender, RoutedEventArgs e) => editor.FontFamily = new FontFamily("Courier New");
        private void Impact_Click(object sender, RoutedEventArgs e) => editor.FontFamily = new FontFamily("Impact");
        private void Symbol_Click(object sender, RoutedEventArgs e) => editor.FontFamily = new FontFamily("Symbol");
    }
}