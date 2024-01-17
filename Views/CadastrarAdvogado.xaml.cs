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
    /// Lógica interna para CadastrarAdvogado.xaml
    /// </summary>
    public partial class CadastrarAdvogado : Window
    {
        private Advogado _advogado;
        private int _id;

        public CadastrarAdvogado()
        {
            InitializeComponent();
            Loaded += CadastrarAdvogado_Loaded;
        }

        public CadastrarAdvogado(int id)
        {
            _id = id;
            InitializeComponent();
            Loaded += CadastrarAdvogado_Loaded;
        }

        private void CadastrarAdvogado_Loaded(object sender, RoutedEventArgs e)
        {
            labeltexto.Content = "Observação: Todos os campos são necessários\n para um melhor gerenciamento dos usuários.";
            _advogado = new Advogado();

            if (_id > 0)
                FillForm();
        }


        /* colocar o botão editar pra evitar possíveis updates indesejados
        private void LiberacaoEditionAdd()
        {
            TxbCpf.IsEnabled = false;
            TxbEmail.IsEnabled = false;
            TxbNome.IsEnabled = false;
            TxbRg.IsEnabled = false;
            TxbTelefone.IsEnabled = false; 
            datePickerNascimento.IsEnabled = false;
            TxbId.IsEnabled = true;
        }

        private void EditionBlock()
        {
            TxbCpf.IsEnabled = true;
            TxbEmail.IsEnabled = true;
            TxbNome.IsEnabled = true;
            TxbRg.IsEnabled = true;
            TxbTelefone.IsEnabled = true;
            datePickerNascimento.IsEnabled = true;
            TxbId.IsEnabled = true;
        }*/

        private void FillForm()
        {
            try
            {
                var dao = new AdvogadoDAO();
                _advogado = dao.GetById(_id);

                TxbCpf.Text = _advogado.Cpf;
                TxbEmail.Text = _advogado.Email;
                TxbNome.Text = _advogado.Nome;
                TxbRg.Text = _advogado.Rg;
                TxbTelefone.Text = _advogado.Telefone;
                datePickerNascimento.SelectedDate = _advogado.DataNasc;
                TxbId.Text = _advogado.Id.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }        

        private void BtnSalvarAdvogado_Click(object sender, RoutedEventArgs e)
        {
            _advogado.Nome = TxbNome.Text;
            _advogado.Cpf = TxbCpf.Text;
            _advogado.Telefone = TxbTelefone.Text;            
            _advogado.Email = TxbEmail.Text;

            if (TxbRg.Text != null)
                _advogado.Rg = TxbRg.Text;
            else
                _advogado.Rg = null;

            if (datePickerNascimento.SelectedDate != null)
                _advogado.DataNasc = (DateTime)datePickerNascimento.SelectedDate;

            _advogado.Descricao = "Teste de inserção ADVOGADO, INSERIR TXB DPS";

            SaveData();
        }

        private void CarregarCadastrarUsuario()
        {
            CadastrarNovoUsuario cadastrarNovoUsuario = new CadastrarNovoUsuario();
            cadastrarNovoUsuario.ShowDialog();
        }

        private void SaveData()
        {
            try
            {
                if (Validate())
                {
                    var dao = new AdvogadoDAO();
                    var text = "atualizado";

                    if (_advogado.Id == 0)
                    {
                        dao.Insert(_advogado);
                        text = "adicionado";
                    }
                    else
                        dao.Update(_advogado);

                    MessageBox.Show($"Advogado {text} com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

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
            if (_advogado.Id == 0)
            {
                var result1 = MessageBox.Show("Deseja adicionar um usuário para este Advogado?","Adicionar?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result1 == MessageBoxResult.Yes)
                {
                    CarregarCadastrarUsuario();
                }
                else
                {
                    var result2 = MessageBox.Show("Deseja continuar adicionando advogados?", "Continuar?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result2 == MessageBoxResult.No)
                        this.Close();
                    else
                        ClearInputs();
                }                
            }
            else
                this.Close();
        }

        private bool Validate()
        {
            var validator = new AdvogadoValidator();
            var result = validator.Validate(_advogado);

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

        private void ClearInputs()
        {
            TxbCpf.Text = null;
            TxbEmail.Text = null;
            TxbNome.Text = null;
            TxbRg.Text = null;
            TxbTelefone.Text = null;
            datePickerNascimento.SelectedDate = null;
        }

        private void BtnAdicionarUsuario_Click(object sender, RoutedEventArgs e)
        {
            CadastrarNovoUsuario cadastrarNovoUsuario = new CadastrarNovoUsuario();
            cadastrarNovoUsuario.ShowDialog();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
