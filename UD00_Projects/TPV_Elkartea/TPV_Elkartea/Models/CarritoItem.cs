using System.ComponentModel;

namespace TPV_Elkartea.Models
{
    public class CarritoItem : INotifyPropertyChanged
    {
        public Producto Producto { get; set; }

        private int cantidad;
        public int Cantidad
        {
            get => cantidad;
            set { cantidad = value; OnPropertyChanged(nameof(Cantidad)); OnPropertyChanged(nameof(Total)); }
        }

        public double Total => Producto.Precio * Cantidad;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}