using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private static readonly User[] users = Helpers.GetUsers();

        static void Main(string[] args) {
            var dbPreparer = new DatabasePreparer(connectionString);
            var results = new Result {
                AmountEntities = users.Length
            };

            results.InsertMs = InsertTest(dbPreparer.CreateEmptyTable);
            results.UpdateMs = UpdateTest(dbPreparer.CreateFilledTable);
            
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

        private static long[] InsertTest(Action prepareDb, int interationCount = 10) {
            var sw = new Stopwatch();
            var times = new List<long>();

            Console.WriteLine($"Start insertion test ({interationCount})...");
            for (var i = 0; i < interationCount; i++) {
                prepareDb();
                using(var db = new Context(connectionString)) {
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
            // Preparing data
            prepareDb();
            var newAges = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var closedQueue = new Helpers.ClosedQueue<int>(newAges);

            var sw = new Stopwatch();
            var times = new List<long>();

            Console.WriteLine($"Start update test ({iterationCount})...");
            using(var db = new Context(connectionString)) {

                for (var iteration = 0; iteration < iterationCount; iteration++) {
                    Parallel.ForEach(db.Users, user => 
                    {
                        user.Age = closedQueue.Dequeue() + iteration;
                    });
                    sw.Start();
                    db.SaveChanges();
                    sw.Stop();
                    times.Add(sw.ElapsedMilliseconds);
                    sw.Reset();
                }
            }
            Console.WriteLine($"Stop update test.");
            return times.ToArray();
        }





    }
}