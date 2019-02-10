using Benchmark.Interfaces;
using Benchmark.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Console.WriteLine($"Trying to fill the database.");
            if (rowCount < 1)
            {
                throw new ArgumentException($"The {nameof(rowCount)} parameter must be greater than zero.");
            }

            var entities = CreateEntities();
            var allEntities = entities.Length > rowCount
                ? entities.Take(rowCount)
                : Enumerable.Range(1, rowCount).Select(x => entities[x % entities.Length]);

            _context.FillDb(allEntities);
            Console.WriteLine($"Database filling was successful.");
        }

        private Entity[] CreateEntities()
        {
            return JsonConvert
                .DeserializeObject<Entity[]>(File.ReadAllText(@".\DataFiles\entities.json"));
        }
    }
}
