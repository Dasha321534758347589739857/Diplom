using Diplom.Data.DB;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Diplom.Data
{
    public class Context : DbContext
    {
        public string Test { get; set; } = "";
        public DbSet<Air> Airs { get; set; } = null!;
        //public DbSet<CalculationModel> CalculationModels { get; set; } = null!;
        public DbSet<EmergencySituation> EmergencySituations { get; set; } = null!;
        public DbSet<EmergencySituationAtTheFacility> EmergencySituationAtTheFacilities { get; set; } = null!;
        public DbSet<Fabric> Materials { get; set; } = null!;
        public DbSet<Objectt> Objectts { get; set; } = null!;
        public DbSet<Priming> Primings { get; set; } = null!;
        public DbSet<Glossary> Glossaries { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Substance> Substances { get; set; } = null!;
        public DbSet<SubstanceInAnObject> SubstanceInAnObjects { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Watcher> Watchers { get; set; } = null!;
        public DbSet<GrafDate> Point { get; set; } = null!;



        public Context()
        {
            Database.EnsureCreated();//создание бд
        }

        public Context(string test)
        {
            Test = test;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)//настройка бд: параметры подключения
        {
            if (Test != "")
                optionsBuilder.UseSqlite($"Data Source={Test}");
            else
            {
                optionsBuilder.UseSqlite($"Data Source={DBConfig.Destination}");

            }
        }
    }
}
