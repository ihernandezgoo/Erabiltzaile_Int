using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Xml.Linq;

namespace AtazaKudeatzailea
{
    public partial class MainWindow : Window
    {
        public class Tarea : INotifyPropertyChanged
        {
            public int Id { get; set; }

            private string titulua;
            public string Titulua
            {
                get => titulua;
                set { titulua = value; OnPropertyChanged(nameof(Titulua)); }
            }

            private string lehentasuna;
            public string Lehentasuna { get => lehentasuna; set { lehentasuna = value; OnPropertyChanged(nameof(Lehentasuna)); } }

            private DateTime azkenEguna;
            public DateTime AzkenEguna { get => azkenEguna; set { azkenEguna = value; OnPropertyChanged(nameof(AzkenEguna)); } }

            private string egoera;
            public string Egoera { get => egoera; set { egoera = value; OnPropertyChanged(nameof(Egoera)); OnPropertyChanged(nameof(Eginda)); } }

            public bool Eginda
            {
                get => Egoera == "Eginda";
                set => Egoera = value ? "Eginda" : "Egin gabe";
            }

            public int EditCount { get; set; } = 0;

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private ObservableCollection<Tarea> Tareas = new ObservableCollection<Tarea>();
        private string XmlPath = "Data/tareas.xml";

        public MainWindow()
        {
            InitializeComponent();
            CargarTareas();
            TareasDataGrid.ItemsSource = Tareas;
        }

        private void CargarTareas()
        {
            if (!System.IO.File.Exists(XmlPath))
            {
                MessageBox.Show($"No se encuentra el archivo XML en: {XmlPath}");
                return;
            }

            try
            {
                var doc = XDocument.Load(XmlPath);
                Tareas.Clear();

                foreach (var x in doc.Descendants("Tarea"))
                {
                    Tareas.Add(new Tarea
                    {
                        Id = (int)x.Attribute("id")!,
                        Titulua = (string)x.Element("Titulua")!,
                        Lehentasuna = (string)x.Element("Lehentasuna")!,
                        AzkenEguna = DateTime.Parse((string)x.Element("AzkenEguna")!),
                        Egoera = (string)x.Element("Egoera")!
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar XML: " + ex.Message);
            }
        }

        private void GordeTareas()
        {
            XDocument doc = new XDocument(new XElement("Tareas",
                Tareas.Select(t =>
                    new XElement("Tarea",
                        new XAttribute("id", t.Id),
                        new XElement("Titulua", t.Titulua),
                        new XElement("Lehentasuna", t.Lehentasuna),
                        new XElement("AzkenEguna", t.AzkenEguna.ToString("yyyy-MM-dd")),
                        new XElement("Egoera", t.Egoera)
                    )
                )
            ));
            doc.Save(XmlPath);
        }

        private void Gehitu_Click(object sender, RoutedEventArgs e)
        {
            var t = new Tarea
            {
                Id = Tareas.Any() ? Tareas.Max(x => x.Id) + 1 : 1,
                Titulua = "Izenburu berria",
                Lehentasuna = "Ertaina",
                AzkenEguna = DateTime.Today,
                Egoera = "Egin gabe"
            };
            Tareas.Add(t);
            GordeTareas();
        }

        private void Editatu_Click(object sender, RoutedEventArgs e)
        {
            if (TareasDataGrid.SelectedItem is Tarea t)
            {
                t.EditCount++;
                t.Titulua = $" [{t.EditCount}]: {t.Titulua.Split(':').Last().Trim()}";
                GordeTareas();
            }
        }

        private void Ezabatu_Click(object sender, RoutedEventArgs e)
        {
            if (TareasDataGrid.SelectedItem is Tarea t)
            {
                Tareas.Remove(t);
                GordeTareas();
            }
        }

        private void Irten_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}