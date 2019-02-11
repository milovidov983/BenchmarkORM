using Benchmark.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Interfaces
{
    public interface IOrm
    {
        Task InsertAsync(Entity[] entities);
        Task UpdateAsync(Entity[] entities);
        void Select();
    }
}