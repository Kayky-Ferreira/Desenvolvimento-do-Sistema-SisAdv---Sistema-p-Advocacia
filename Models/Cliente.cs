using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisAdv.Models;

namespace SisAdv.Models
{
    public class Cliente
    {
        public String Nome { get; set; }        
        public String Rg { get; set; }
        public int Id { get; set; }
        public String Profissao { get; set; }
        public String Descricao { get; set; }
        public String Telefone { get; set; }
        public String Email { get; set; }
        public String Cpf { get; set; }
        public Endereco Endereco { get; set; }
    }
}
