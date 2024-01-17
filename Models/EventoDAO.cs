using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisAdv.Interface;
using SisAdv.Database;
using MySql.Data.MySqlClient;
using SisAdv.Helpers;
using SisAdv.Views;
using System.Windows.Controls;

namespace SisAdv.Models
{
    class EventoDAO : IDAO<Evento>
    {
        private static Conexao conn;

        public EventoDAO()
        {
            conn = new Conexao();
        }

        public void Delete(Evento t)
        {
            throw new NotImplementedException();
        }

        public Evento GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Evento t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "CALL inserirEvento (@titulo,  @data, @horario, @descricao, @importancia, @notificacao)";

                query.Parameters.AddWithValue("@titulo", t.Titulo);
                query.Parameters.AddWithValue("@horario", t.Horario);
                query.Parameters.AddWithValue("@data", t.Data);
                query.Parameters.AddWithValue("@descricao", t.Descricao);
                query.Parameters.AddWithValue("@importancia", t.Importancia);
                query.Parameters.AddWithValue("@notificacao", t.Notificacao);

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

        public List<Evento> List()
        {
            try
            {
                List<Evento> list = new List<Evento>();

                var query = conn.Query();
                query.CommandText = "SELECT * FROM evento";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Evento()
                    {
                        Id = reader.GetInt32("id_evento"),
                        Importancia = reader.GetString("importancia_even"),
                        Horario = DAOHelper.GetString(reader, "horario_even"),
                        Data = DAOHelper.GetDateTime(reader, "data_even"),
                        Descricao = DAOHelper.GetString(reader, "descricao_even"),
                        Notificacao = reader.GetBoolean("notificacao_even"),
                        Titulo = DAOHelper.GetString(reader, "titulo_even")
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

        /*public List<Evento> ListDataCalendar(System.Windows.Controls.Calendar teste)
        {
            try
            {
                List<Evento> list = new List<Evento>();

                var query = conn.Query();
                query.CommandText = "SELECT data_even FROM evento";

                MySqlDataReader reader = query.ExecuteReader();

                DateTime?[] vetoooor = new DateTime?[100];
                int i = 0;

                while (reader.Read())
                {
                    i += 1;

                    list.Add(new Evento()
                    {
                        Data = DAOHelper.GetDateTime(reader, "data_even")
                    });

                    vetoooor[i] = list.;

                    teste.SelectionMode = CalendarSelectionMode.MultipleRange;
                    teste.SelectedDates.Add(new DateTime(list));
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

        /* Código do calendário, vou tentar implementar esse que achei na internet
         string _query = "SELECT * FROM calendario WHERE estado=1 AND utilizador=@user;";

            using (MySqlConnection con  = new MySqlConnection(ConSql))
            {
                using (MySqlCommand cmd = new MySqlCommand(_query, con))
                {
                    con.Open();
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@user", Sessao.Id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            		monthCalendar1.AddBoldedDate(DateTime.Parse(funcao.OutHoras(reader["dataAviso"].ToString())));

                        }
                        reader.Close();
                    }
                }
                con.Close();
            }
         * */

        public List<Evento> ListConsulta(string dataAgenda)
        {
            try
            {
                List<Evento> list = new List<Evento>();

                var query = conn.Query();
                query.CommandText = $"SELECT * FROM evento WHERE data_even = '{dataAgenda}'";

                MySqlDataReader reader = query.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Evento()
                    {
                        Id = reader.GetInt32("id_evento"),
                        Importancia = DAOHelper.GetString(reader, "importancia_even"),
                        Horario = DAOHelper.GetString(reader, "horario_even"),
                        Data = DAOHelper.GetDateTime(reader, "data_even"),
                        Descricao = DAOHelper.GetString(reader, "descricao_even"),
                        Notificacao = reader.GetBoolean("notificacao_even"),
                        Titulo = DAOHelper.GetString(reader, "titulo_even")
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
            /*Implementar aqui
            try
            {
                string textoSelect = "SELECT  * FROM servico LEFT JOIN cliente ON fk_cliente = id_cliente LEFT JOIN advogado ON fk_advogado = id_advogado WHERE";

                List<Servico> listConsulta = new List<Servico>();

                var query = conn.Query();

                

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
            }*/
        }

        public void Update(Evento t)
        {
            throw new NotImplementedException();
        }
    }
}
