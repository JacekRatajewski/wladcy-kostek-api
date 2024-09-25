using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WładcyKostek.Repo.Entities;

namespace WładcyKostek.Repo.Context
{
    public class DatabaseContext : DbContext
    {
        public IConfiguration _config { get; set; }
        public DbSet<News> News { get; set; }
        public DatabaseContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conStrBuilder = new SqlConnectionStringBuilder(_config.GetConnectionString("DB"));
            conStrBuilder.Password = _config["DB_PASSWORD"];
            optionsBuilder.UseSqlServer(conStrBuilder.ConnectionString);
        }
    }
}
