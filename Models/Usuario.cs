using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisAdv.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public String NomeUser { get; set; }

        public String Senha { get; set; }

        public Advogado Advogado { get; set; }

        private static Usuario _instance;

        public Usuario() { }

        public static Usuario GetInstance()
        {
            if (_instance == null)
                _instance = new Usuario();

            return _instance;
        }

        public static bool Login(string usuario, string senha)
        {
            var user = new UsuarioDAO().GetById(usuario, senha);

            if (user != null)
                return true;

            return false;
        }

        public static int GetFuncionarioId()
        {
            return _instance.Advogado.Id;
        }
    }
}
