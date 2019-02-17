using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
            var dbPreparer = new DatabasePreparer(connectionString);
            var times = InsertTest(dbPreparer.CreateEmptyTable);

            foreach (var t in times) {
                Console.WriteLine($"Insert 5000 in {t} ms.");
            }
            Console.WriteLine($"Average {times.Average()} ms.");
        }

        private static void WriteResults(Result result) {
            if (result == null) {
                throw new ArgumentNullException("Param result must be not null.");
            }

            var sb = new StringBuilder(result.AmountEntities * 10);

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

        private static long[] InsertTest(Action prepareDb, int interationCount = 2) {
            // Preparing data
            var users = GetUsers();
            var sw = new Stopwatch();
            var times = new List<long>();

            Console.WriteLine($"Start insertion test ({interationCount})...");
            for (var i = 0; i < interationCount; i++) {
                prepareDb();
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

        private static long[] UpdateTest(Action prepareDb, int iterationCount = 10) {
            var newAges = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var sw = new Stopwatch();
            var times = new List<long>();
            prepareDb();

            Console.WriteLine($"Start update test ({interationCount})...");
            for (var i = 0; i < interationCount; i++) {
                using(var db = new Context()) {
                    sw.Start();
                    db.Users.AddRange(users);
                    db.SaveChanges();
                    sw.Stop();
                }
                times.Add(sw.ElapsedMilliseconds);
                sw.Reset();
            }
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

    }
}