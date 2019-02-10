using Benchmark.Interfaces;
using Benchmark.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Benchmark
{
    public class Helper
    {
        private readonly IFiller _context;

        public Helper(IFiller _context)
        {
            this._context = _context;
        }

        public void FillDb(int rowCount)
        {
            var entities = CreateEntities(rowCount);

        }

        private IEnumerable<Entity> CreateEntities(int rowCount)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Entity>>(File.ReadAllText(@".\DataFiles\entities.json"));
        }
    }
}
