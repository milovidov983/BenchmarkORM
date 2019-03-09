namespace DapperBenchmark {
    using System.Data.SqlClient;
    using System.IO;
    using System;
    using Dapper;
    using Npgsql;

    public class DatabasePreparer {
        private readonly string connectionString;

        public DatabasePreparer(string connectionString) {
            this.connectionString = connectionString;
        }

        private void PrepareDatabase(Action<NpgsqlConnection> action) {
            using(var connection = new NpgsqlConnection(connectionString)) {
                try {
                    connection.Open();
                    action(connection);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                } finally {
                    connection.Close();
                }
            }
        }

        public void CreateEmptyTable() {
            Action<NpgsqlConnection> actionCreate = (connection) => {
                using(var cmd = new NpgsqlCommand()) {
                    cmd.Connection = connection;
                    cmd.CommandText = File.ReadAllText("createScript.sql");
                    cmd.ExecuteNonQuery();
                }
            };
            PrepareDatabase(actionCreate);
        }

        public void CreateFilledTable() {
            CreateEmptyTable();
            var users = Helpers.GetUsers();
            var sql = "INSERT INTO Users (Name,Age) VALUES (@Name,@Age) ";
            using(var db = new SqlConnection(connectionString)) {
                db.Open();
                var affectedRows = db.Execute(sql, users);
                db.Close();
            }

        }
    }
}