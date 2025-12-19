using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TPV_Elkartea.Controls
{
    public partial class Mahaia : UserControl
    {
        public enum EgoeraMota { Librea, Hautatua, Okupatua }

        // DependencyProperty ondo konfiguratuta
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(Mahaia), new PropertyMetadata(""));

        public static readonly DependencyProperty ReservationNameProperty =
            DependencyProperty.Register("ReservationName", typeof(string), typeof(Mahaia), new PropertyMetadata(""));

        public static readonly DependencyProperty ReservationTimeProperty =
            DependencyProperty.Register("ReservationTime", typeof(string), typeof(Mahaia), new PropertyMetadata(""));

        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public string ReservationName
        {
            get => (string)GetValue(ReservationNameProperty);
            set => SetValue(ReservationNameProperty, value);
        }

        public string ReservationTime
        {
            get => (string)GetValue(ReservationTimeProperty);
            set => SetValue(ReservationTimeProperty, value);
        }

        public EgoeraMota Egoera { get; private set; } = EgoeraMota.Librea;
        public string ErreserbaIzena { get; private set; } = "";
        public string ErreserbaOrdua { get; private set; } = "";

        // Mahaia klik egitean jakinarazteko gertaera
        public event EventHandler<string>? MahaiaKlikatu;

        public Mahaia()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Mahaia(string id) : this()
        {
            Id = id;
        }

        public void SetEgoera(EgoeraMota egoera)
        {
            Egoera = egoera;
            EguneratuItxura();
        }

        public void Erreserbatu(string izena, string ordua)
        {
            ErreserbaIzena = izena;
            ErreserbaOrdua = ordua;
            ReservationName = izena;
            ReservationTime = ordua;
            SetEgoera(EgoeraMota.Okupatua);
        }

        public void Askatu()
        {
            ErreserbaIzena = "";
            ErreserbaOrdua = "";
            ReservationName = "";
            ReservationTime = "";
            SetEgoera(EgoeraMota.Librea);
        }

        private void EguneratuItxura()
        {
            switch (Egoera)
            {
                case EgoeraMota.Librea:
                    SeatBorder.Background = Brushes.LightGreen;
                    IsEnabled = true;
                    break;

                case EgoeraMota.Hautatua:
                    SeatBorder.Background = Brushes.Yellow;
                    IsEnabled = true;
                    break;

                case EgoeraMota.Okupatua:
                    SeatBorder.Background = Brushes.Red;
                    IsEnabled = true;
                    break;
            }
        }

        private void SeatBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            SeatBorder.BorderBrush = Brushes.Blue;
            SeatBorder.BorderThickness = new Thickness(2);
        }

        private void SeatBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            SeatBorder.BorderBrush = Brushes.Gray;
            SeatBorder.BorderThickness = new Thickness(1);
        }

        private void SeatBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TPVWindow-k klik-a kudeatu dezan gertaera jaurti
            MahaiaKlikatu?.Invoke(this, Id);
        }
    }
}
