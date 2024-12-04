using Microsoft.EntityFrameworkCore;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Repo.Context;
using WladcyKostek.Repo.Entities;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System.Text.Json;
using OpenAI_API.Images;

namespace WladcyKostek.Repo.Repository
{
    internal class BonusesRepository : IBonusesRepository
    {
        private DatabaseContext _database;
        private OpenAIAPI _http;
        private string? _userCount;
        private string? _city;

        public BonusesRepository(DatabaseContext database, IConfiguration config, OpenAIAPI openAIAPI)
        {
            _database = database;
        }

        public async Task<BonusesDTO> GetBonusesAsync(int id)
        {
            var bonus = await _database.Bonuses.FirstOrDefaultAsync(x => x.Id == id && x.IsPublic == true);
            if(bonus is null)
            {
                throw new Exception("User not added or not public!");
            }
            return new BonusesDTO
            {
                Id = id,
                Name = bonus.Name,
                BonusCount = bonus.BonusCount,
                MoneySupported = bonus.MoneySupported,
                SessionCount = bonus.SessionCount,
                PlayerSeasonStart = bonus.PlayerSeasonStart
            };
        }

        public async Task<int?> GetBonusIdAsync(string name)
        {
            var bonus = await _database.Bonuses.Where(b => EF.Functions.Collate(b.Name, "SQL_Latin1_General_CP1_CI_AI") == name).FirstOrDefaultAsync();
            if (bonus?.IsPublic == false)
            {
                throw new Exception("User not public!");
            }
            return bonus?.Id;
        }
    }
}
