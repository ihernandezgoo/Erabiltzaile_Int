using System;
using System.Windows;

namespace DietCalculation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            const int gosaria = 3;
            const int bazkaria = 9;
            const double afaria = 15.5;

            const double kmRate = 0.25;
            const double viajeRate = 18; 

            const double trabajoRate = 42; 



            double total = 0;
            if (chkDesayuno.IsChecked == true) total += gosaria; // GOSARIA
            if (chkComida.IsChecked == true) total += bazkaria; // BAZKARIA
            if (chkCena.IsChecked == true) total += afaria; // AFARIA

            if (double.TryParse(txtKM.Text, out double km))
            {
                total += km * kmRate; // 0.25 km
            }
            if (double.TryParse(txtHorasViaje.Text, out double horasViaje))
            {
                total += horasViaje * viajeRate; // 18 per hour
            }

            if (double.TryParse(txtHorasTrabajo.Text, out double horasTrabajo))
            {
                total += horasTrabajo * trabajoRate; // 42 per hour
            }

            txtTotal.Text = total.ToString("C");
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            // Clear checkboxes
            chkDesayuno.IsChecked = false;
            chkComida.IsChecked = false;
            chkCena.IsChecked = false;

            // Clear textboxes
            txtKM.Clear();
            txtHorasViaje.Clear();
            txtHorasTrabajo.Clear();
            txtTotal.Clear();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}