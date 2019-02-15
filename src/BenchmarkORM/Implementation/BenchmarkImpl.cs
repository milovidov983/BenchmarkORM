using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BenchmarkORM
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

        public async Task<TimeSpan> StartInsertBemchmark(Entity[] entities)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await _context.InsertAsync(entities);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
