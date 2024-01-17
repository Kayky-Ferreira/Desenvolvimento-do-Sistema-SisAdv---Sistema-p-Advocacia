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
    /// Lógica interna para CadastrarPagamento.xaml
    /// </summary>
    public partial class CadastrarPagamento : Window
    {
        private int _id;
        private Pagamento _pagamento; 
        public CadastrarPagamento()
        {
            InitializeComponent();
            Loaded += CadastrarPagamento_Loaded;
        }
        //esse é para pesquisar e preencher
        public CadastrarPagamento(int id)
        {
            _id = id;
            InitializeComponent();
            Loaded += CadastrarPagamento_Loaded;
        }

        private void CadastrarPagamento_Loaded(object sender, RoutedEventArgs e)
        {
            _pagamento = new Pagamento();
            LoadCombobox();
            if (_id > 0)
            {
                FillForm();
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (boxcaixa.SelectedItem != null)
                _pagamento.Caixa = boxcaixa.SelectedItem as Caixa;

            if (boxdespesa.SelectedItem != null)
                _pagamento.Despesa = boxdespesa.SelectedItem as Despesa;

            if (datapagamento.SelectedDate != null)
                _pagamento.DataPagamento = (DateTime)datapagamento.SelectedDate;

            if (boxtipopagamento.Text == "Á vista")
                _pagamento.TipoPagamento = "Á vista";
            else if (boxtipopagamento.Text == "No cartão")
                _pagamento.TipoPagamento = "No cartão";
            else
                _pagamento.TipoPagamento = "Via Transferência";

            //ele não entra
            if (double.TryParse(textvalor.Text, out double valor))
                _pagamento.Valor = valor;

            SaveData();
        }

        private void SaveData()
        {
            try
            {
                if (Validate())
                {
                    var dao = new PagamentoDAO();
                    var text = "atualizado";

                    if (_pagamento.Id == 0)
                    {
                        dao.Insert(_pagamento);
                        text = "adicionado";
                    }
                    else
                        dao.Update(_pagamento);

                    MessageBox.Show($"O Pagamento foi {text} com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseFormVerify();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            var validator = new PagamentoValidator();
            var result = validator.Validate(_pagamento);

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

        private void FillForm()
        {
            try
            {
                var dao = new PagamentoDAO();
                _pagamento = dao.GetById(_id);

                textid.Text = _pagamento.Id.ToString();
                datapagamento.SelectedDate = _pagamento.DataPagamento;
                textvalor.Text = _pagamento.Valor.ToString();

                if (_pagamento.Despesa != null)
                {
                    boxdespesa.SelectedValue = _pagamento.Despesa.Id;
                    boxdespesa.IsEnabled = false;
                }

                if (_pagamento.Caixa != null)
                {
                    boxcaixa.SelectedValue = _pagamento.Caixa.Id;
                    boxcaixa.IsEnabled = false;
                }                    

                if (_pagamento.TipoPagamento == "Á vista")
                    boxtipopagamento.SelectedItem = vista;
                else if (_pagamento.TipoPagamento == "No cartão")
                    boxtipopagamento.SelectedItem = cartao;
                else
                    boxtipopagamento.SelectedItem = transferencia;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btnpesquisarcaixa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btnadicionar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadCombobox()
        {
            //ComboboxAdvogado.ItemsSource = new AdvogadoDAO().List();
            boxcaixa.ItemsSource = new CaixaDAO().List();
            boxdespesa.ItemsSource = new DespesaDAO().List();
        }

        private void CloseFormVerify()
        {
            if (_pagamento.Id == 0)
            {
                var result = MessageBox.Show("Deseja continuar adicionando 'Pagamentos'?", "Continuar?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    this.Close();
                else
                    ClearInputs();
            }
            else
                this.Close();
        }

        private void ClearInputs()
        {
            textvalor.Clear();
            textid.Clear();
            boxtipopagamento.SelectedItem = null;
            datapagamento.SelectedDate = null;
            boxcaixa.SelectedItem = null;
            boxdespesa.SelectedItem = null;
        }

        private void BtnDeletarPagamento_Click(object sender, RoutedEventArgs e)
        {
            var iddelete = _id;

            var result = MessageBox.Show($"Deseja realmente remover esse pagamento?", "Confirmação de Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            /*try
            {
                if (result == MessageBoxResult.Yes)
                {
                    var dao = new PagamentoDAO();
                    dao.Delete(iddelete);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }
    }
}
