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
    /// Lógica interna para AlterarSenha.xaml
    /// </summary>
    public partial class AlterarSenha : Window
    {
        private Usuario _alterarSenha;
        public AlterarSenha()
        {
            InitializeComponent();
            Loaded += AlterarSenha_Loaded;
        }

        private void AlterarSenha_Loaded(object sender, RoutedEventArgs e)
        {
            _alterarSenha = new Usuario();
            LoadComboBox();
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxAdvogado.SelectedItem != null)
                _alterarSenha.Advogado = ComboBoxAdvogado.SelectedItem as Advogado;

            if (TxbNovaSenha.Password == TxbConfirmarSenha.Password)
                _alterarSenha.Senha = TxbNovaSenha.Password;

            SaveData();
        }

        private void LoadComboBox()
        {
            try
            {
                var dao = new AdvogadoDAO();
                ComboBoxAdvogado.ItemsSource = dao.List();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SaveData()
        {
            try
            {
                if (Validate())
                {
                    var dao = new UsuarioDAO();
                    dao.Update(_alterarSenha);

                    MessageBox.Show($"A senha do Advogado '{_alterarSenha.Advogado.Nome}' foi alterada com sucesso!", "Sucesso",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            int i = 1;
            var validator = new UsuarioValidator(i);
            var result = validator.Validate(_alterarSenha);

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
    }
}
