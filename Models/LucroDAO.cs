using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SisAdv.Interface;
using SisAdv.Database;
using SisAdv.Helpers;

namespace SisAdv.Models 
{
    class LucroDAO : IDAO<Lucro>
    {
        private static Conexao conn;

        public LucroDAO()
        {
            conn = new Conexao();
        }

        public void Delete(Lucro t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "DELETE FROM lucro WHERE id_lucro = @id";

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

        public Lucro GetById(int id)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "SELECT * FROM lucro LEFT JOIN caixa ON fk_caixa = id_cx " +
                                    "LEFT JOIN servico ON fk_servico = id_servico WHERE id_lucro = @id; ";

                query.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = query.ExecuteReader();

                if (!reader.HasRows)
                    throw new Exception("Nenhum registro foi encontrado!");

                var lucro = new Lucro();

                while (reader.Read())
                {
                    lucro.Id = reader.GetInt32("id_lucro");
                    lucro.Origem = reader.GetString("origem_luc");
                    lucro.Data = reader.GetDateTime("data_luc");
                    lucro.Valor = reader.GetDouble("valor_luc");
                    lucro.Descricao = reader.GetString("descricao_luc");
                    lucro.FormaRecebimento = reader.GetString("forma_pagamento");
                    lucro.Mensal = reader.GetBoolean("mensal_luc");

                    if (!DAOHelper.IsNull(reader, "fk_caixa"))
                        lucro.Caixa = new Caixa()
                        {
                            Id = reader.GetInt32("id_cx"),
                            Mes = reader.GetString("mes_cx")
                        };

                    if (!DAOHelper.IsNull(reader, "fk_servico"))
                        lucro.Servico = new Servico()
                        {
                            Id = reader.GetInt32("id_servico"),
                            Descricao = reader.GetString("descricao_serv")
                        };
                }

                return lucro;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Insert(Lucro t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "CALL cadastrarLucro(@data, @origem, @descricao, @pagamento, @mensal, @caixa, @servico)";

                query.Parameters.AddWithValue("@data", t.Data?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@origem", t.Origem);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@mensal", t.Mensal);
                query.Parameters.AddWithValue("@pagamento", t.FormaRecebimento);
                query.Parameters.AddWithValue("@caixa", t.Caixa.Id);
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

        public List<Lucro> List()
        {
            try
            {
                List<Lucro> list = new List<Lucro>();
                var query = conn.Query();

                query.CommandText = "SELECT  * FROM lucro";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Lucro()
                    {
                        Id = reader.GetInt32("id_lucro"),
                        Origem = DAOHelper.GetString(reader, "origem_luc"),
                        Data = DAOHelper.GetDateTime(reader, "data_luc"),
                        Valor = DAOHelper.GetDouble(reader, "valor_luc"),
                        Descricao = DAOHelper.GetString(reader, "descricao_luc"),
                        FormaRecebimento = DAOHelper.GetString(reader, "forma_pagamento"),
                        Mensal = reader.GetBoolean("mensal_luc"),

                        Caixa = DAOHelper.IsNull(reader, "fk_caixa") ? null : new Caixa() { Id = reader.GetInt32("fk_caixa") },
                        Servico = DAOHelper.IsNull(reader, "fk_servico") ? null : new Servico { Id = reader.GetInt32("fk_servico") }
                    }); ;
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Lucro> ListConsulta(string origem, string data, double valor, int caixa)
        {
            try
            {
                string textoSelect = "SELECT * FROM lucro WHERE";

                List<Lucro> listConsulta = new List<Lucro>();

                var query = conn.Query();

                if ((origem != null) && (data != null) && (valor != 0.0))
                    query.CommandText = $"{textoSelect} origem_luc LIKE '%{origem}%' and data_luc = '{data}' and valor_luc = {valor}";
                else if ((origem != null) && (data != null))
                    query.CommandText = $"{textoSelect} origem_luc LIKE '%{origem}%' and data_luc = '{data}'";
                else if ((origem != null) && (valor != 0.0))
                    query.CommandText = $"{textoSelect} origem_luc LIKE '%{origem}%' and valor_luc = {valor}";
                else if ((valor != 0.0) && (data != null))
                    query.CommandText = $"{textoSelect} data_luc = '{data}' and valor_luc = {valor}";
                else if (valor != 0.0)
                    query.CommandText = $"{textoSelect} valor_luc = {valor}";
                else if (origem != null)
                    query.CommandText = $"{textoSelect} origem_luc LIKE '%{origem}%'";
                else if (data != null)
                    query.CommandText = $"{textoSelect} data_luc = '{data}'";
                else if (caixa != 0)
                    query.CommandText = $"{textoSelect} fk_caixa = '{caixa}'";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    listConsulta.Add(new Lucro()
                    {
                        Id = reader.GetInt32("id_lucro"),
                        Origem = DAOHelper.GetString(reader, "origem_luc"),
                        Data = DAOHelper.GetDateTime(reader, "data_luc"),
                        Valor = DAOHelper.GetDouble(reader, "valor_luc"),
                        Descricao = DAOHelper.GetString(reader, "descricao_luc"),
                        FormaRecebimento = DAOHelper.GetString(reader, "forma_pagamento"),
                        Mensal = reader.GetBoolean("mensal_luc"),

                        Caixa = DAOHelper.IsNull(reader, "fk_caixa") ? null : new Caixa() { Id = reader.GetInt32("fk_caixa") },
                        Servico = DAOHelper.IsNull(reader, "fk_servico") ? null : new Servico { Id = reader.GetInt32("fk_servico") }
                    });
                }

                return listConsulta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(Lucro t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "UPDATE lucro SET data_luc = @data, valor_luc = (select valor_serv from servico where id_servico = @servico), origem_luc = @origem, " +
                                    "descricao_luc = @descricao, mensal_luc = @mensal, forma_pagamento = @formarecebimento, fk_caixa = @caixa, fk_servico = @servico WHERE id_lucro = @id";

                query.Parameters.AddWithValue("@data", t.Data?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@origem", t.Origem);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@mensal", t.Mensal);
                query.Parameters.AddWithValue("@formarecebimento", t.FormaRecebimento);
                query.Parameters.AddWithValue("@caixa", t.Caixa.Id);
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
