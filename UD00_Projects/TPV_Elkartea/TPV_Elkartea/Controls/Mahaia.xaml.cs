using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TPV_Elkartea.Controls
{
    public partial class Mahaia : UserControl
    {
        public enum EgoeraMota { Librea, Hautatua, Okupatua }

        // DependencyProperty bien configurada
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(Mahaia), new PropertyMetadata(""));

        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public EgoeraMota Egoera { get; private set; } = EgoeraMota.Librea;

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
                    IsEnabled = false;
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
            if (Egoera == EgoeraMota.Okupatua)
                return;

            // Alternar entre libre ↔ seleccionado
            if (Egoera == EgoeraMota.Librea)
                SetEgoera(EgoeraMota.Hautatua);
            else if (Egoera == EgoeraMota.Hautatua)
                SetEgoera(EgoeraMota.Librea);
        }
    }
}
