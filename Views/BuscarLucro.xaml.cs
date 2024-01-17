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
    /// Lógica interna para BuscarLucro.xaml
    /// </summary>
    public partial class BuscarLucro : Window
    {
        public BuscarLucro()
        {
            InitializeComponent();
            Loaded += BuscarLucro_Loaded;
        }
        
        public void BuscarLucro_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void buttonExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deseja excluir este(s) cadastro(s)?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        private void LoadDataGrid()
        {
            try
            {
                var dao = new LucroDAO();

                dataGridBuscarLucro.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void buttonPesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (textOrigem.Text == "" && dataLucro.SelectedDate == null && textValor.Text == "")
                MessageBox.Show("Nenhum dos campos foi inserido. Insira dados em algum dos campos para realizar uma consulta!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                ConsultaLoadDataGrid();
        }

        private void ConsultaLoadDataGrid()
        {

            try
            {
                var dao = new LucroDAO();
                DateTime? data;
                string Origem = null;
                string dataconvertida = null;
                double valor = 0.0;

                if (dataLucro.SelectedDate != null)
                {
                    DateTime? selectedDate = (DateTime?)dataLucro.SelectedDate;
                    data = selectedDate;
                    dataconvertida = data?.ToString("yyyy-MM-dd");
                }

                if (textOrigem.Text != null)
                    Origem = textOrigem.Text;

                if (double.TryParse(textValor.Text, out double salario))
                    valor = salario;

                dataGridBuscarLucro.ItemsSource = null;
                dataGridBuscarLucro.ItemsSource = dao.ListConsulta(Origem, dataconvertida, valor, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            var lucroSelected = dataGridBuscarLucro.SelectedItem as Lucro;

            var window = new Cadastrarlucro(lucroSelected.Id);

            window.ShowDialog();

            LoadDataGrid();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var lucroSelected = dataGridBuscarLucro.SelectedItem as Lucro;

            var result = MessageBox.Show($"Deseja realmente remover o lucro`{lucroSelected.Origem}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new LucroDAO();
                    dao.Delete(lucroSelected);
                    LoadDataGrid();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
