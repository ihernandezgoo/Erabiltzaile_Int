using System;
using System.Globalization;
using System.Windows;
using Microsoft.VisualBasic;

namespace SimpleDateApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEjecutar_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;

            txtAhora.Text = now.ToString("dd/MM/yyyy HH:mm:ss");
            txtHoy.Text = now.ToString("dd/MM/yyyy");
            txtHoraHoy.Text = now.ToString("HH:mm:ss");

            string inputDate = Interaction.InputBox("Ingrese una fecha (dd/mm/yyyy):", "Fecha Inicial", "01/01/2011");

            if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                string inputMeses = Interaction.InputBox("¿Cuántos meses desea sumar?", "Meses a Sumar", "1");

                if (int.TryParse(inputMeses, out int meses))
                {
                    DateTime nuevaFecha = fecha.AddMonths(meses);
                    txtSumaFechas.Text = nuevaFecha.ToString("dd/MM/yyyy");
                }
                else
                {
                    MessageBox.Show("Número de meses inválido.");
                }
            }
            else
            {
                MessageBox.Show("Fecha inválida.");
            }

            DateTime fechaBase = new DateTime(2011, 1, 1);
            TimeSpan diferencia = now - fechaBase;
            txtDiferenciaFechas.Text = $"{diferencia.Days} días";
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtAhora.Clear();
            txtHoy.Clear();
            txtHoraHoy.Clear();
            txtSumaFechas.Clear();
            txtDiferenciaFechas.Clear();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}