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
using SisAdv.Models;
using System.Windows.Shapes;

namespace SisAdv.Views
{
    /// <summary>
    /// Lógica interna para BuscarCaixa.xaml
    /// </summary>
    public partial class BuscarCaixa : Window
    {
        public BuscarCaixa()
        {
            InitializeComponent();
            Loaded += BuscarCaixa_Loaded;
        }

        private void BuscarCaixa_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            var caixa = dataGridBuscarCaixa.SelectedItem as Caixa;

            var window = new Cadastrarcaixa(caixa.Id);

            window.ShowDialog();

            LoadDataGrid();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var caixa = dataGridBuscarCaixa.SelectedItem as Caixa;

            var result = MessageBox.Show($"Deseja realmente remover caixa referente ao mês: `{caixa.Mes}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new CaixaDAO();
                    dao.Delete(caixa);
                    LoadDataGrid();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (TxbMes.Text == "" && TxbValorInicial.Text == "")
                MessageBox.Show("Nenhum dos campos foi inserido. Insira dados em algum dos campos para realizar uma consulta!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                ConsultaLoadDataGrid();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdicionarNovocaixa_Click(object sender, RoutedEventArgs e)
        {
            Cadastrarcaixa cadastrarcaixa = new Cadastrarcaixa();
            cadastrarcaixa.ShowDialog();
        }

        private void ConsultaLoadDataGrid()
        {

            try
            {
                var dao = new CaixaDAO();
                string mes = null;
                double valorinicial = 0.0;

                if (TxbMes.Text != null)
                {
                    string text = TxbMes.Text;
                    mes = text;
                }
                if (double.TryParse(TxbValorInicial.Text, out double valor))
                {
                    valorinicial = valor;
                }

                dataGridBuscarCaixa.ItemsSource = null;
                dataGridBuscarCaixa.ItemsSource = dao.ListConsulta(mes, valorinicial);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDataGrid()
        {
            try
            {
                var dao = new CaixaDAO();

                dataGridBuscarCaixa.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
