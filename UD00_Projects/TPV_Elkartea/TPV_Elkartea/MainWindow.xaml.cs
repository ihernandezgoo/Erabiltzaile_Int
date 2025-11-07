using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace TPV
{
    public partial class MainWindow : Window
    {
        private ErabiltzaileakData _data;

        public MainWindow()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            string ruta = "db/erabiltzaileak.json";
            string json = File.ReadAllText(ruta);
            _data = JsonSerializer.Deserialize<ErabiltzaileakData>(json);
        }

        private void Sartu_Click(object sender, RoutedEventArgs e)
        {
            var user = _data.usuarios
                .FirstOrDefault(u => u.erabiltzailea == txtUsuario.Text &&
                                     u.pasahitza == txtPin.Password);

            if (user == null)
            {
                MessageBox.Show("Erabiltzailea edo PIN okerra!");
                return;
            }

            if (user.isAdmin)
            {
                AdminChoiceWindow ac = new AdminChoiceWindow(user);
                bool? result = ac.ShowDialog();

                if (result == true)
                {
                    if (ac.IsAdminMode)
                    {
                        // Modo admin → más adelante adminpanel
                        MessageBox.Show("Modo Admin (falta ventana AdminPanel)");
                    }
                    else
                    {
                        TPVView.tpv tpvWin = new TPVView.tpv(user, false);
                        tpvWin.Show();
                    }

                    this.Close();
                }
            }
            else
            {
                TPVView.tpv tpvWin = new TPVView.tpv(user, false);
                tpvWin.Show();
                this.Close();
            }
        }
    }

    public class ErabiltzaileakData
    {
        public Usuario[] usuarios { get; set; }
    }

    public class Usuario
    {
        public int id { get; set; }
        public string izena { get; set; }
        public string erabiltzailea { get; set; }
        public string pasahitza { get; set; }
        public bool isAdmin { get; set; }
    }
}