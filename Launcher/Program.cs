using Benchmark;
using Benchmark.Interfaces;
using Benchmark.Models;
using EF_1;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            IFiller context = new Context();
            var entities = GetEntities();
            var helper = new Helper(context, entities);
            helper.FillDb(5000);
        }



        private static Entity[] GetEntities()
        {
            return JsonConvert
                .DeserializeObject<Entity[]>(File.ReadAllText($"entities.json"));
        }
    }
}
