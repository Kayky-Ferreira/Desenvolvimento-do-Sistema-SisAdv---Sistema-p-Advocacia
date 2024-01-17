using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisAdv.Database;
using MySql.Data.MySqlClient;

namespace SisAdv.Models
{
    class EnderecoDAO
    {
        private static Conexao conn = new Conexao();

        public long Insert(Endereco t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "INSERT INTO endereco (rua_end, numero_residencia_end, bairro_end, cidade_end, estado) " +
                    "VALUES (@rua, @numero, @bairro, @cidade, @estado)";

                query.Parameters.AddWithValue("@rua", t.Rua);
                query.Parameters.AddWithValue("@bairro", t.Bairro);
                query.Parameters.AddWithValue("@cidade", t.Cidade);
                query.Parameters.AddWithValue("@numero", t.Numero);
                query.Parameters.AddWithValue("@estado", t.Estado);

                var result = query.ExecuteNonQuery();

                if (result == 0)
                    throw new Exception("Erro ao salvar o endereço. Verifique e tente novamente.");

                return query.LastInsertedId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Endereco t)
        {
            try
            {
                var query = conn.Query();
                query.CommandText = "UPDATE endereco SET rua_end = @rua, numero_residencia_end = @numero, bairro_end = @bairro, " +
                            "cidade_end = @cidade, estado = @estado WHERE cod_end = @id ";

                query.Parameters.AddWithValue("@rua", t.Rua);
                query.Parameters.AddWithValue("@numero", t.Numero);
                query.Parameters.AddWithValue("@bairro", t.Bairro);
                query.Parameters.AddWithValue("@cidade", t.Cidade);
                query.Parameters.AddWithValue("@estado", t.Estado);

                query.Parameters.AddWithValue("@id", t.Id);

                var result = query.ExecuteNonQuery();

                if (result == 0)
                    throw new Exception("Erro ao atualizar o endereço. Verifique e tente novamente.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
