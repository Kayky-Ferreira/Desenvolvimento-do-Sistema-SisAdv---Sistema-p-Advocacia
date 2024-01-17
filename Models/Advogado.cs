using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    public class Advogado
    {
        public int Id { get; set; }

        public String Nome { get; set; }

        public String Cpf { get; set; }

        public String Rg { get; set; }

        public String Telefone { get; set; }

        public String Descricao { get; set; }

        public String Email { get; set; }

        public DateTime? DataNasc { get; set; }
    }
}
