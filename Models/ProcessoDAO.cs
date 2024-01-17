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
using System.Windows;

namespace SisAdv.Models
{
    class ProcessoDAO : IDAO<Processo>
    {
        private Conexao conn;

        public ProcessoDAO()
        {
            conn = new Conexao();
        }

        public void Delete(Processo t)
        {
            try
            {
                var query = conn.Query();

                //Verificar tabela processo;
                query.CommandText = "DELETE FROM processo WHERE id_proc = @id";

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

        public Processo GetById(int id)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "SELECT * FROM processo LEFT JOIN cliente ON fk_cliente = id_cliente " +
                                                           "LEFT JOIN advogado ON fk_advogado = id_advogado LEFT JOIN servico ON fk_servico = id_servico WHERE id_proc = @id";

                query.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = query.ExecuteReader();

                if (!reader.HasRows)
                    throw new Exception("Nenhum registro foi encontrado!");

                var processo = new Processo();

                while (reader.Read())
                {
                    processo.Id = reader.GetInt32("id_proc");
                    processo.Valor = DAOHelper.GetDouble(reader, "valor_proc");
                    processo.DataProcesso = DAOHelper.GetDateTime(reader, "data_inicio_proc");
                    processo.Descricao = DAOHelper.GetString(reader, "descricao_proc");
                    processo.Status = DAOHelper.GetString(reader, "status_proc");
                    processo.Resultado = DAOHelper.GetString(reader, "resultado");

                    //CÓDIGO AULA 1:N

                    if (!DAOHelper.IsNull(reader, "fk_cliente"))
                        processo.Cliente = new Cliente()
                        {
                            Id = reader.GetInt32("id_cliente"),
                            Nome = reader.GetString("nome_cli")
                        };

                    if (!DAOHelper.IsNull(reader, "fk_advogado"))
                        processo.Advogado = new Advogado()
                        {
                            Id = reader.GetInt32("id_advogado"),
                            Nome = reader.GetString("nome_adv")
                        };

                    if (!DAOHelper.IsNull(reader, "fk_servico"))
                        processo.Servico = new Servico()
                        {
                            Id = reader.GetInt32("id_servico"),
                            Descricao = reader.GetString("descricao_serv")
                        };
                }

                return processo;

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

        public void Insert(Processo t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "CALL registrarProcesso(@descricao, @data, @status, @resultado, @servico)";

                query.Parameters.AddWithValue("@descricao", t.Descricao);
                //query.Parameters.AddWithValue("@valor", t.Valor);
                query.Parameters.AddWithValue("@data", t.DataProcesso?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@status", t.Status);
                query.Parameters.AddWithValue("@resultado", t.Resultado);
                query.Parameters.AddWithValue("@servico", t.Servico.Id);

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

        public List<Processo> List()
        {
            try
            {
                List<Processo> list = new List<Processo>();

                var query = conn.Query();
                query.CommandText = "SELECT * FROM processo";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Processo()
                    {
                        Id = reader.GetInt32("id_proc"),
                        Status = DAOHelper.GetString(reader, "status_proc"),
                        DataProcesso = DAOHelper.GetDateTime(reader, "data_inicio_proc"),
                        Resultado = DAOHelper.GetString(reader, "resultado"),
                        Descricao = DAOHelper.GetString(reader, "descricao_proc"),
                        Valor = DAOHelper.GetDouble(reader, "valor_proc"),

                        Cliente = DAOHelper.IsNull(reader, "fk_cliente") ? null : new Cliente() { Id = reader.GetInt32("fk_cliente"), Nome = reader.GetString("cliente_proc") },
                        Advogado = DAOHelper.IsNull(reader, "fk_advogado") ? null : new Advogado() { Id = reader.GetInt32("fk_advogado") },
                        Servico = DAOHelper.IsNull(reader, "fk_servico") ? null : new Servico() { Id = reader.GetInt32("fk_servico") },
                    });
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

        public List<Processo> ListConsulta(string cliente, string status, double valor)
        {
            try
            {
                string textoSelect = "SELECT * FROM processo LEFT JOIN cliente ON fk_cliente = id_cliente WHERE";
                List<Processo> listconsulta = new List<Processo>();

                var query = conn.Query();

                if ((cliente != null) && (status != null) && (valor != 0.0))
                    query.CommandText = $"{textoSelect} nome_cli LIKE '%{cliente}%' AND status_proc LIKE '%{status}%' AND valor_proc = {valor}";
                else if ((cliente != null) && (status != null))
                    query.CommandText = $"{textoSelect} nome_cli LIKE '%{cliente}%' AND status_proc LIKE '%{status}%'";
                else if ((cliente != null) && (valor != 0.0))
                    query.CommandText = $"{textoSelect} nome_cli LIKE '%{cliente}%' AND valor_proc = {valor}";
                else if ((status != null) && (valor != 0.0))
                    query.CommandText = $"{textoSelect} status_proc LIKE '%{status}%' AND valor_proc = {valor}";
                else if (status != null)
                    query.CommandText = $"{textoSelect} status_proc LIKE '%{status}%'";
                else if (cliente != null)
                    query.CommandText = $"{textoSelect} nome_cli LIKE '%{cliente}%'";
                else if (valor != 0.0)
                    query.CommandText = $"{textoSelect} valor_proc = {valor}";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    listconsulta.Add(new Processo()
                    {
                        Id = reader.GetInt32("id_proc"),
                        Status = DAOHelper.GetString(reader, "status_proc"),
                        DataProcesso = DAOHelper.GetDateTime(reader, "data_inicio_proc"),
                        Resultado = DAOHelper.GetString(reader, "resultado"),
                        Descricao = DAOHelper.GetString(reader, "descricao_proc"),
                        Valor = DAOHelper.GetDouble(reader, "valor_proc"),

                        Cliente = DAOHelper.IsNull(reader, "fk_cliente") ? null : new Cliente() { Id = reader.GetInt32("fk_cliente"), Nome = reader.GetString("cliente_proc") },
                        //Advogado = DAOHelper.IsNull(reader, "fk_advogado") ? null : new Advogado() { Id = reader.GetInt32("fk_advogado") }
                        //Servico = DAOHelper.IsNull(reader, "fk_servico") ? null : new Servico() { Id = reader.GetInt32("fk_servico") }
                    });
                }

                return listconsulta;
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

        public void Update(Processo t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "UPDATE processo SET descricao_proc = @descricao, data_inicio_proc = @data, " +
                                    "status_proc = @status, resultado = @resultado, " +
                                    "cliente_proc = (select cliente_serv from servico where id_servico = @servico), " +
                                    "fk_cliente = (select fk_cliente from servico where id_servico = @servico), " +
                                    "fk_advogado = (select fk_advogado from servico where id_servico = @servico), " +
                                    "valor_proc = (select valor_serv from servico where id_servico = @servico), " +
                                    "fk_servico = @servico WHERE id_proc = @id ";

                query.Parameters.AddWithValue("@valor", t.Valor);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@data", t.DataProcesso?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@status", t.Status);
                query.Parameters.AddWithValue("@resultado", t.Resultado);
                query.Parameters.AddWithValue("@servico", t.Servico.Id);

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
