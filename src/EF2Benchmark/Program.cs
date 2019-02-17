using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EF2Benchmark {
    class Program {
        private static readonly string connectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=postgre";

        static void Main(string[] args) {
            Prepare();
        }

        public class User {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
        public class Context : DbContext {
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

                    CreateTable(connection);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                } finally {
                    connection.Close();
                    Console.WriteLine("Connection closed.");
                }
            }
        }

        private static void CreateTable(NpgsqlConnection connection) {
            Console.WriteLine("Start create table.");
            using(var cmd = new NpgsqlCommand()) {
                cmd.Connection = connection;
                cmd.CommandText = System.IO.File.ReadAllText("createScript.sql");
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Table created successfully.");
        }
    }
}