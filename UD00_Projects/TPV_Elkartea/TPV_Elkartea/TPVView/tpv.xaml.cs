using System.Windows;

namespace TPV.TPVView
{
    public partial class tpv : Window
    {
        private Usuario _erabiltzailea;
        private bool _isAdmin;

        public tpv(Usuario erabiltzailea, bool isAdmin = false)
        {
            InitializeComponent();
            _erabiltzailea = erabiltzailea;
            _isAdmin = isAdmin;

            lblUser.Text = isAdmin
                ? $"[ADMIN] {_erabiltzailea.izena}"
                : _erabiltzailea.izena;
        }
    }
}