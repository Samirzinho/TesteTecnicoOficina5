using System;
using locadora.banco;
using locadora.models;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace locadora.Models
{
    public class VeiculoQuery
    {
        public readonly AppDb Db;
        public VeiculoQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Veiculo>> ListaVeiculoAsync()
        {
            var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT placa, marca, ano, descricao, vendido, created, updated FROM veiculos ORDER BY ano DESC ;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Veiculo> InsereNovoVeiculoAsync(locadora.models.Veiculo veiculonovo)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO veiculos " +
                               "(placa, " +
                               "marca, " +
                               "ano, " +
                               "descricao, " +
                               "vendido, " +
                               "created, " +
                               "updated) " +
                               "VALUES " +
                               "(@placa, " +
                               "@marca, " +
                               "@ano, " +
                               "@descricao, " +
                               "@vendido, " +
                               "@created, " +
                               "@updated)";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@placa",
                DbType = DbType.String,
                Value = veiculonovo.Placa,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@marca",
                DbType = DbType.String,
                Value = veiculonovo.Marca,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ano",
                DbType = DbType.Int32,
                Value = veiculonovo.Ano,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@descricao",
                DbType = DbType.String,
                Value = veiculonovo.Descricao,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@vendido",
                DbType = DbType.Boolean,
                Value = veiculonovo.Vendido,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@created",
                DbType = DbType.DateTime,
                Value = DateTime.Today,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@updated",
                DbType = DbType.DateTime,
                Value = DateTime.Today,
            });

            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Veiculo> AlteraVeiculoAsync(locadora.models.Veiculo veiculonovo)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE veiculos SET " +
                               "marca = @marca, " +
                               "ano = @ano, " +
                               "descricao = @descricao, " +
                               "vendido = @vendido, " +
                               "updated = @updated " +
                               "WHERE " +
                               "placa = @placa ";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@placa",
                DbType = DbType.String,
                Value = veiculonovo.Placa,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@marca",
                DbType = DbType.String,
                Value = veiculonovo.Marca,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ano",
                DbType = DbType.Int32,
                Value = veiculonovo.Ano,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@descricao",
                DbType = DbType.String,
                Value = veiculonovo.Descricao,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@vendido",
                DbType = DbType.Boolean,
                Value = veiculonovo.Vendido,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@updated",
                DbType = DbType.DateTime,
                Value = DateTime.Today,
            });

            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Veiculo> BuscaVeiculoIDAsync(string placa)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT placa, marca, ano, descricao, vendido, created, updated FROM veiculos WHERE placa = @placa";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@placa",
                DbType = DbType.String,
                Value = placa,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<Veiculo> DeletaVeiculoAsync(string placa)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM veiculos WHERE placa = @placa";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@placa",
                DbType = DbType.String,
                Value = placa,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        private async Task<List<Veiculo>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Veiculo>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Veiculo(Db)
                    {
                        Placa = await reader.GetFieldValueAsync<string>(0),
                        Marca = await reader.GetFieldValueAsync<string>(1),
                        Ano = await reader.GetFieldValueAsync<int>(2),
                        Descricao = await reader.GetFieldValueAsync<string>(3),
                        Vendido = await reader.GetFieldValueAsync<bool>(4),
                        Created = await reader.GetFieldValueAsync<DateTime>(5),
                        Updated = await reader.GetFieldValueAsync<DateTime>(6)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
