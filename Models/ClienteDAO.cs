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
    class ClienteDAO : IDAO<Cliente>
    { 
        private static Conexao conn;

        public ClienteDAO()
        {
            conn = new Conexao();
        }

        public void Delete(Cliente t)
        {
            try
            {
                var query = conn.Query();

                //Verificar tabela processo;
                query.CommandText = "DELETE FROM cliente WHERE id_cliente = @id";

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

        public Cliente GetById(int id)
        {
            try
            {
                //Verificar código do Mateus, com minhas alterações, parou de funcionar
                var query = conn.Query();
                query.CommandText = "SELECT * FROM cliente LEFT JOIN endereco ON id_endereco = fk_endereco WHERE id_cliente = @id";

                query.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = query.ExecuteReader();

                if (!reader.HasRows)
                    throw new Exception("Nenhum registro foi encontrado!");

                var cliente = new Cliente();

                while (reader.Read())
                {
                    cliente.Id = reader.GetInt32("id_cliente");                    
                    cliente.Cpf = reader.GetString("cpf_cli");
                    cliente.Nome = reader.GetString("nome_cli");
                    cliente.Rg = reader.GetString("rg_cli");
                    cliente.Email = reader.GetString("email_cli");
                    cliente.Telefone = reader.GetString("telefone_cli");
                    cliente.Profissao = DAOHelper.GetString(reader, "profissao_cli");
                    cliente.Descricao = DAOHelper.GetString(reader, "descricao_cli");

                    if (!DAOHelper.IsNull(reader, "fk_endereco"))
                        cliente.Endereco = new Endereco()
                        {
                            Id = reader.GetInt32("id_endereco"),
                            Rua = reader.GetString("rua_end"),
                            Numero = reader.GetInt32("numero_residencia_end"),
                            Bairro = reader.GetString("bairro_end"),
                            Cidade = reader.GetString("cidade_end"),
                            Estado = reader.GetString("estado")
                        };
                }               

                return cliente;

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

        public void Insert(Cliente t)
        {
            try
            {
                //inserção endereco cliente
                var enderecoId = new EnderecoDAO().Insert(t.Endereco);

                var query = conn.Query();
                query.CommandText = "CALL inserirCliente(@nome, @email, @cpf, @rg, @telefone, @profissao, @descricao, @enderecoID)";

                query.Parameters.AddWithValue("@nome", t.Nome);
                query.Parameters.AddWithValue("@email", t.Email);
                query.Parameters.AddWithValue("@cpf", t.Cpf);
                query.Parameters.AddWithValue("@rg", t.Rg);
                query.Parameters.AddWithValue("@telefone", t.Telefone);
                query.Parameters.AddWithValue("@profissao", t.Profissao);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@enderecoID", enderecoId);

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

        public List<Cliente> List()
        {
            try
            {
                List<Cliente> list = new List<Cliente>();

                var query = conn.Query();
                query.CommandText = "SELECT * FROM cliente";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Cliente()
                    {
                        Id = reader.GetInt32("id_cliente"),
                        Nome = reader.GetString("nome_cli"),
                        Rg = reader.GetString("rg_cli"),
                        Cpf = reader.GetString("cpf_cli")
                    });
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cliente> ListConsulta(string nome, string rg, string cpf)
        {
            try
            {
                string textoSelect = "SELECT  * FROM cliente WHERE";

                List<Cliente> listConsulta = new List<Cliente>();

                var query = conn.Query();

                if ((nome != null) && (rg != null) && (cpf != null))
                    query.CommandText = $"{textoSelect} nome_cli like '%{nome}%' and rg_cli like '{rg}%' and cpf_cli like '{cpf}%'";
                else if ((nome != null) && (rg != null))
                    query.CommandText = $"{textoSelect} nome_cli like '%{nome}%' and rg_cli like '{rg}%'";
                else if ((nome != null) && (cpf != null))
                    query.CommandText = $"{textoSelect} nome_cli like '%{nome}%' and cpf_cli like '{cpf}%'";
                else if ((cpf != null) && (rg != null))
                    query.CommandText = $"{textoSelect} rg_cli like '{rg}%' and cpf_cli like '{cpf}%'";
                else if (cpf != null)
                    query.CommandText = $"{textoSelect} cpf_cli like '{cpf}%'";
                else if (nome != null)
                    query.CommandText = $"{textoSelect} nome_cli like '%{nome}%'";
                else if (rg != null)
                    query.CommandText = $"{textoSelect} rg_cli like '{rg}%'";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    listConsulta.Add(new Cliente()
                    {
                        Id = reader.GetInt32("id_cliente"),
                        Nome = reader.GetString("nome_cli"),
                        Rg = reader.GetString("rg_cli"),
                        Cpf = reader.GetString("cpf_cli")
                    });
                }

                return listConsulta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(Cliente t)
        {
            try
            {
                long enderecoId = t.Endereco.Id;
                var endDAO = new EnderecoDAO();

                if (enderecoId > 0)
                    endDAO.Update(t.Endereco);
                else
                    enderecoId = endDAO.Insert(t.Endereco);

                var query = conn.Query();

                query.CommandText = "UPDATE cliente SET nome_cli = @nome, cpf_cli = @cpf, rg_cli = @rg, telefone_cli = @telefone," +  
                                    "profissao_cli = @profissao, descricao_cli = @descricao, email_cli = @email, fk_endereco = @enderecoID WHERE id_cliente = @id";

                query.Parameters.AddWithValue("@nome", t.Nome);
                query.Parameters.AddWithValue("@cpf", t.Cpf);
                query.Parameters.AddWithValue("@rg", t.Rg);
                query.Parameters.AddWithValue("@telefone", t.Telefone);
                query.Parameters.AddWithValue("@profissao", t.Profissao);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@email", t.Email);
                query.Parameters.AddWithValue("@enderecoID", enderecoId);

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
