using System.ComponentModel;

namespace TPV_Elkartea.Models
{
    public class Producto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        private double precio;
        public double Precio
        {
            get => precio;
            set { precio = value; OnPropertyChanged(nameof(Precio)); }
        }

        private int stock;
        public int Stock
        {
            get => stock;
            set { stock = value; OnPropertyChanged(nameof(Stock)); }
        }

        private string imagen;
        public string Img
        {
            get => imagen;
            set { imagen = value; OnPropertyChanged(nameof(Img)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}