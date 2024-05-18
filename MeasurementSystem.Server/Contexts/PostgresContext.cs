using MeasurementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MeasurementSystem.Server.Contexts
{
    public class PostgresContext : DbContext
    {
        public DbSet<DeviceInfo> DeviceInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CalibrationItem> CalibrationItems { get; set; }

        public PostgresContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgresDBConnection"));
        }
    }
}
