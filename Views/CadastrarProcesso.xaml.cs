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
    /// Lógica interna para CadastrarProcesso.xaml
    /// </summary>
    public partial class CadastrarProcesso : Window
    {
        private int _id;

        private Processo _processo;

        public CadastrarProcesso()
        {
            InitializeComponent();
            Loaded += CadastrarProcesso_Loaded;
        }

        public CadastrarProcesso(int id)
        {
            _id = id;
            InitializeComponent();
            Loaded += CadastrarProcesso_Loaded;
        }

        private void CadastrarProcesso_Loaded(object sender, RoutedEventArgs e)
        {
            _processo = new Processo();
            LoadCombobox();

            if (_id > 0)
                FillForm();
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            BuscarProcesso buscarProcesso = new BuscarProcesso();
            buscarProcesso.ShowDialog();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            _processo.Status = TxbStatus.Text;
            _processo.Descricao = TxbDescricao.Text;
            _processo.Resultado = TxbResultado.Text;

            if (ComboboxServico.SelectedItem != null)
                _processo.Servico = ComboboxServico.SelectedItem as Servico;

            if (DataPickerDataInício.SelectedDate != null)
                _processo.DataProcesso = (DateTime)DataPickerDataInício.SelectedDate;

            if (double.TryParse(TxbValor.Text, out double salario))
                _processo.Valor = salario;

            SaveData();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadCombobox()
        {
            try
            {
                DataPickerDataInício.SelectedDate = DateTime.Now;
                ComboboxCliente.ItemsSource = new ClienteDAO().List();
                ComboboxAdvogado.ItemsSource = new AdvogadoDAO().List();
                ComboboxServico.ItemsSource = new ServicoDAO().List();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            var validator = new ProcessoValidator();
            var result = validator.Validate(_processo);

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
                    var dao = new ProcessoDAO();
                    var text = "atualizado";

                    if (_processo.Id == 0)
                    {
                        dao.Insert(_processo);
                        text = "adicionado";
                    }
                    else
                        dao.Update(_processo);

                    MessageBox.Show($"O Servico foi {text} com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseFormVerify();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseFormVerify()
        {
            if (_processo.Id == 0)
            {
                var result = MessageBox.Show("Deseja continuar adicionando serviços?", "Continuar?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    this.Close();
                else
                    ClearInputs();
            }
            else
                this.Close();
        }

        private void FillForm()
        {
            try
            {
                var dao = new ProcessoDAO();
                _processo = dao.GetById(_id);

                TxbIdProcesso.Text = _processo.Id.ToString();
                DataPickerDataInício.SelectedDate = _processo.DataProcesso;
                TxbResultado.Text = _processo.Resultado;
                TxbDescricao.Text = _processo.Descricao;
                TxbStatus.Text = _processo.Status;
                TxbValor.Text = _processo.Valor.ToString();

                if (_processo.Cliente != null)
                    ComboboxCliente.SelectedValue = _processo.Cliente.Id;

                if (_processo.Advogado != null)
                    ComboboxAdvogado.SelectedValue = _processo.Advogado.Id;

                if (_processo.Servico != null)
                    ComboboxServico.SelectedValue = _processo.Servico.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputs()
        {
            TxbDescricao.Clear();
            TxbIdProcesso.Clear();
            TxbStatus.Clear();
            TxbResultado.Clear();
            ComboboxCliente.SelectedItem = null;
            ComboboxAdvogado.SelectedItem = null;
            ComboboxServico.SelectedItem = null;
            DataPickerDataInício.SelectedDate = null;
        }
    }
}
