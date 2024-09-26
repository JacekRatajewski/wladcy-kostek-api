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

        private ILogger _logger;

        public DbSet<News> News { get; set; }
        public DatabaseContext(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conStrBuilder = new SqlConnectionStringBuilder(_config.GetConnectionString("DB"));
            _logger.LogInformation($"DB: {_config["DbPassword"]}, OPENAI: {_config["OpenAiKey"]}");
            conStrBuilder.Password = _config["DbPassword"];
            optionsBuilder.UseSqlServer(conStrBuilder.ConnectionString);
        }
    }
}
