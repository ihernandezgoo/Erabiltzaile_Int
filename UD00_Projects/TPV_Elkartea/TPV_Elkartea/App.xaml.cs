using System.Configuration;
using System.Data;
using System.Windows;
using TPV_Elkartea.Views;

namespace TPV_Elkartea
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var login = new LoginWindow();
            bool? result = login.ShowDialog();

            // If the dialog was accepted (e.g., user logged in), open the TPV window
            if (result == true)
            {
                var tpv = new TPVWindow();
                tpv.Show();
            }
            else
            {
                // Otherwise shutdown the application
                Shutdown();
            }
        }
    }

}
