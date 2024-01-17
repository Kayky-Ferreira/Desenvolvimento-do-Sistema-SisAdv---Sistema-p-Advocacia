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

namespace SisAdv
{
    /// <summary>
    /// Lógica interna para CadastrarEvento.xaml
    /// </summary>
    public partial class CadastrarEvento : Window
    {
        public CadastrarEvento()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonAdicionar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Evento Adicionado", "", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Deseja excluir este Evento?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Evento Salvo", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        
    }
}
