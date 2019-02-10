using Benchmark.Interfaces;
using Benchmark.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EF_1
{
    public class Context : DbContext, IFiller
    {
        public DbSet<Entity> Users { get; set; }

        public async Task FillDb(IEnumerable<Entity> entities)
        {
            await this.Users.AddRangeAsync(entities);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=password");
        }
    }
}
