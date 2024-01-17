using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SisAdv.Database;
using SisAdv.Interface;

namespace SisAdv.Models
{
    class UsuarioDAO : AbstractDAO<Usuario>
    {
        public Usuario GetById(string usuarioNome, string senha)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "SELECT * FROM usuario LEFT JOIN advogado ON fk_advogado = id_advogado " + 
                    "WHERE nome_user = @usuario AND senha_user = @senha;";

                query.Parameters.AddWithValue("@usuario", usuarioNome);
                query.Parameters.AddWithValue("@senha", senha);

                MySqlDataReader reader = query.ExecuteReader();

                Usuario usuario = null;

                while (reader.Read())
                {
                    usuario = Usuario.GetInstance();
                    usuario.Id = reader.GetInt32("id_user");
                    usuario.NomeUser = reader.GetString("nome_user");
                    usuario.Advogado = new Advogado() { Id = reader.GetInt32("fk_advogado"), Nome = reader.GetString("nome_user") };
                }

                return usuario;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override void Insert(Usuario t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "CALL inserirUsuario (@senha, @nome, @advogado)";

                query.Parameters.AddWithValue("@senha", t.Senha);
                query.Parameters.AddWithValue("@nome", t.NomeUser);
                query.Parameters.AddWithValue("@advogado", t.Advogado.Id);

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetName(0).Equals("Alerta"))
                    {
                        throw new Exception(reader.GetString("Alerta"));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public override void Update(Usuario t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "UPDATE usuario SET senha_user = @senha WHERE fk_advogado = @advogado";

                query.Parameters.AddWithValue("@senha", t.Senha);
                query.Parameters.AddWithValue("@advogado", t.Advogado.Id);

                var result = query.ExecuteNonQuery();

                if (result == 0)
                    throw new Exception("Atualização do registro não foi realizada.");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
