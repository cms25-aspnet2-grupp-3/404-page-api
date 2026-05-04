using Microsoft.EntityFrameworkCore;
using MonitoringServiceApi.Models;

namespace MonitoringServiceApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<ErrorLog> ErrorLogs { get; set; }
    }
}