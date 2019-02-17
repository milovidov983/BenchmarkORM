using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EF2Benchmark {
    class Program {
        private static readonly string connectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=postgre";

        static void Main(string[] args) {

        }

        class User {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
        class Context : DbContext {
            public DbSet<User> Users { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        private User[] GetUsers() {
            var users = System.IO.File.ReadAllText("users.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<User[]>(users);
        }

        private static void Prepare() {
            Console.WriteLine("Start preparing database.");
            Console.WriteLine("Try connect to database...");
            using(var connection = new NpgsqlConnection(connectionString)) {
                try {
                    connection.Open();
                    Console.WriteLine("Connection opened.");

                    DeleteTable(connection);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                } finally {
                    connection.Close();
                    Console.WriteLine("Connection closed.");
                }
            }

            // var connString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";

            // using (var conn = new NpgsqlConnection(connString))
            // {
            //     conn.Open();

            //     // Insert some data
            //     using (var cmd = new NpgsqlCommand())
            //     {
            //         cmd.Connection = conn;
            //         cmd.CommandText = "INSERT INTO data (some_field) VALUES (@p)";
            //         cmd.Parameters.AddWithValue("p", "Hello world");
            //         cmd.ExecuteNonQuery();
            //     }

            //     // Retrieve all rows
            //     using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
            //     using (var reader = cmd.ExecuteReader())
            //         while (reader.Read())
            //             Console.WriteLine(reader.GetString(0));
            // }
        }

        private static void DeleteTable(NpgsqlConnection connection) {
            Console.WriteLine("Drop old table.");
            using(var cmd = new NpgsqlCommand()) {
                cmd.Connection = connection;
                cmd.CommandText = "DROP TABLE IF EXISTS public.\"Users\"";
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Table droped successfully.");
        }
    }
}