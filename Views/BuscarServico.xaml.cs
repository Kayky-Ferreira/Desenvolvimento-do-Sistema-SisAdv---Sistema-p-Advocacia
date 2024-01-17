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
using SisAdv.Helpers;
using MySql.Data.MySqlClient;

namespace SisAdv.Views
{
    /// <summary>
    /// Lógica interna para BuscarServico.xaml
    /// </summary>
    public partial class BuscarServico : Window
    {
        public BuscarServico()
        {
            InitializeComponent();
            Loaded += BuscarServico_Loaded;
        }

        public void BuscarServico_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            var servicoSelected = dataGridBuscarServico.SelectedItem as Servico;

            var window = new CadastrarServico(servicoSelected.Id);            

            window.ShowDialog();

            LoadDataGrid();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var servicoSelected = dataGridBuscarServico.SelectedItem as Servico;

            var result = MessageBox.Show($"Deseja realmente remover o servico do cliente `{servicoSelected.Cliente}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new ServicoDAO();
                    dao.Delete(servicoSelected);
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
                var dao = new ServicoDAO();

                dataGridBuscarServico.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void Btn_Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (TxbCliente.Text == "" && datePickerDataServ.SelectedDate == null && rbtipoCivil.IsChecked == false && rbtipoCriminal.IsChecked == false && rbtipoEleitoral.IsChecked == false)
                MessageBox.Show("Nenhum dos campos foi inserido. Insira dados em algum dos campos para realizar uma consulta!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                ConsultaLoadDataGrid();
        }

        private void ConsultaLoadDataGrid()
        {           

            try
            {
                var dao = new ServicoDAO();
                DateTime? data;
                string cliente = null;
                string dataconvertida = null;
                string tipoServico = null;

                if (datePickerDataServ.SelectedDate != null)
                {
                    DateTime? selectedDate = (DateTime?)datePickerDataServ.SelectedDate;
                    data = selectedDate;
                    dataconvertida = data?.ToString("yyyy-MM-dd");
                }

                if (TxbCliente.Text != null)
                {
                    string text = TxbCliente.Text;
                    cliente = text;
                }

                if (rbtipoEleitoral.IsChecked.Value)
                    tipoServico = "Eleitoral";
                else if (rbtipoCriminal.IsChecked.Value)
                    tipoServico = "Criminal";
                else if (rbtipoCivil.IsChecked.Value)
                    tipoServico = "Civil";
                else
                    tipoServico = null;

                dataGridBuscarServico.ItemsSource = null;                
                dataGridBuscarServico.ItemsSource = dao.ListConsulta(cliente, dataconvertida, tipoServico);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputs()
        {
            TxbCliente.Clear();
            datePickerDataServ.SelectedDate = null;
            rbtipoCivil.IsChecked = false;
            rbtipoCriminal.IsChecked = false;
            rbtipoEleitoral.IsChecked = false;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
