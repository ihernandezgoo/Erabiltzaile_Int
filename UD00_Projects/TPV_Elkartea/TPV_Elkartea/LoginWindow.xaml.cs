using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Windows;
using TPV_Elkartea.Views;
using TPV_Elkartea.Models;

namespace TPV_Elkartea
{
    public partial class LoginWindow : Window
    {
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "erabiltzaileak.db");

        public LoginWindow()
        {
            InitializeComponent();

            if (!File.Exists(dbPath))
            {
                MessageBox.Show("Erabiltzaileen datu-basea ez da aurkitu.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string izena = tbUsuario.Text.Trim();
            string pasahitza = pbContrasena.Password.Trim();

            if (string.IsNullOrEmpty(izena) || string.IsNullOrEmpty(pasahitza))
            {
                MessageBox.Show("Sartu erabiltzaile-izena eta pasahitza.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool loginOk = false;

            try
            {
                using var conexion = new SqliteConnection($"Data Source={dbPath}");
                conexion.Open();

                string query = "SELECT Rola FROM Erabiltzaileak WHERE Izena=@izena AND Pasahitza=@pasahitza";
                using var cmd = new SqliteCommand(query, conexion);
                cmd.Parameters.AddWithValue("@izena", izena);
                cmd.Parameters.AddWithValue("@pasahitza", pasahitza);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    loginOk = true;
                    string rola = result.ToString();

                    if (rola.Equals("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        var admin = new AdminWindow();
                        admin.Show();
                    }
                    else
                    {
                        var tpv = new TPVWindow();
                        tpv.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea erabiltzailea balidatzen: {ex.Message}", "Errorea", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!loginOk)
            {
                MessageBox.Show("Erabiltzaile-izena edo pasahitza okerra.", "Errorea", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}