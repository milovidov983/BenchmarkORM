using Benchmark.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Interfaces
{
    public interface IFiller
    {
        Task FillDb(IEnumerable<Entity> entities);
    }
}
