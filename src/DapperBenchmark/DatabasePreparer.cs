namespace DapperBenchmark {
    using System.IO;
    using System;
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
            using(var db = new Context(connectionString)) {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

        }
    }
}