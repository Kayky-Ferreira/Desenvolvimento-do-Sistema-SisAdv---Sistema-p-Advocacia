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
    /// Lógica interna para CadastrarServico.xaml
    /// </summary>

    public partial class CadastrarServico : Window
    {
        private int _id;

        private Servico _servico;        

        public CadastrarServico()
        {
            InitializeComponent();
            Loaded += CadastrarServico_Loaded;
        }

        public CadastrarServico(int id)
        {
            _id = id;
            InitializeComponent();
            Loaded += CadastrarServico_Loaded;
        }

        private void CadastrarServico_Loaded(object sender, RoutedEventArgs e)
        {
            _servico = new Servico();

            LoadCombobox();

            if (_id > 0)
                FillForm();
        }

        private void btnPesquisarServico_Click(object sender, RoutedEventArgs e)
        {
            BuscarServico buscarServico = new BuscarServico();

            buscarServico.ShowDialog();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (comboboxCliente.SelectedItem != null)
                _servico.Cliente = comboboxCliente.SelectedItem as Cliente;
            else
                MessageBox.Show("Insira o Cliente. Verifique e tente novamente.");

            if (comboboxAdvogado.SelectedItem != null)
                _servico.Advogado = comboboxAdvogado.SelectedItem as Advogado;
            else
                MessageBox.Show("Insira o Advogado. Verifique e tente novamente.");

            _servico.Descricao = txbDescricao.Text;            

            if (double.TryParse(txbValor.Text, out double valor))
                _servico.Valor = valor;

            if (datepickerDataServico.SelectedDate != null)
                _servico.Data = (DateTime)datepickerDataServico.SelectedDate;
            else
                MessageBox.Show("Insira a Data. Verifique e tente novamente.");

            if (rbtipoEleitoral.IsChecked.Value)
                _servico.Tipo = "Eleitoral";
            else if (rbtipoCriminal.IsChecked.Value)
                _servico.Tipo = "Criminal";
            else if (rbtipoCivil.IsChecked.Value)
                _servico.Tipo = "Civil";

            SaveData();
        }

        private void btnAtribuirEvento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Evento Atribuido com Sucesso", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnAdicionarServico_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();            
        }

        private void LoadCombobox()
        {
            try
            {
                datepickerDataServico.SelectedDate = DateTime.Now;
                comboboxCliente.ItemsSource = new ClienteDAO().List();
                comboboxAdvogado.ItemsSource = new AdvogadoDAO().List();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            var validator = new ServicoValidator();
            var result = validator.Validate(_servico);

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
                    var dao = new ServicoDAO();
                    var text = "atualizado";

                    if (_servico.Id == 0)
                    {
                        dao.Insert(_servico);
                        text = "adicionado";
                    }
                    else
                        dao.Update(_servico);

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
            if (_servico.Id == 0)
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
                var dao = new ServicoDAO();
                _servico = dao.GetById(_id);

                txbId.Text = _servico.Id.ToString();
                datepickerDataServico.SelectedDate = _servico.Data;
                txbValor.Text = _servico.Valor.ToString();
                txbDescricao.Text = _servico.Descricao;

                if (_servico.Cliente != null)
                    comboboxCliente.SelectedValue = _servico.Cliente.Id;

                if (_servico.Advogado != null)
                comboboxAdvogado.SelectedValue = _servico.Advogado.Id;

                if (_servico.Tipo == "Eleitoral")
                    rbtipoEleitoral.IsChecked = true;
                if (_servico.Tipo == "Civil")
                    rbtipoCivil.IsChecked = true;
                if (_servico.Tipo == "Criminal")
                    rbtipoCriminal.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputs()
        {
            txbValor.Clear();
            txbDescricao.Clear();
            datepickerDataServico.SelectedDate = DateTime.Now;
            comboboxCliente.SelectedItem = null;
            comboboxAdvogado.SelectedItem = null;
            rbtipoCivil.IsChecked = false;
            rbtipoCriminal.IsChecked = false;
            rbtipoEleitoral.IsChecked = false;
        }
    }
}
