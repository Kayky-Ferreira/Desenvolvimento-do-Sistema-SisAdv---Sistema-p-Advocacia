using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    public class Processo
    {
        public int Id { get; set; }

        public double Valor { get; set; }

        public string Descricao { get; set; }

        public DateTime? DataProcesso { get; set; }

        public string Status { get; set; }

        public string Resultado { get; set; }

        public Cliente Cliente { get; set; }

        public Advogado Advogado { get; set; }

        public Servico Servico { get; set; }
    }
}