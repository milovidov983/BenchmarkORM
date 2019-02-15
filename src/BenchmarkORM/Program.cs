using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace BenchmarkORM
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



        static Random _rand = new Random();

        static void DataCreator(string[] args)
        {
            var _strData = File.ReadAllLines(@"data.txt");

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
