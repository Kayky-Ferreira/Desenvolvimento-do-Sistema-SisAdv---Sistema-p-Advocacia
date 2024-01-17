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
    /// Lógica interna para CadastrarDespesa.xaml
    /// </summary>
    public partial class CadastrarDespesa : Window
    {
        private Despesa _despesa;
        private int _id;

        public CadastrarDespesa()
        {
            InitializeComponent();
            Loaded += CadastrarDespesa_Loaded;
        }

        public CadastrarDespesa(int id)
        {
            _id = id;
            InitializeComponent();
            Loaded += CadastrarDespesa_Loaded;
        }

        public void CadastrarDespesa_Loaded(object sender, RoutedEventArgs e)
        {
            _despesa = new Despesa();

            if(_id > 0)
            {
                FillForm();
            }
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            _despesa.Origem = TxBOrigem.Text;
            _despesa.Descricao = TxBDescricao.Text;

            if (datadopagamento.SelectedDate != null)
            {
                _despesa.Data = (DateTime)datadopagamento.SelectedDate;
            }

            if (double.TryParse(TxBValor.Text, out double valor))
            {
                _despesa.Valor = valor;
            }

            if (chavemensal.IsChecked.Value)
            {
                _despesa.Mensal = true;
            }
            else
            {
                _despesa.Mensal = false;
            }
                
            /*Testar esse código do Lucas mais a Hellen*/
            if (RadioCartao.IsChecked.Value)
            {
                _despesa.FormaPagamento = "Cartão";
            }
            else if (RadioDinheiro.IsChecked.Value)
            {
                _despesa.FormaPagamento = "Dinheiro";
            }
            else if (RadioTransf.IsChecked.Value)
            {
                _despesa.FormaPagamento = "Transferência";
            }

            SaveData();
        }

        private bool Validate()
        {
            var validator = new DespesaValidator();
            var result = validator.Validate(_despesa);

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
                    var dao = new DespesaDAO();
                    var text = "atualizada";

                    if (_despesa.Id == 0)
                    {
                        dao.Insert(_despesa);
                        text = "adicionada";
                    }
                    else
                        dao.Update(_despesa);

                    MessageBox.Show($"A despesa foi {text} com sucesso ao sistema!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseFormVerify();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FillForm()
        {
            try
            {
                var dao = new DespesaDAO();
                _despesa = dao.GetById(_id);

                TxbCodigo.Text = _despesa.Id.ToString();
                datadopagamento.SelectedDate = _despesa.Data;
                TxBValor.Text = _despesa.Valor.ToString();
                TxBDescricao.Text = _despesa.Descricao;
                TxBOrigem.Text = _despesa.Origem;

                if (_despesa.Mensal == true)
                    chavemensal.IsChecked = true;
                else
                    chavemensal.IsChecked = false;

                if (_despesa.FormaPagamento == "Dinheiro")
                    RadioDinheiro.IsChecked = true;
                else if (_despesa.FormaPagamento == "Cartão")
                    RadioCartao.IsChecked = true;
                else
                    RadioTransf.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseFormVerify()
        {
            if (_despesa.Id == 0)
            {
                var result = MessageBox.Show("Deseja continuar adicionando serviços?", "Continuar?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    this.Close();
                /*else
                    ClearInputs();*/
            }
            else
                this.Close();
        }
    }
}
