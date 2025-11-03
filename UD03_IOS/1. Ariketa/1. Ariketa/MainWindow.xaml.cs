using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;

namespace LekuErreserbaSistema
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

            // GAURKO DATA JARRI
            erreserbaDatePicker.SelectedDate = DateTime.Today;
            erreserbaDatePicker.SelectedDateChanged += ErreserbaDatePicker_SelectedDateChanged;
        }

        // DATA ALDATZEKO METODO BERRIA
        private void ErreserbaDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_uneanErakutsitakoGarraioa != null)
            {
                EguneratuEserlekuenInterfazea();
            }
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
                default: _uneanErakutsitakoGarraioa = null; break;
            }
            EguneratuEserlekuenInterfazea();
        }

        private void EguneratuEserlekuenInterfazea()
        {
            if (eserlekuenGrid == null) return;
            eserlekuenGrid.Children.Clear();
            eserlekuenGrid.RowDefinitions.Clear();
            eserlekuenGrid.ColumnDefinitions.Clear();

            if (_uneanErakutsitakoGarraioa == null) return;

            var data = erreserbaDatePicker.SelectedDate ?? DateTime.Today;
            var egunekoErreserbak = _gordetakoErreserbak
                .Where(r => r.GarraioMota == _uneanErakutsitakoGarraioa.Mota && r.ErreserbaData.Date == data.Date)
                .SelectMany(r => r.EserlekuakId)
                .ToList();

            foreach (var eserlekua in _uneanErakutsitakoGarraioa.Eserlekuak)
            {
                eserlekua.Egoera = EserlekuEgoera.Librea;
                if (egunekoErreserbak.Contains(eserlekua.Id))
                {
                    eserlekua.Egoera = EserlekuEgoera.Okupatua;
                }
            }

            int eserlekuakErrenkadako;
            int pasilloZutabea;

            switch (_uneanErakutsitakoGarraioa.Mota)
            {
                case "Hegazkina":
                    eserlekuakErrenkadako = 6;
                    pasilloZutabea = 3;
                    break;
                case "Autobusa":
                case "Trena":
                default:
                    eserlekuakErrenkadako = 4;
                    pasilloZutabea = 2;
                    break;
            }

            int zutabeKopurua = eserlekuakErrenkadako + 1;
            int errenkadaKopurua = (int)Math.Ceiling((double)_uneanErakutsitakoGarraioa.Eserlekuak.Count / eserlekuakErrenkadako);

            for (int i = 0; i < zutabeKopurua; i++)
            {
                var zutabeDef = new ColumnDefinition();
                if (i == pasilloZutabea)
                {
                    zutabeDef.Width = new GridLength(0.5, GridUnitType.Star);
                }
                else
                {
                    zutabeDef.Width = new GridLength(1, GridUnitType.Star);
                }
                eserlekuenGrid.ColumnDefinitions.Add(zutabeDef);
            }

            for (int i = 0; i < errenkadaKopurua; i++)
            {
                eserlekuenGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            int unekoErrenkada = 0;
            int unekoZutabea = 0;
            foreach (var eserlekua in _uneanErakutsitakoGarraioa.Eserlekuak)
            {
                if (unekoZutabea == pasilloZutabea)
                {
                    unekoZutabea++;
                }

                Button eserlekuBotoia = new Button
                {
                    Content = eserlekua.Id,
                    Tag = eserlekua,
                    Width = 40,
                    Height = 40,
                    Margin = new Thickness(5)
                };

                EguneratuBotoiarenItxura(eserlekuBotoia);
                eserlekuBotoia.Click += Eserlekua_Click;
                eserlekuBotoia.MouseEnter += Eserlekua_MouseEnter;
                eserlekuBotoia.MouseLeave += Eserlekua_MouseLeave;

                Grid.SetRow(eserlekuBotoia, unekoErrenkada);
                Grid.SetColumn(eserlekuBotoia, unekoZutabea);
                eserlekuenGrid.Children.Add(eserlekuBotoia);

                unekoZutabea++;
                if (unekoZutabea >= zutabeKopurua)
                {
                    unekoZutabea = 0;
                    unekoErrenkada++;
                }
            }
        }

        private void Eserlekua_Click(object sender, RoutedEventArgs e)
        {
            Button botoia = (Button)sender;
            Eserlekua eserlekua = (Eserlekua)botoia.Tag;

            if (eserlekua.Egoera == EserlekuEgoera.Librea) eserlekua.Egoera = EserlekuEgoera.Hautatua;
            else if (eserlekua.Egoera == EserlekuEgoera.Hautatua) eserlekua.Egoera = EserlekuEgoera.Librea;
            else if (eserlekua.Egoera == EserlekuEgoera.Okupatua) MessageBox.Show("Eserleku hau jada okupatuta dago.");

            EguneratuBotoiarenItxura(botoia);
        }

        private void Eserlekua_MouseEnter(object sender, MouseEventArgs e)
        {
            Button botoia = (Button)sender;
            botoia.BorderBrush = Brushes.Blue;
            botoia.BorderThickness = new Thickness(2);
        }

        private void Eserlekua_MouseLeave(object sender, MouseEventArgs e)
        {
            Button botoia = (Button)sender;
            botoia.BorderBrush = Brushes.Gray;
            botoia.BorderThickness = new Thickness(1);
        }

        private void EguneratuBotoiarenItxura(Button botoia)
        {
            Eserlekua eserlekua = (Eserlekua)botoia.Tag;
            switch (eserlekua.Egoera)
            {
                case EserlekuEgoera.Librea:
                    botoia.Background = Brushes.LightGreen;
                    botoia.IsEnabled = true;
                    break;
                case EserlekuEgoera.Okupatua:
                    botoia.Background = Brushes.Red;
                    botoia.IsEnabled = false;
                    break;
                case EserlekuEgoera.Hautatua:
                    botoia.Background = Brushes.Yellow;
                    botoia.IsEnabled = true;
                    break;
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

            var hautatutakoEserlekuak = _uneanErakutsitakoGarraioa.Eserlekuak
                .Where(es => es.Egoera == EserlekuEgoera.Hautatua)
                .ToList();

            if (hautatutakoEserlekuak.Count == 0)
            {
                MessageBox.Show("Ez duzu eserlekurik hautatu.");
                return;
            }

            var erreserbaBerria = new Erreserba
            {
                ErabiltzaileIzena = "Proba Erabiltzailea",
                GarraioMota = _uneanErakutsitakoGarraioa.Mota,
                ErreserbaData = erreserbaDatePicker.SelectedDate.Value,
                EserlekuakId = hautatutakoEserlekuak.Select(es => es.Id).ToList()
            };

            _gordetakoErreserbak.Add(erreserbaBerria);
            await _datuenKudeatzailea.GordeErreserbakAsync(_gordetakoErreserbak);

            MessageBox.Show("Erreserba behar bezala gorde da.");
            EguneratuEserlekuenInterfazea();
        }
    }
}