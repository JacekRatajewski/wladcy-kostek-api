using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WladcyKostek.Repo.Entities;

namespace WladcyKostek.Repo.Context
{
    public class DatabaseContext : DbContext
    {
        public IConfiguration _config { get; set; }

        public DbSet<News> News { get; set; }
        public DbSet<Bonuses> Bonuses { get; set; }
        public DbSet<ScrappedNews> ScrappedNews { get; set; }
        public DbSet<User> Users { get; set; }
        public DatabaseContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conStrBuilder = new SqlConnectionStringBuilder(_config.GetConnectionString("DB"));
            conStrBuilder.Password = _config["DbPassword"];
            optionsBuilder.UseSqlServer(conStrBuilder.ConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);
            });
        }
    }
}
