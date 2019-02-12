using Benchmark.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Benchmark
{
    public class BenchmarkImpl
    {
        private readonly IOrm _context;
        private readonly int _maxRows;

        public BenchmarkImpl(IOrm _context, int _maxRows)
        {
            this._context = _context;
            this._maxRows = _maxRows;
        }

        public async Task<TimeSpan> StartInsertBemchmark()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await _context.InsertAsync(Helper.CreateEntities(_maxRows));

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
