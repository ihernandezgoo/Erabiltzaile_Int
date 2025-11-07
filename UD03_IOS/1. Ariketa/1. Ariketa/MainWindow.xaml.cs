using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _1._Ariketa
{
    public partial class MainWindow : Window
    {
        private Garraioa _uneanErakutsitakoGarraioa;
        private DatuenKudeatzailea _datuenKudeatzailea;
        private List<Erreserba> _gordetakoErreserbak;

        public MainWindow()
        {
            InitializeComponent();
            _datuenKudeatzailea = new DatuenKudeatzailea("erreserbak.json");
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _gordetakoErreserbak = await _datuenKudeatzailea.KargatuErreserbakAsync();
            erreserbaDatePicker.SelectedDate = DateTime.Today;
            erreserbaDatePicker.SelectedDateChanged += (s, a) => EguneratuEserlekuenInterfazea();
        }

        private void GarraioMotaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (garraioMotaComboBox.SelectedItem == null) return;
            string aukeratutakoMota = ((ComboBoxItem)garraioMotaComboBox.SelectedItem).Content.ToString();

            switch (aukeratutakoMota)
            {
                case "Autobusa": _uneanErakutsitakoGarraioa = new Autobusa(); break;
                case "Trena": _uneanErakutsitakoGarraioa = new Trena(); break;
                case "Hegazkina": _uneanErakutsitakoGarraioa = new Hegazkina(); break;
            }

            EguneratuEserlekuenInterfazea();
        }

        private void EguneratuEserlekuenInterfazea()
        {
            if (_uneanErakutsitakoGarraioa == null) return;

            eserlekuenGrid.Children.Clear();
            eserlekuenGrid.RowDefinitions.Clear();
            eserlekuenGrid.ColumnDefinitions.Clear();

            var data = erreserbaDatePicker.SelectedDate ?? DateTime.Today;
            var egunekoErreserbak = _gordetakoErreserbak
                .Where(r => r.GarraioMota == _uneanErakutsitakoGarraioa.Mota && r.ErreserbaData.Date == data.Date)
                .SelectMany(r => r.EserlekuakId)
                .ToList();

            int kolPerRow = (_uneanErakutsitakoGarraioa.Mota == "Hegazkina") ? 6 : 4;
            int pasillo = (_uneanErakutsitakoGarraioa.Mota == "Hegazkina") ? 3 : 2;
            int colCount = kolPerRow + 1;
            int rowCount = (int)Math.Ceiling((double)_uneanErakutsitakoGarraioa.Eserlekuak.Count / kolPerRow);

            for (int i = 0; i < colCount; i++)
                eserlekuenGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < rowCount; i++)
                eserlekuenGrid.RowDefinitions.Add(new RowDefinition());

            int row = 0, col = 0;
            foreach (var seat in _uneanErakutsitakoGarraioa.Eserlekuak)
            {
                if (col == pasillo) col++;

                if (egunekoErreserbak.Contains(seat.Id))
                    seat.SetEgoera(SeatControl.EgoeraMota.Okupatua);
                else
                    seat.SetEgoera(SeatControl.EgoeraMota.Librea);

                Grid.SetRow(seat, row);
                Grid.SetColumn(seat, col);
                eserlekuenGrid.Children.Add(seat);

                col++;
                if (col >= colCount)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private async void ErreserbatuButton_Click(object sender, RoutedEventArgs e)
        {
            if (erreserbaDatePicker.SelectedDate == null || erreserbaDatePicker.SelectedDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Mesedez, aukeratu etorkizuneko data bat.");
                return;
            }

            if (_uneanErakutsitakoGarraioa == null)
            {
                MessageBox.Show("Mesedez, aukeratu garraio mota bat lehenik.");
                return;
            }

            var hautatutakoak = _uneanErakutsitakoGarraioa.Eserlekuak
                .Where(s => s.Egoera == SeatControl.EgoeraMota.Hautatua)
                .Select(s => s.Id)
                .ToList();

            if (!hautatutakoak.Any())
            {
                MessageBox.Show("Ez duzu eserlekurik hautatu.");
                return;
            }

            var berria = new Erreserba
            {
                ErabiltzaileIzena = "Proba Erabiltzailea",
                GarraioMota = _uneanErakutsitakoGarraioa.Mota,
                ErreserbaData = erreserbaDatePicker.SelectedDate.Value,
                EserlekuakId = hautatutakoak
            };

            _gordetakoErreserbak.Add(berria);
            await _datuenKudeatzailea.GordeErreserbakAsync(_gordetakoErreserbak);

            MessageBox.Show("Erreserba ondo gorde da.");
            EguneratuEserlekuenInterfazea();
        }
    }
}