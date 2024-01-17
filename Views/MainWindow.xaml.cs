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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SisAdv.Models;

namespace SisAdv.Views
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            ShowColumnChart();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();

            LoadEventosCalendar();

            dataGridServicosRecentes.CanUserReorderColumns = true;
            dataGridServicosRecentes.CanUserSortColumns = true;
        }

        private void Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Window window;

            switch (button.Name)
            {
                case "btadicionar":
                    funcoesadicionar.Visibility = Visibility.Visible;
                    funcoesbuscar.Visibility = Visibility.Collapsed;
                    break;
                case "btaddrevento":
                    window = new Cadastraeventonov();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaddcliente":
                    window = new Cadastrarcliente();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaaddservico":
                    window = new CadastrarServico();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btadddespesa":
                    window = new CadastrarDespesa();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaddlucro":
                    window = new Cadastrarlucro();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaddAdvogado":
                    window = new CadastrarAdvogado();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaddUsuario":
                    window = new CadastrarNovoUsuario();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaddProcesso":
                    window = new CadastrarProcesso();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaddCaixa":
                    window = new Cadastrarcaixa();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btaddPagamento":
                    window = new CadastrarPagamento();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscar":
                    funcoesbuscar.Visibility = Visibility.Visible;
                    funcoesadicionar.Visibility = Visibility.Collapsed;
                    break;
                case "btbuscardespesa":
                    window = new BuscarDespesa();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscarlucro":
                    window = new BuscarLucro();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscarcliente":
                    window = new BuscarCliente();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscaservico":
                    window = new BuscarServico();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscaAdvogado":
                    window = new BuscarAdvogado();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscaProcesso":
                    window = new BuscarProcesso();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscaPagamento":
                    window = new BuscarPagamento();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btbuscaCaixa":
                    window = new BuscarCaixa();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btconfiguracoes":
                    window = new ConfigurarPreferenciasRecursos();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btImprimirRelatorio":
                    window = new ImprimirRelatorio();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btacessardiariojustica":
                    window = new AcessarDiarioJustiça();
                    ColapsarButtons();
                    window.ShowDialog();
                    break;
                case "btOcultarServicos":
                    dataGridServicosRecentes.Visibility = Visibility.Collapsed;
                    textRecente.Visibility = Visibility.Collapsed;
                    gridDireitaRecentes.Width = 80;
                    gridCentral.Width = 800;
                    break;
                case "btsair":
                    MessageBoxResult result = MessageBox.Show("Deseja Realmente Sair?", "Save error", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                        this.Close();
                    break;
                case "BtnVisualizarDia":
                    Agenda();
                    break;
            }
        }        

        private void LoadDataGrid()
        {
            try
            {
                var dao = new ServicoDAO();
                dataGridServicosRecentes.ItemsSource = dao.List();
                /*dataGridServicosRecentes.SortMode = sor
                dataGridServicosRecentes.Sorting(dataGridServicosRecentes.Columns[Id], System.ComponentModel.ListSortDirection.Descending);
                //dataGridServicosRecentes.Columns[].SortMode = dataGridServicosRecentes.UpdateLayout;*/
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void LoadEventosCalendar()
        {
            try
            {
                var dao = new EventoDAO();
                //dao.ListDataCalendar(agendacompromissos);                
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        private void Agenda()//Está funcionando
        {
            try
            {
                string dataCalendario = null;
                DateTime? data = null;

                DateTime? selectedDate = (DateTime?)agendacompromissos.SelectedDate.Value;
                data = selectedDate;
                dataCalendario = data?.ToString("yyyy-MM-dd");

                GridAgenda gridAgenda = new GridAgenda(dataCalendario);
                gridAgenda.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowColumnChart()
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            valueList.Add(new KeyValuePair<string, int>("Segunda", 8));
            valueList.Add(new KeyValuePair<string, int>("Terça", 7));
            valueList.Add(new KeyValuePair<string, int>("Quarta", 7));
            valueList.Add(new KeyValuePair<string, int>("Quinta", 10));
            valueList.Add(new KeyValuePair<string, int>("Sexta", 6));
            valueList.Add(new KeyValuePair<string, int>("Sábado", 4));

            //Setting data for column chart
            columnChart.DataContext = valueList;
        }   

        private void Btn_visualizarServico_Click(object sender, RoutedEventArgs e)
        {
            var servicoSelected = dataGridServicosRecentes.SelectedItem as Servico;

            var window = new CadastrarServico(servicoSelected.Id);

            window.ShowDialog();

            LoadDataGrid();
        }

        private void BtnFixar_Click(object sender, RoutedEventArgs e)
        {
            //Verificar se tem uma forma de subir as linhas
        }

        private void ColapsarButtons()
        {
            funcoesbuscar.Visibility = Visibility.Collapsed;
            funcoesadicionar.Visibility = Visibility.Collapsed;
        }

        private void BtnDeletar_Click(object sender, RoutedEventArgs e)
        {
            var servicoSelected = dataGridServicosRecentes.SelectedItem as Servico;

            var result = MessageBox.Show($"Deseja realmente remover o servico do cliente `{servicoSelected.Cliente.Nome}`?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
    }
}
    
