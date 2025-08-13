using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;
using ThousandSunny.Services;
using System.IO;
using ThousandSunny.Utilis;

namespace ThousandSunny.Modules
{
    public class BountyModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly WelcomeService _welcomeService;
        private readonly BountyService _bountyService;

        public BountyModule(WelcomeService welcomeService, BountyService bountyService)
        {
            _welcomeService = welcomeService;
            _bountyService = bountyService;
        }

        // --- Funksioni i unifikuar ---
        public async Task SendBountyAsync(SocketGuildUser user, string preMessage = null)
        {
            int bounty = await _bountyService.GetBerriesAsync(user.Id.ToString());

            using var imageStream = await _welcomeService.CreateBountyPosterAsync(user, bounty);
            if (imageStream == null)
            {
                if (Context != null)
                    await FollowupAsync("Diçka shkoi gabim gjatë krijimit të posterit të bounty-t.", ephemeral: true);
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle($"🎯 Marine ka vlerësuar kokën e {user.DisplayName} me një total prej {bounty} berries! 🏴‍☠️")
                .WithImageUrl("attachment://bounty.png")
                .WithColor(Color.Gold)
                .Build();


            var component = new ComponentBuilder()
                .WithButton("🍻 Dolli", $"dolli:{user.Id}", ButtonStyle.Primary)
                .WithButton("🧭 Log Pose", "bounty_log_pose", ButtonStyle.Success)
                .Build();

            // Nëse ka mesazh parazgjedhur (p.sh. Welcome message), e dërgojmë para embed-it
            if (!string.IsNullOrEmpty(preMessage))
            {
                if (Context != null)
                    await FollowupAsync(preMessage, ephemeral: true);
                else
                {
                    // Në event-in UserJoined nuk kemi Context, ndaj dërgojmë tek channel i përdoruesit
                    var defaultChannel = user.Guild.GetTextChannel(1136334194218389685); // MAIN CHAT ose çfarëdo kanali default
                    if (defaultChannel != null)
                        await defaultChannel.SendMessageAsync(preMessage);
                }
            }

            if (Context != null)
            {
                // Për slash command
                await FollowupWithFileAsync(imageStream, "bounty.png", embed: embed, components: component);
            }
            else
            {
                // Për UserJoined
                var defaultChannel = user.Guild.GetTextChannel(1136334194218389685); // MAIN CHAT
                if (defaultChannel != null)
                    await defaultChannel.SendFileAsync(imageStream, "bounty.png", embed: embed, components: component);
            }
        }

        // --- Slash command ---
        [SlashCommand("bounty", "Shfaq bounty-n e një përdoruesi.")]
        public async Task GetBountyAsync([Summary("user", "Përdoruesi për të cilin dëshironi të shihni bounty-n.")] SocketGuildUser user)
        {
            await DeferAsync();
            await SendBountyAsync(user);
        }

        // --- Funksion ndihmës për UserJoined ---
        public async Task OnUserJoinedAsync(SocketGuildUser member)
        {
            string randomMessage = WelcomeMessages.GetRandomMessage().Replace("@nickname", $"<@{member.Id}>");
            await SendBountyAsync(member, preMessage: randomMessage);
        }
    }
}
