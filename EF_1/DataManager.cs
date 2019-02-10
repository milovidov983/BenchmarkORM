using Benchmark.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EF_1
{
    public class DataManager : IOrm
    {
        public Task InsertAsync(int _maxRows)
        {
            throw new NotImplementedException();
        }

        public Task SelectAsync(int _maxRows)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int _maxRows)
        {
            throw new NotImplementedException();
        }
    }
}
