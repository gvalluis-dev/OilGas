using OilGas.Models;
using Microsoft.EntityFrameworkCore;

namespace OilGas.Data
{
    public class SensorContext : DbContext
    {
        public DbSet<SensorData> SensorData { get; set; }

        public SensorContext(DbContextOptions<SensorContext> options) : base(options) { }

    }

}
