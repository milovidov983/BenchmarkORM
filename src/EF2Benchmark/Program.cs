using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EF2Benchmark {
    public class Result {
        public int AmountEntities { get; set; }
        public long[] InsertMs { get; set; }
        public long[] UpdateMs { get; set; }
        public long[] SelectMs { get; set; }
    }
    class Program {
        private static readonly string connectionString = "Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=postgre";

        static void Main(string[] args) {
            var times = InsertTest();
            foreach (var t in times) {
                Console.WriteLine($"Insert 5000 in {t} ms.");
            }
            Console.WriteLine($"Average {times.Average()} ms.");
        }

        private static void WriteResults(Result result) {
            if (result == null) {
                throw new ArgumentNullException("Param result must be not null.");
            }

            var sb = new StringBuilder(result.AmountEntities * 10 ?? 10000);

            sb.Append($"Insert {result.AmountEntities} rows/ms.\t")
                .Append($"Update {result.AmountEntities} rows/ms.\t")
                .Append($"Select {result.AmountEntities} rows/ms.")
                .Append(Environment.NewLine);

            Enumerable.Range(0, result.AmountEntities).Select(x => sb
                .Append($"{result.InsertMs[x]}\t")
                .Append($"{result.UpdateMs[x]}\t")
                .Append($"{result.SelectMs[x]}\t")
                .Append(Environment.NewLine)
            );

            File.WriteAllText("results.csv", sb.ToString());
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
            var users = File.ReadAllText("users.json");
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
                cmd.CommandText = File.ReadAllText("createScript.sql");
                cmd.ExecuteNonQuery();
            }
        }
    }
}