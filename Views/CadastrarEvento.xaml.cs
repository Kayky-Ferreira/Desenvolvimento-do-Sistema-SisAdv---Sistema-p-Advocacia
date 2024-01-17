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
    /// Lógica interna para Cadastraeventonov.xaml
    /// </summary>
    public partial class Cadastraeventonov : Window
    {
        public Evento _evento;
        public Cadastraeventonov()
        {
            InitializeComponent();
            Loaded += Cadastraeventonov_Loaded;
        }

        private void Cadastraeventonov_Loaded(object sender, RoutedEventArgs e)
        {
            _evento = new Evento();
        }

        private void BntSalvar_Click(object sender, RoutedEventArgs e)
        {
            _evento.Titulo = txbTitulo.Text;
            //Falta adicionar uma caixa para descrição, adicionar na próxima commit
            _evento.Descricao = txbDescricao.Text;
            _evento.Horario = txbHorario.Text;
            _evento.Data = (DateTime)datepickerDataServico.SelectedDate;

            if (boxImportancia.Text == "Baixa")
                _evento.Importancia = "Baixa";
            else if (boxImportancia.Text == "Média")
                _evento.Importancia = "Média";
            else
                _evento.Importancia = "Alta";


            if (rbNotificacao.IsChecked.Value)
            {
                _evento.Notificacao = true;
            }
            else
            {
                _evento.Notificacao = false;
            }

            var eventoDAO = new EventoDAO();
            eventoDAO.Insert(_evento);

            MessageBox.Show("O Evento foi registrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
