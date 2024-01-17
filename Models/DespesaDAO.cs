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
    class DespesaDAO : IDAO<Despesa>
    {
        private static Conexao conn;

        public DespesaDAO()
        {
            conn = new Conexao();
        }


        public void Delete(Despesa t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "DELETE FROM despesa WHERE id_despesa = @id";

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

        public Despesa GetById(int id)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "SELECT * FROM despesa WHERE id_despesa = @id";

                query.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = query.ExecuteReader();

                if (!reader.HasRows)
                    throw new Exception("Nenhum registro foi encontrado!");

                var despesa = new Despesa();

                while (reader.Read())
                {
                    despesa.Id = reader.GetInt32("id_despesa");
                    despesa.Valor = reader.GetDouble("valor_desp");
                    despesa.Data = reader.GetDateTime("data_desp");
                    despesa.Descricao = DAOHelper.GetString(reader, "descricao_desp");
                    despesa.Origem = reader.GetString("origem_desp");
                    despesa.FormaPagamento = reader.GetString("forma_pagamento");
                    despesa.Mensal = reader.GetBoolean("mensal_desp");
                }

                return despesa;
                

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

        public void Insert(Despesa t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "CALL inserirDespesa(@data, @valor, @origem, @descricao, @mensal, @pagamento)";

                query.Parameters.AddWithValue("@data", t.Data?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@valor", t.Valor);
                query.Parameters.AddWithValue("@origem", t.Origem);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@mensal", t.Mensal);
                query.Parameters.AddWithValue("@pagamento", t.FormaPagamento);

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

        public List<Despesa> List()
        {
            try
            {
                List<Despesa> list = new List<Despesa>();

                var query = conn.Query();
                query.CommandText = "SELECT * FROM despesa";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    while (reader.Read())
                    {
                        list.Add(new Despesa()
                        {
                            Id = reader.GetInt32("id_despesa"),
                            Origem = reader.GetString("origem_desp"),
                            Data = DAOHelper.GetDateTime(reader, "data_desp"),
                            Valor = DAOHelper.GetDouble(reader, "valor_desp")
                        });
                    }
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

        public List<Despesa> ListConsulta(string origem, string data, double valor)
        {
            try
            {
                string textoSelect = "SELECT  * FROM despesa WHERE";

                List<Despesa> listConsulta = new List<Despesa>();

                var query = conn.Query();

                if ((origem != null) && (data != null) && (valor != 0.0))
                    query.CommandText = $"{textoSelect} origem_desp LIKE '%{origem}%' and data_desp = '{data}' and valor_desp = {valor}";
                else if ((origem != null) && (data != null))
                    query.CommandText = $"{textoSelect} origem_desp LIKE '%{origem}%' and data_desp = '{data}'";
                else if ((origem != null) && (valor != 0.0))
                    query.CommandText = $"{textoSelect} origem_desp LIKE '%{origem}%' and valor_desp = {valor}";
                else if ((valor != 0.0) && (data != null))
                    query.CommandText = $"{textoSelect} data_desp = '{data}' and valor_desp = {valor}";
                else if (valor != 0.0)
                    query.CommandText = $"{textoSelect} valor_desp = {valor}";
                else if (origem != null)
                    query.CommandText = $"{textoSelect} origem_desp LIKE '%{origem}%'";
                else if (data != null)
                    query.CommandText = $"{textoSelect} data_desp = '{data}'";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    listConsulta.Add(new Despesa()
                    {
                        Id = reader.GetInt32("id_despesa"),
                        Origem = reader.GetString("origem_desp"),
                        Data = DAOHelper.GetDateTime(reader, "data_desp"),
                        Valor = DAOHelper.GetDouble(reader, ("valor_desp"))
                    });
                }

                return listConsulta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(Despesa t)
        {
            try
            {
                var query = conn.Query();

                query.CommandText = "UPDATE despesa SET data_desp = @data, valor_desp = @valor, origem_desp = @origem," +
                                    "descricao_desp = @descricao, mensal_desp = @mensal, forma_pagamento = @pagamento WHERE id_despesa = @id; ";

                query.Parameters.AddWithValue("@data", t.Data?.ToString("yyyy-MM-dd"));
                query.Parameters.AddWithValue("@valor", t.Valor);
                query.Parameters.AddWithValue("@origem", t.Origem);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@mensal", t.Mensal);
                query.Parameters.AddWithValue("@pagamento", t.FormaPagamento);


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
