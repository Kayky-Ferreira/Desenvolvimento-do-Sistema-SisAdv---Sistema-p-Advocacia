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
    /// Lógica interna para BuscarAdvogado.xaml
    /// </summary>
    public partial class BuscarAdvogado : Window
    {
        public BuscarAdvogado()
        {
            InitializeComponent();
            Loaded += BuscarAdvogado_Loaded;
        }

        private void BuscarAdvogado_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void Btn_Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            if (TxbNomeAdvogado.Text == "" && TxbCpf.Text == "")
            {
                MessageBox.Show("Nenhum dos campos foi inserido. Insira dados em algum dos campos para realizar uma consulta!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDataGrid();
            }
            else
                ConsultaLoadDataGrid();
        }

        private void LoadDataGrid()
        {
            try
            {
                var dao = new AdvogadoDAO();

                dataGridBuscarAdvogado.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void ConsultaLoadDataGrid()
        {
            try
            {
                var dao = new AdvogadoDAO();
                string nomeAdvogado = null;
                string cpfAdvogado = null;

                if (TxbNomeAdvogado.Text != null)
                {
                    nomeAdvogado = TxbNomeAdvogado.Text;
                }

                if (TxbCpf.Text != null)
                {
                    cpfAdvogado = TxbCpf.Text;
                }

                dataGridBuscarAdvogado.ItemsSource = null;
                dataGridBuscarAdvogado.ItemsSource = dao.ListConsulta(nomeAdvogado, cpfAdvogado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            var advogadoSelected = dataGridBuscarAdvogado.SelectedItem as Advogado;

            var window = new CadastrarAdvogado(advogadoSelected.Id);

            window.ShowDialog();

            LoadDataGrid();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            var advogadoSelected = dataGridBuscarAdvogado.SelectedItem as Advogado;

            var result = MessageBox.Show($"Deseja realmente remover o advogado `{advogadoSelected.Nome}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new AdvogadoDAO();
                    dao.Delete(advogadoSelected);
                    LoadDataGrid();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAdicionarNovoProcesso_Click(object sender, RoutedEventArgs e)
        {
            CadastrarAdvogado cadastrarAdvogado = new CadastrarAdvogado();
            cadastrarAdvogado.ShowDialog();
        }
    }
}
