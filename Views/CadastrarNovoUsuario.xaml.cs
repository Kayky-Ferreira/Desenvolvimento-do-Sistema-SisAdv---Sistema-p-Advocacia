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
    /// Lógica interna para CadastrarNovoUsuario.xaml
    /// </summary>
    public partial class CadastrarNovoUsuario : Window
    {
        private Usuario _usuario; 

        public CadastrarNovoUsuario()
        {
            InitializeComponent();
            Loaded += CadastrarNovoUsuario_Loaded;            
        }

        private void CadastrarNovoUsuario_Loaded(object sender, RoutedEventArgs e)
        {
            _usuario = new Usuario();
            LoadCombobox();
        }

        private void BtnSalvarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if(TxbLogin.Text != null)
                _usuario.NomeUser = TxbLogin.Text;

            if (PassConfirmarSenha.Password == PassSenha.Password)
                _usuario.Senha = PassSenha.Password;

            if(ComboboxAdvogado.SelectedItem != null)
                _usuario.Advogado = ComboboxAdvogado.SelectedItem as Advogado;

            SaveData();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        

        private bool Validate()
        {
            var validator = new UsuarioValidator();
            var result = validator.Validate(_usuario);

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
                    var dao = new UsuarioDAO();
                    var text = "atualizado";

                    if (_usuario.Id == 0)
                    {
                        dao.Insert(_usuario);
                        text = "adicionado";
                    }
                    else
                        dao.Update(_usuario);

                    MessageBox.Show($"O Usuário foi {text} com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCombobox()
        {
            ComboboxAdvogado.ItemsSource = new AdvogadoDAO().List();
        }

        private void ClearInputs()
        {
            TxbLogin.Text = null;
            PassSenha.Password = null;
            PassConfirmarSenha.Password = null;
            ComboboxAdvogado.SelectedItem = null;
        }
    }
}
