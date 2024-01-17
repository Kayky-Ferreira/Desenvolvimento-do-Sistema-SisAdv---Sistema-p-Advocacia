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
    /// Lógica interna para BuscarProcesso.xaml
    /// </summary>
    public partial class BuscarProcesso : Window
    {
        public BuscarProcesso()
        {
            InitializeComponent();
            Loaded += BuscarProcesso_Loaded;
        }

        private void BuscarProcesso_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void Btn_Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (TxbNomeCliente.Text == "" && TxbValor.Text == "" && TxbStatus.Text == "")
            {
                MessageBox.Show("Nenhum dos campos foi inserido. Insira dados em algum dos campos para realizar uma consulta!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDataGrid();
            }
            else
                ConsultaLoadDataGrid();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdicionarNovoProcesso_Click(object sender, RoutedEventArgs e)
        {
            CadastrarProcesso cadastrarProcesso = new CadastrarProcesso();
            cadastrarProcesso.ShowDialog();
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            var processoSelected = dataGridBuscarProcesso.SelectedItem as Processo;

            var window = new CadastrarProcesso(processoSelected.Id);

            window.ShowDialog();

            LoadDataGrid();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var processoSelected = dataGridBuscarProcesso.SelectedItem as Processo;

            var result = MessageBox.Show($"Deseja realmente remover o processo do cliente `{processoSelected.Cliente.Nome}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new ProcessoDAO();
                    dao.Delete(processoSelected);
                    LoadDataGrid();
                }
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
                var dao = new ProcessoDAO();

                dataGridBuscarProcesso.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de Processos. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConsultaLoadDataGrid()
        {
            try
            {
                var dao = new ProcessoDAO();
                string cliente = null;
                string status = null;
                double valor = 0.0;

                if (TxbNomeCliente.Text != null)
                {
                    string text = TxbNomeCliente.Text;
                    cliente = text;
                }

                if (TxbStatus.Text != null)
                {
                    string text = TxbStatus.Text;
                    status = text;
                }

                if (double.TryParse(TxbValor.Text, out double salario))
                    valor = salario;

                dataGridBuscarProcesso.ItemsSource = null;
                dataGridBuscarProcesso.ItemsSource = dao.ListConsulta(cliente, status, valor);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
