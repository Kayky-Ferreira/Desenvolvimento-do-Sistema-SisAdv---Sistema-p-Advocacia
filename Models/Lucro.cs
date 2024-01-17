using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    public class Lucro
    {
        public int Id { get; set; }
        public String Origem { get; set; }
        public String FormaRecebimento { get; set; }
        public String Descricao { get; set; }
        public DateTime? Data { get; set; }
        public Boolean Mensal { get; set; }
        public double Valor { get; set; }
        public Caixa Caixa { get; set; }
        public Servico Servico { get; set; }
    }
}
