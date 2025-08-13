using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ThousandSunny.Modules
{
    public class AutoRoleMonitor
    {
        private readonly DiscordSocketClient _client;
        private const ulong UNVERIFIED_ROLE_ID = 1403861136109076681;

        public AutoRoleMonitor(DiscordSocketClient client)
        {
            _client = client;
            _client.GuildMemberUpdated += OnGuildMemberUpdated;
        }

        private async Task OnGuildMemberUpdated(Cacheable<SocketGuildUser, ulong> beforeCache, SocketGuildUser after)
        {
            // Log that the event was triggered, for debugging purposes.
            Console.WriteLine($"[AutoRoleMonitor] GuildMemberUpdated event triggered for user: {after.Username}");

            // Try to get the user's state from before the update.
            var before = beforeCache.HasValue ? beforeCache.Value : null;

            // If we don't have the previous state, we can't compare roles.
            if (before == null)
            {
                Console.WriteLine($"[AutoRoleMonitor] 'Before' state not available for {after.Username}. Cannot compare roles.");
                return;
            }

            // Check if the user gained a new role.
            if (after.Roles.Count > before.Roles.Count)
            {
                // Get the unverified role by its ID.
                var unverifiedRole = before.Guild.GetRole(UNVERIFIED_ROLE_ID);

                // Check if the user had the unverified role before the update.
                if (before.Roles.Contains(unverifiedRole))
                {
                    try
                    {
                        // Add a 3-second delay here before removing the role.
                        Console.WriteLine($"[AutoRoleMonitor] Waiting 3 seconds before removing unverified role from {after.Username}.");
                        await Task.Delay(3000);

                        // Remove the unverified role.
                        await after.RemoveRoleAsync(unverifiedRole);
                        Console.WriteLine($"[AutoRoleMonitor] Removed unverified role from {after.Username} after they gained a new role.");
                    }
                    catch (Exception ex)
                    {
                        // Log any errors if the bot fails to remove the role.
                        Console.WriteLine($"[AutoRoleMonitor] Failed to remove unverified role from {after.Username}: {ex.Message}");
                    }
                }
            }
        }
    }
}