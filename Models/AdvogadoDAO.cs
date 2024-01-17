using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SisAdv.Database;
using SisAdv.Interface;
using SisAdv.Helpers;

namespace SisAdv.Models
{
    class AdvogadoDAO : IDAO<Advogado>
    {
        private static Conexao conn;

        public AdvogadoDAO()
        {
            conn = new Conexao();
        }
    
        public void Delete(Advogado t)
        {
            try
            {
                var query = conn.Query();

                //Verificar tabela processo;
                query.CommandText = "DELETE FROM advogado WHERE id_advogado = @id";

                query.Parameters.AddWithValue("@id", t.Id);

                var result = query.ExecuteNonQuery();

                if (result == 0)
                    throw new Exception("Registro não foi deletado da base de dados. Verifique e tente novamente.");

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

        public Advogado GetById(int id)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "SELECT * FROM advogado WHERE id_advogado = @id";

                query.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = query.ExecuteReader();

                if (!reader.HasRows)
                    throw new Exception("Nenhum registro foi encontrado!");

                var advogado = new Advogado();

                while (reader.Read())
                {
                    advogado.Id = reader.GetInt32("id_advogado");
                    advogado.Nome = reader.GetString("nome_adv");
                    advogado.DataNasc = DAOHelper.GetDateTime(reader, "data_nasc_adv");
                    advogado.Descricao = reader.GetString("descricao_adv");
                    advogado.Rg = reader.GetString("rg_adv");
                    advogado.Telefone = reader.GetString("telefone_adv");
                    advogado.Cpf = reader.GetString("cpf_adv");
                    advogado.Email = reader.GetString("e_mail_adv");
                }

                return advogado;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Query();
            }
        }

        public void Insert(Advogado t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "CALL inserirAdvogado(@nome, @cpf, @rg, @data, @email, @telefone, @descricao)";

                query.Parameters.AddWithValue("@nome", t.Nome);
                query.Parameters.AddWithValue("@data", t.DataNasc?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@cpf", t.Cpf);
                query.Parameters.AddWithValue("@rg", t.Rg);
                query.Parameters.AddWithValue("@email", t.Email);
                query.Parameters.AddWithValue("@telefone", t.Telefone);
                query.Parameters.AddWithValue("@descricao", t.Descricao);

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

        public List<Advogado> List()
        {
            try
            {
                List<Advogado> list = new List<Advogado>();

                var query = conn.Query();
                query.CommandText = "SELECT * FROM advogado";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Advogado()
                    {
                        Id = reader.GetInt32("id_advogado"),
                        Nome = reader.GetString("nome_adv"),
                        Descricao = reader.GetString("descricao_adv"),
                        Rg = reader.GetString("rg_adv"),
                        Cpf = reader.GetString("cpf_adv"),
                        Telefone = reader.GetString("telefone_adv"),
                        Email = reader.GetString("e_mail_adv"),
                        DataNasc = reader.GetDateTime("data_nasc_adv")
                    });
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Advogado> ListConsulta(string nomeAdvogado, string cpfAdvogado)
        {
            try
            {
                string textoSelect = "SELECT * FROM advogado WHERE";

                List<Advogado> list = new List<Advogado>();
                var query = conn.Query();

                if ((nomeAdvogado != null) && (cpfAdvogado != null))
                    query.CommandText = $"{textoSelect} nome_adv LIKE '%{nomeAdvogado}%' AND cpf_adv LIKE '{cpfAdvogado}%'";
                else if (nomeAdvogado != null)
                    query.CommandText = $"{textoSelect} nome_adv LIKE '%{nomeAdvogado}%'";
                else if (cpfAdvogado != null)
                    query.CommandText = $"{textoSelect} cpf_adv LIKE '{cpfAdvogado}%'";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Advogado()
                    {
                        Id = reader.GetInt32("id_advogado"),
                        Nome = reader.GetString("nome_adv"),
                        Descricao = reader.GetString("descricao_adv"),
                        Rg = reader.GetString("rg_adv"),
                        Cpf = reader.GetString("cpf_adv"),
                        Telefone = reader.GetString("telefone_adv"),
                        Email = reader.GetString("e_mail_adv"),
                        DataNasc = reader.GetDateTime("data_nasc_adv")
                    });
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(Advogado t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "UPDATE advogado SET nome_adv = @nome, cpf_adv = @cpf, rg_adv = @rg, data_nasc_adv = @data," +
                                    "e_mail_adv = @email, telefone_adv = @telefone, descricao_adv = @descricao WHERE id_advogado = @id";

                query.Parameters.AddWithValue("@nome", t.Nome);
                query.Parameters.AddWithValue("@cpf", t.Cpf);
                query.Parameters.AddWithValue("@rg", t.Rg);
                query.Parameters.AddWithValue("@data", t.DataNasc);
                query.Parameters.AddWithValue("@email", t.Email);
                query.Parameters.AddWithValue("@telefone", t.Telefone);
                query.Parameters.AddWithValue("@descricao", t.Descricao);


                query.Parameters.AddWithValue("@id", t.Id);

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
