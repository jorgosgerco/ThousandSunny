using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;
using ThousandSunny.Services;
using System.IO;

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

        [SlashCommand("bounty", "Shfaq bounty-n e një përdoruesi.")]
        public async Task GetBountyAsync(
            [Summary("user", "Përdoruesi për të cilin dëshironi të shihni bounty-n.")]
            SocketGuildUser user)
        {
            await DeferAsync();

            int bounty = await _bountyService.GetBerriesAsync(user.Id.ToString());
            string responseMessage = $"🎯 Bounty i **{user.DisplayName}** është:";

            using (var imageStream = await _welcomeService.CreateBountyPosterAsync(user, bounty))
            {
                if (imageStream != null)
                {
                    // Krijimi i butonit
                    var component = new ComponentBuilder()
                        .WithButton("🧭 Log Pose", "bounty_log_pose", ButtonStyle.Success);

                    // Dërgimi i përgjigjes me foton dhe butonin
                    await FollowupWithFileAsync(imageStream, "bounty.png", responseMessage, components: component.Build());
                }
                else
                {
                    await FollowupAsync("Diçka shkoi gabim gjatë krijimit të posterit të bounty-t.", ephemeral: true);
                }
            }
        }
    }
}