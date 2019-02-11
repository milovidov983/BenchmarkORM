using Benchmark.Interfaces;
using Benchmark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_1
{
    public class DataManager : IOrm
    {
        private readonly Context _context;

        public DataManager(Context _context)
        {
            _context = _context;
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

        public Task UpdateAsync(Entity[] entities)
        {
            
        }
    }
}
