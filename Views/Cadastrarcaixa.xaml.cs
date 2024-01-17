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
    /// Lógica interna para Cadastrarcaixa.xaml
    /// </summary>
    public partial class Cadastrarcaixa : Window
    {
        private Caixa _caixa;

        private int _id;

        public Cadastrarcaixa()
        {
            InitializeComponent();
            Loaded += Cadastrarcaixa_Loaded;
        }

        public Cadastrarcaixa(int id)
        {
            _id = id;
            InitializeComponent();
            Loaded += Cadastrarcaixa_Loaded;
        }

        private void Cadastrarcaixa_Loaded(object sender, RoutedEventArgs e)
        {
            _caixa = new Caixa();

            if(_id > 0)
            {
                FillForm();
            }
        }

        private void Btnadicionar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            _caixa.Mes = Txbmes.Text;

            if (double.TryParse(Txbsaldoinicial.Text, out double saldoinicial))
                _caixa.SaldoInicial = saldoinicial;

            if (double.TryParse(Txbsaldofinal.Text, out double saldofinal))
                _caixa.SaldoFinal = saldofinal;

            if (double.TryParse(Txbtotaldespesa.Text, out double despesatotal))
                _caixa.TotalDespesa = despesatotal;

            if (double.TryParse(Txbtotallucro.Text, out double lucrototal))
                _caixa.TotalLucro = lucrototal;

            SaveData();
        }


        private void Btnpesquisarcaixa_Click(object sender, RoutedEventArgs e)
        {
            BuscarCaixa buscarCaixa = new BuscarCaixa();
            buscarCaixa.ShowDialog();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FillForm()
        {
            try
            {
                var dao = new CaixaDAO();
                _caixa = dao.GetById(_id);

                Txbid.Text = _caixa.Id.ToString();
                Txbmes.Text = _caixa.Mes;
                Txbsaldoinicial.Text = _caixa.SaldoInicial.ToString();
                Txbsaldofinal.Text = _caixa.SaldoFinal.ToString();
                Txbtotaldespesa.Text = _caixa.TotalDespesa.ToString();
                Txbtotallucro.Text = _caixa.TotalLucro.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exceção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveData()
        {
            try
            {
                if (Validate())
                {
                    var dao = new CaixaDAO();
                    var text = "atualizado";

                    if (_caixa.Id == 0)
                    {
                        if (Txbsaldoinicial.Text == null)
                        {
                            dao.Insert(_caixa);
                            text = "adicionado";
                        }
                        else
                        {
                            dao.InsertInicial(_caixa);
                            text = "adicionado";
                        }
                    }
                    else
                        dao.Update(_caixa);

                    MessageBox.Show($"O Caixa foi {text} com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não Executado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            var validator = new CaixaValidator();
            var result = validator.Validate(_caixa);

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
