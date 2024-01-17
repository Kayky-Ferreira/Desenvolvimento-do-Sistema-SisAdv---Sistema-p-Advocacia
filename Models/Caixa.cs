using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    public class Caixa
    {
        public int Id { get; set; }
        public string Mes { get; set; }
        public double SaldoInicial { get; set; }
        public double TotalLucro { get; set; }
        public double TotalDespesa { get; set; }
        public double SaldoFinal { get; set; }
    }
}