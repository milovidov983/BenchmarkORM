
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkORM
{
    public class DataManager : IOrm
    {
        private readonly Context _context;

        public DataManager(Context _context)
        {
            this._context = _context;
        }

        public async Task InsertAsync(Entity[] entities)
        {
            await _context.Entities.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public void Select()
        {
            var result = _context.Entities.Select(x => x).ToArray();
        }

        public async Task UpdateAsync()
        {
            Parallel.ForEach(_context.Entities, (entity) =>
            {
                entity.IntData = 1;
                entity.DoubleData = 0.1;
                entity.StringData = "Updated";
            });

            await _context.SaveChangesAsync();

        }
    }
}
