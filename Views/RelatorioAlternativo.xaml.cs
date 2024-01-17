using SisAdv.Models;
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

namespace SisAdv.Views
{
    /// <summary>
    /// Lógica interna para RelatorioAlternativo.xaml
    /// </summary>
    public partial class RelatorioAlternativo : Window
    {
        private int _caixa;
        private string _data;

        public RelatorioAlternativo(int idcaixa, string dataescolhida)
        {
            InitializeComponent();
            Loaded += RelatorioAlternativo_Loaded;
            _caixa = idcaixa;
            _data = dataescolhida;
        }

        private void RelatorioAlternativo_Loaded(object sender, RoutedEventArgs e)
        {
            ConsultarBancodeDados(_caixa, _data);
        }

        private void ConsultarBancodeDados(int idcaixa, string dataescolhida)
        {
            if (dataescolhida != null)
            {
                try
                {
                    var dao = new PagamentoDAO();
                    griddespesas.ItemsSource = dao.ListBusca(dataescolhida, 0, null, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                try
                {
                    var dao1 = new LucroDAO();
                    gridlucros.ItemsSource = dao1.ListConsulta(null, dataescolhida, 0, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (idcaixa != 0)
            {
                try
                {
                    var dao = new PagamentoDAO();
                    griddespesas.ItemsSource = dao.ListBusca(null, 0, null, idcaixa);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                try
                {
                    var dao1 = new LucroDAO();
                    gridlucros.ItemsSource = dao1.ListConsulta(null, null, 0, idcaixa);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
