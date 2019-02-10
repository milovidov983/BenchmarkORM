using Benchmark;
using Benchmark.Interfaces;
using EF_1;
using System;

namespace Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            IFiller context = new Context();
            var helper = new Helper(context);
            helper.FillDb(5000);
        }
    }
}
