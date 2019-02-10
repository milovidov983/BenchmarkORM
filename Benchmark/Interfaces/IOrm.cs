using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Interfaces
{
    public interface IOrm
    {
        Task InsertAsync(int _maxRows);
        Task UpdateAsync(int _maxRows);
        Task SelectAsync(int _maxRows);
    }
}