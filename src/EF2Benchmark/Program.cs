using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EF2Benchmark {
    class Program {
        private static readonly string connectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=postgre";

        static void Main(string[] args) {
            var times = InsertTest();
            foreach (var t in times) {
                Console.WriteLine($"Insert 5000 in {t} ms.");
            }
            Console.WriteLine($"Average {times.Average()} ms.");
        }

        private static long[] InsertTest(int interationCount = 10) {
            // Preparing data
            var users = GetUsers();
            var sw = new Stopwatch();
            var times = new List<long>();

            Console.WriteLine($"Start {interationCount} insertion tests...");
            for (var i = 0; i < interationCount; i++) {
                PrepareDatabase();
                using(var db = new Context()) {
                    sw.Start();
                    db.Users.AddRange(users);
                    db.SaveChanges();
                    sw.Stop();
                }
                times.Add(sw.ElapsedMilliseconds);
                sw.Reset();
            }

            Console.WriteLine("Stop insertion test.");
            return times.ToArray();
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

        private static User[] GetUsers() {
            var users = System.IO.File.ReadAllText("users.json");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<User[]>(users);
        }

        private static void PrepareDatabase() {
            using(var connection = new NpgsqlConnection(connectionString)) {
                try {
                    connection.Open();
                    CreateTable(connection);
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                } finally {
                    connection.Close();
                }
            }
        }

        private static void CreateTable(NpgsqlConnection connection) {
            using(var cmd = new NpgsqlCommand()) {
                cmd.Connection = connection;
                cmd.CommandText = System.IO.File.ReadAllText("createScript.sql");
                cmd.ExecuteNonQuery();
            }
        }
    }
}