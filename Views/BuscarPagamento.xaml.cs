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
    /// Lógica interna para BuscarPagamento.xaml
    /// </summary>
    public partial class BuscarPagamento : Window
    {
        public BuscarPagamento()
        {
            InitializeComponent();
            Loaded += BuscarPagamento_Loaded;
        }

        private void BuscarPagamento_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }        

        private void LoadDataGrid()
        {
            try
            {
                var dao = new PagamentoDAO();
                gridpagamento.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de Pagamentos. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => Close();

        private void Btn_Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (datapagamento.SelectedDate == null && TxbValor.Text == "" && Txborigem.Text == "")
            {
                MessageBox.Show("Nenhum dos campos foi inserido. Insira dados em algum dos campos para realizar uma consulta!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDataGrid();
            }
            else
                ConsultaLoadDataGrid();
        }

        private void ConsultaLoadDataGrid()
        {
            try
            {
                var dao = new PagamentoDAO();
                DateTime? data = null;
                string datafinal = null;
                string origem = null;
                double valor = 0.0;

                if (double.TryParse(TxbValor.Text, out double valorpagamento))
                {
                    valor = valorpagamento;
                }

                if (datapagamento.SelectedDate != null)
                {
                    DateTime? selectedDate = (DateTime?)datapagamento.SelectedDate;
                    data = selectedDate;
                    datafinal = data?.ToString("yyyy-MM-dd");
                }

                if(Txborigem.Text != null)
                {
                    origem = Txborigem.Text;
                }

                gridpagamento.ItemsSource = null;
                gridpagamento.ItemsSource = dao.ListBusca(datafinal, valor, origem, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            CadastrarPagamento cadastrarPagamento = new CadastrarPagamento();
            cadastrarPagamento.Show();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var pagamento = gridpagamento.SelectedItem as Pagamento;

            var result = MessageBox.Show($"Deseja realmente remover a despesa de `{pagamento.Despesa.Origem}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new PagamentoDAO();
                    dao.Delete(pagamento);
                    LoadDataGrid();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            var pagamento = gridpagamento.SelectedItem as Pagamento;

            var window = new CadastrarPagamento(pagamento.Id);

            window.ShowDialog();

            LoadDataGrid();
        }
    }
}
