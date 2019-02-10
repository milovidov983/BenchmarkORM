using System;
using System.IO;
using System.Linq;
using Benchmark.Models;
using Newtonsoft;
using Newtonsoft.Json;

namespace DataCreator
{
    class Program
    {
        static Random _rand = new Random();

        static void Main(string[] args)
        {
            var _strData = File.ReadAllLines(@".\DataFiles\data.txt");

            var entities = Enumerable.Range(1, 100).Select(x => new Entity
            {
                IntData = _rand.Next(int.MinValue, int.MaxValue),
                DoubleData = (double)_rand.Next(int.MinValue, int.MaxValue) / _rand.Next(int.MinValue, int.MaxValue),
                StringData = _strData[_rand.Next(0, _strData.Length - 1)]
            });

            File.WriteAllText("entities.json", JsonConvert.SerializeObject(entities));
        }

    }
}
