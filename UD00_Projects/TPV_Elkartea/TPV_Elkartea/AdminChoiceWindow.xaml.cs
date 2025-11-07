using System.Windows;

namespace TPV
{
    public partial class AdminChoiceWindow : Window
    {
        public bool IsAdminMode { get; private set; } = false;

        private Usuario _erabiltzailea;

        public AdminChoiceWindow(Usuario erabiltzailea)
        {
            InitializeComponent();
            _erabiltzailea = erabiltzailea;
        }

        private void UserMode_Click(object sender, RoutedEventArgs e)
        {
            IsAdminMode = false;
            DialogResult = true;
            Close();
        }

        private void AdminMode_Click(object sender, RoutedEventArgs e)
        {
            IsAdminMode = true;
            DialogResult = true;
            Close();
        }
    }
}