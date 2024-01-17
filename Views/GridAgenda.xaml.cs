using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SisAdv.Models;

namespace SisAdv.Views
{
    /// <summary>
    /// Lógica interna para GridAgenda.xaml
    /// </summary>
    public partial class GridAgenda : Window
    {
        public string _data;
        public GridAgenda()
        {
            InitializeComponent();
            Loaded += GridAgenda_Loaded;
        }
        public GridAgenda(string dataCalendario)
        {
            InitializeComponent();
            Loaded += GridAgenda_Loaded;
            _data = dataCalendario;
        }

        private void GridAgenda_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(_data);
        }

        private void LoadDataGrid(string data)
        {
            try
            {
                var dao = new EventoDAO();
                dataGridAgenda.ItemsSource = dao.ListConsulta(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
