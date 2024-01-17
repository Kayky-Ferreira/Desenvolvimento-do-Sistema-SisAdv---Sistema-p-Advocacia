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
    /// Lógica interna para ImprimirRelatorio.xaml
    /// </summary>
    public partial class ImprimirRelatorio : Window
    {
        public ImprimirRelatorio()
        {
            InitializeComponent();
            Loaded += ImprimirRelatorio_Loaded;
        }

        private void ImprimirRelatorio_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBoxCaixa();
        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            //Abrir explorador de arquivos para salvar o relatório (por enquanto vou deixar somente uma mensagem)

        }

        private void LoadComboBoxCaixa()
        {
            var dao = new CaixaDAO();
            comboboxcaixa.ItemsSource = dao.List();
        }

        private void VisualizarRelatorio()
        {
            string data = null;
            Caixa valorcombobox = null;
            int boxnulo = 0;
            DateTime? datasemconversao = null;

            /*if (comboboxcaixa.SelectedItem != null)
            {
                valorcombobox = comboboxcaixa.Text;
                var relatorio = new RelatorioAlternativo(valorcombobox, data);
            }*/
            if (comboboxcaixa.SelectedItem != null)
            {
                valorcombobox = comboboxcaixa.SelectedItem as Caixa;
                var relatorio = new RelatorioAlternativo(valorcombobox.Id, data);
                relatorio.ShowDialog();
            }

            if (dateEscolha.SelectedDate != null)
            {
                DateTime? selectedDate = (DateTime?)dateEscolha.SelectedDate;
                datasemconversao = selectedDate;
                data = datasemconversao?.ToString("yyyy-MM-dd");
                var relatorio = new RelatorioAlternativo(boxnulo, data);
                relatorio.ShowDialog();
            }
        }

        private void btnvisualizar_Click(object sender, RoutedEventArgs e)
        {
            VisualizarRelatorio();
        }
    }
    
}
