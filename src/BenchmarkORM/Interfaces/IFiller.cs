using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkORM
{
    public interface IFiller
    {
        Task FillDb(IEnumerable<Entity> entities);
    }
}
