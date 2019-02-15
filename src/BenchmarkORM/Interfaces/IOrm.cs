using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkORM
{
    public interface IOrm
    {
        Task InsertAsync(Entity[] entities);
        Task UpdateAsync();
        void Select();
    }
}