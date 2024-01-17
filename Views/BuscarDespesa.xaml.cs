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
    /// Lógica interna para BuscarDespesa.xaml
    /// </summary>
    public partial class BuscarDespesa : Window
    {
        public BuscarDespesa()
        {
            InitializeComponent();
            Loaded += BuscarDespesa_Loaded;
        }

        public void BuscarDespesa_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            try
            {
                var dao = new DespesaDAO();
                dataGridBuscarDespesa.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (textOrigem.Text == "" && dataDespesa.SelectedDate == null && textValor.Text == "")
                MessageBox.Show("Nenhum dos campos foi inserido. Insira dados em algum dos campos para realizar uma consulta!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                ConsultaLoadDataGrid();
        }

        private void ConsultaLoadDataGrid()
        {

            try
            {
                var dao = new DespesaDAO();
                DateTime? data;
                string Origem = null;
                string dataconvertida = null;
                double valor = 0.0;

                if (dataDespesa.SelectedDate != null)
                {
                    DateTime? selectedDate = (DateTime?)dataDespesa.SelectedDate;
                    data = selectedDate;
                    dataconvertida = data?.ToString("yyyy-MM-dd");
                }

                if (textOrigem.Text != null)
                    Origem = textOrigem.Text;

                if (double.TryParse(textValor.Text, out double salario))
                    valor = salario;

                dataGridBuscarDespesa.ItemsSource = null;
                dataGridBuscarDespesa.ItemsSource = dao.ListConsulta(Origem, dataconvertida, valor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var despesaSelecionada = dataGridBuscarDespesa.SelectedItem as Despesa;

            var result = MessageBox.Show($"Deseja realmente remover a despesa de origem `{despesaSelecionada.Origem}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new DespesaDAO();
                    dao.Delete(despesaSelecionada);
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
            var despesaSelecionada = dataGridBuscarDespesa.SelectedItem as Despesa;

            var window = new CadastrarDespesa(despesaSelecionada.Id);

            window.ShowDialog();

            LoadDataGrid();
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
