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

namespace SisAdv.Views
{
    /// <summary>
    /// Lógica interna para ConfigurarPreferenciasRecursos.xaml
    /// </summary>
    public partial class ConfigurarPreferenciasRecursos : Window
    {
        public ConfigurarPreferenciasRecursos()
        {
            InitializeComponent();
        }

        private void btnsalvar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Alterações salvas com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void bttrocarsenha_Click(object sender, RoutedEventArgs e)
        {
            AlterarSenha alterarSenha = new AlterarSenha();

            alterarSenha.ShowDialog();
        }
    }
}
