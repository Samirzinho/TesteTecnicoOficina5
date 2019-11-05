using System;
using MySql.Data.MySqlClient;

namespace locadora.banco
{
    public class AppDb : IDisposable
    {
        public MySqlConnection Connection;

        public AppDb(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Port=3306;Database=cadastro;Uid=root;Pwd=tom2003b;";
            }
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}