using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    public class Despesa
    {
        public int Id { get; set; }
        public String Origem { get; set; }
        public DateTime? Data { get; set; }
        public Boolean Mensal { get; set; }
        public double Valor { get; set; }
        public String FormaPagamento { get; set; }
        public String Descricao { get; set; }
    }
}
