using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ThousandSunny.Services
{
    public class BountyService
    {
        private readonly string _filePath;

        public BountyService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "berries.json");
        }

        private async Task<Dictionary<string, int>> GetBerriesDataAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    await SaveBerriesDataAsync(new Dictionary<string, int>());
                    return new Dictionary<string, int>();
                }

                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim gjatë leximit të berries.json: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }

        private async Task SaveBerriesDataAsync(Dictionary<string, int> data)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(data, options);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim gjatë shkrimit në berries.json: {ex.Message}");
            }
        }

        // Kthen vlerën aktuale të bounty-t
        public async Task<int> GetBerriesAsync(string userId)
        {
            var data = await GetBerriesDataAsync();
            if (data.TryGetValue(userId, out int berries))
            {
                return berries;
            }
            return 0;
        }

        // Kthen vlerën aktuale të lastMessageTime
        public async Task<long> GetLastMessageTimeAsync(string userId)
        {
            var data = await GetBerriesDataAsync();
            var key = $"lastMessage_{userId}";
            if (data.TryGetValue(key, out int lastMessageTime))
            {
                return (long)lastMessageTime;
            }
            return 0;
        }

        // Shton një sasi bounty-sh në vlerën ekzistuese
        public async Task<int> AddBerriesAsync(string userId, int amount)
        {
            var data = await GetBerriesDataAsync();
            var currentBerries = await GetBerriesAsync(userId);
            var newBerries = currentBerries + amount;
            data[userId] = newBerries;
            await SaveBerriesDataAsync(data);
            return newBerries;
        }

        // Vendos një vlerë të caktuar bounty-sh
        public async Task SetBerriesAsync(string userId, int amount)
        {
            var data = await GetBerriesDataAsync();
            data[userId] = amount;
            await SaveBerriesDataAsync(data);
        }

        // Vendos vlerën e lastMessageTime
        public async Task SetLastMessageTimeAsync(string userId, long time)
        {
            var data = await GetBerriesDataAsync();
            var key = $"lastMessage_{userId}";
            data[key] = (int)time;
            await SaveBerriesDataAsync(data);
        }
    }
}