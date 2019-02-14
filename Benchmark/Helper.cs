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
        private readonly Entity[] _entities;

        public Helper(IFiller context, Entity[] entities)
        {
            this._context = context;
            this._entities = entities;
        }

        public void FillDb(int rowCount)
        {
            Console.WriteLine($"Trying to fill the database.");
            if (rowCount < 1)
            {
                throw new ArgumentException($"The {nameof(rowCount)} parameter must be greater than zero.");
            }

            _context.FillDb(_entities);
            
            Console.WriteLine($"Database filling was successful.");
        }

    }
}
