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
    /// Lógica interna para Cadastrarlucro.xaml
    /// </summary>
    public partial class Cadastrarlucro : Window
    {
        public Lucro _lucro;
        public int _id;

        public Cadastrarlucro()
        {
            InitializeComponent();
            Loaded += Cadastrarlucro_Loaded;
        }

        public Cadastrarlucro(int id)
        {
            _id = id;
            InitializeComponent();
            Loaded += Cadastrarlucro_Loaded;
        }

        public void Cadastrarlucro_Loaded(object sender, RoutedEventArgs e)
        {
            _lucro = new Lucro();

            LoadDataGrid();
            LoadComboBox();

            if (_id > 0)
                FillForm();
        }

        private void LoadComboBox()
        {
            try
            {
                dateLucro.SelectedDate = DateTime.Now;
                ComboboxServico.ItemsSource = new ServicoDAO().List();
                ComboBoxCaixa.ItemsSource = new CaixaDAO().List();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillForm()
        {
            try
            {
                var dao = new LucroDAO();
                _lucro = dao.GetById(_id);

                textId.Text = _lucro.Id.ToString();
                dateLucro.SelectedDate = _lucro.Data;
                textvalor.Text = _lucro.Valor.ToString();
                textDescricao.Text = _lucro.Descricao;
                textOrigem.Text = _lucro.Origem;

                if (_lucro.Servico != null)
                    ComboboxServico.SelectedValue = _lucro.Servico.Id;
                if (_lucro.Servico != null)
                    ComboBoxCaixa.SelectedValue = _lucro.Caixa.Id;
                if (_lucro.FormaRecebimento == "Dinheiro")
                    rbDinheiro.IsChecked = true;
                if (_lucro.FormaRecebimento == "Transferência")
                    rbTransferência.IsChecked = true;
                if (_lucro.FormaRecebimento == "Cartão")
                    rbCartao.IsChecked = true;
                if (_lucro.Mensal == true)
                    opcaoMensal.IsChecked = true;
                else
                    opcaoMensal.IsChecked = false;
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
                var dao = new LucroDAO();

                gridcadastrarlucro.ItemsSource = dao.List();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não foi possível carregar as listas de serviços. Verifique e tente novamente.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            _lucro.Origem = textOrigem.Text;
            _lucro.Descricao = textDescricao.Text;

            if (ComboboxServico.SelectedItem != null)
                _lucro.Servico = ComboboxServico.SelectedItem as Servico;

            if (ComboBoxCaixa.SelectedItem != null)
                _lucro.Caixa = ComboBoxCaixa.SelectedItem as Caixa;

            if (dateLucro.SelectedDate != null)
                _lucro.Data = (DateTime)dateLucro.SelectedDate;

            if (double.TryParse(textvalor.Text, out double valor))
                _lucro.Valor = valor;

            if (opcaoMensal.IsChecked.Value)
                _lucro.Mensal = true;
            else
                _lucro.Mensal = false;

            if (rbCartao.IsChecked.Value)
                _lucro.FormaRecebimento = "Cartão";
            else if (rbDinheiro.IsChecked.Value)
                _lucro.FormaRecebimento = "Dinheiro";
            else if (rbTransferência.IsChecked.Value)
                _lucro.FormaRecebimento = "Transferência";

            SaveData();
        }

        private bool Validate()
        {
            var validator = new LucroValidator();
            var result = validator.Validate(_lucro);

            if (!result.IsValid)
            {
                string errors = null;
                var count = 1;

                foreach (var failure in result.Errors)
                {
                    errors += $"{count++} - {failure.ErrorMessage}\n";
                }

                MessageBox.Show(errors, "Validação de Dados", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return result.IsValid;
        }

        private void SaveData()
        {
            try
            {
                if (Validate())
                {
                    var dao = new LucroDAO();
                    var text = "atualizada";

                    if (_lucro.Id == 0)
                    {
                        dao.Insert(_lucro);
                        text = "adicionada";
                    }
                    else
                        dao.Update(_lucro);

                    MessageBox.Show($"O Lucro foi {text} com sucesso ao sistema!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseFormVerify();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputs()
        {
            textOrigem.Clear();
            textId.Clear();
            textDescricao.Clear();
            textvalor.Clear();
            dateLucro.SelectedDate = null;
            ComboboxServico.SelectedItem = "";
            ComboBoxCaixa.SelectedItem = "";
            rbCartao.IsChecked = false;
            rbDinheiro.IsChecked = false;
            rbTransferência.IsChecked = false;
        }

        private void CloseFormVerify()
        {
            if (_lucro.Id == 0)
            {
                var result = MessageBox.Show("Deseja continuar adicionando lucros?", "Continuar?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    this.Close();
                else
                    ClearInputs();
            }
            else
                this.Close();
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            var window = new BuscarLucro();
            window.ShowDialog();
        }
    }
}
