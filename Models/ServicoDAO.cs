using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisAdv.Interface;
using MySql.Data.MySqlClient;
using SisAdv.Database;
using SisAdv.Models;
using SisAdv.Helpers;
using SisAdv.Views;

namespace SisAdv.Models
{    
    class ServicoDAO : IDAO<Servico>
    {
        private static Conexao conn;

        public ServicoDAO()
        {
            conn = new Conexao();
        }

        public void Delete(Servico t)
        {
            try
            {
                var query = conn.Query();
                
                //Verificar tabela processo;
                query.CommandText = "DELETE FROM servico WHERE id_servico = @id";

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

        public Servico GetById(int id)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "SELECT * FROM servico LEFT JOIN cliente ON fk_cliente = id_cliente " +
                                                           "LEFT JOIN advogado ON fk_advogado = id_advogado WHERE id_servico = @id";

                query.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = query.ExecuteReader();

                if (!reader.HasRows)
                    throw new Exception("Nenhum registro foi encontrado!");

                var servico = new Servico();

                while (reader.Read())
                {
                    servico.Id = reader.GetInt32("id_servico");
                    servico.Valor = reader.GetDouble("valor_serv");
                    servico.Data = reader.GetDateTime("data_serv");
                    servico.Descricao = reader.GetString("descricao_serv");
                    servico.Tipo = reader.GetString("tipo_serv");

                    //CÓDIGO AULA 1:N

                    if (!DAOHelper.IsNull(reader, "fk_cliente"))
                        servico.Cliente = new Cliente()
                        {
                            Id = reader.GetInt32("id_cliente"),
                            Nome = reader.GetString("nome_cli")
                        };

                    if (!DAOHelper.IsNull(reader, "fk_advogado"))
                        servico.Advogado = new Advogado()
                        {
                            Id = reader.GetInt32("id_advogado"),
                            Nome = reader.GetString("nome_adv")
                        };
                }

                return servico;

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

        public void Insert(Servico t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "CALL inserirServico(@valor, @data, @tipo, @advogado, @cliente, @descricao)";

                query.Parameters.AddWithValue("@valor", t.Valor);
                query.Parameters.AddWithValue("@data", t.Data?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@tipo", t.Tipo);
                query.Parameters.AddWithValue("@advogado", t.Advogado.Id);
                query.Parameters.AddWithValue("@cliente", t.Cliente.Id);
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

        public List<Servico> List()
        {
            try
            {                
                List<Servico> list = new List<Servico>();

                var query = conn.Query();
                query.CommandText = "SELECT * FROM servico";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Servico()
                    {
                        Id = reader.GetInt32("id_servico"),
                        Valor = DAOHelper.GetDouble(reader, "valor_serv"),
                        Data = DAOHelper.GetDateTime(reader, "data_serv"),
                        Tipo = DAOHelper.GetString(reader, "tipo_serv"),
                        Descricao = DAOHelper.GetString(reader,"descricao_serv"),

                        //CÓDIGO AULA 1:N
                        Cliente = DAOHelper.IsNull(reader, "cliente_serv") ? null : new Cliente() { Id = reader.GetInt32("fk_cliente"), Nome = reader.GetString("cliente_serv") },
                        Advogado = DAOHelper.IsNull(reader, "advogado_serv") ? null : new Advogado() { Id = reader.GetInt32("fk_advogado"), Nome = reader.GetString("advogado_serv") }
                    }) ;                    
                }

                return list;
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

        public List<Servico> ListConsulta(string cliente, string data, string tipoServico)
        {
            try
            {
                string textoSelect = "SELECT  * FROM servico LEFT JOIN cliente ON fk_cliente = id_cliente LEFT JOIN advogado ON fk_advogado = id_advogado WHERE";

                List<Servico> listConsulta = new List<Servico>();                

                var query = conn.Query();

                if ((cliente != null) && (data != null) && (tipoServico != null))
                    query.CommandText = $"{textoSelect} cliente_serv LIKE '%{cliente}%' AND data_serv = '{data}' AND tipo_serv = '{tipoServico}'";
                else if ((cliente != null) && (data != null))
                    query.CommandText = $"{textoSelect} cliente_serv LIKE '%{cliente}%' AND data_serv = '{data}'";
                else if ((cliente != null) && (tipoServico != null))
                    query.CommandText = $"{textoSelect} cliente_serv LIKE '%{cliente}%' AND tipo_serv = '{tipoServico}'";
                else if ((tipoServico != null) && (data != null))
                    query.CommandText = $"{textoSelect} data_serv = '{data}' AND tipo_serv = '{tipoServico}'";
                else if (tipoServico != null)
                    query.CommandText = $"{textoSelect} tipo_serv = '{tipoServico}'";                
                else if (cliente != null)                
                    query.CommandText = $"{textoSelect} cliente_serv LIKE '%{cliente}%'";
                else if (data != null)                
                    query.CommandText = $"{textoSelect} data_serv = '{data}'";                

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    listConsulta.Add(new Servico()
                    {
                        Id = reader.GetInt32("id_servico"),
                        Data = DAOHelper.GetDateTime(reader, "data_serv"),
                        Tipo = DAOHelper.GetString(reader, "tipo_serv"),

                        //CÓDIGO AULA 1:N
                        Cliente = DAOHelper.IsNull(reader, "cliente_serv") ? null : new Cliente() { Id = reader.GetInt32("fk_cliente"), Nome = reader.GetString("cliente_serv") },
                        Advogado = DAOHelper.IsNull(reader, "advogado_serv") ? null : new Advogado() { Id = reader.GetInt32("fk_advogado"), Nome = reader.GetString("advogado_serv") }
                    });
                }

                return listConsulta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(Servico t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "UPDATE servico SET valor_serv = @valor, data_serv = @data, tipo_serv = @tipo, " +
                                    "descricao_serv = @descricao, fk_advogado = @advogado, fk_cliente = @cliente, " + 
                                    "advogado_serv = @advogadonome, cliente_serv = @clientenome WHERE id_servico = @id";

                query.Parameters.AddWithValue("@valor", t.Valor);
                query.Parameters.AddWithValue("@data", t.Data);
                query.Parameters.AddWithValue("@tipo", t.Tipo);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@advogado", t.Advogado.Id);
                query.Parameters.AddWithValue("@cliente", t.Cliente.Id);
                query.Parameters.AddWithValue("@clientenome", t.Cliente.Nome);
                query.Parameters.AddWithValue("@advogadonome", t.Advogado.Nome);


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
