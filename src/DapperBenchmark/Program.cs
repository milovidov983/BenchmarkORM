using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperBenchmark {
    public class Result {
        public int IterationsCount { get; set; }
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
                IterationsCount = 10
            };
            try {
                results.InsertMs = InsertTest(dbPreparer.CreateEmptyTable, results.IterationsCount);
                results.UpdateMs = UpdateTest(dbPreparer.CreateFilledTable, results.IterationsCount);
                results.SelectMs = SelectTest(dbPreparer.CreateFilledTable, results.IterationsCount);

                WriteResults(results);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        private static void WriteResults(Result result) {
            if (result == null) {
                throw new ArgumentNullException("Param result must be not null.");
            }

            var sb = new StringBuilder();

            sb.Append($"Insert ms.,")
                .Append($"Update ms.,")
                .Append($"Select ms.")
                .Append(Environment.NewLine);

            Enumerable.Range(0, result.IterationsCount).Select(x => sb
                .Append($"{result.InsertMs[x]},")
                .Append($"{result.UpdateMs[x]},")
                .Append($"{result.SelectMs[x]}")
                .Append(Environment.NewLine)
            ).ToArray();

            Console.WriteLine("Results saved to result.csv");
            File.WriteAllText("results.csv", sb.ToString());
        }

        private static long[] InsertTest(Action prepareDb, int interationCount = 10) {
            var sw = new Stopwatch();
            var times = new List<long>();

            Console.WriteLine($"Start insertion test ({interationCount})...");
            for (var iteration = 0; iteration < interationCount; iteration++) {
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
                    Parallel.ForEach(db.Users, user => {
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

        private static long[] SelectTest(Action prepareDb, int iterationCount = 10) {
            prepareDb();
            var sw = new Stopwatch();
            var times = new List<long>();

            Console.WriteLine($"Start select test ({iterationCount})...");
            for (var iteration = 0; iteration < iterationCount; iteration++) {
                using(var db = new Context(connectionString)) {
                    sw.Start();
                    var users = db.Users.ToArray();
                    sw.Stop();
                    times.Add(sw.ElapsedMilliseconds);
                    sw.Reset();
                }
            }

            Console.WriteLine($"Stop select test.");
            return times.ToArray();
        }
    }
}