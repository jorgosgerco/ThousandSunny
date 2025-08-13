using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;

public class BountyButtonHandler : InteractionModuleBase<SocketInteractionContext>
{
    [ComponentInteraction("bounty_log_pose")]
    public async Task HandleBountyLogPoseAsync()
    {
        var embed = new EmbedBuilder()
            .WithTitle("🧭 LogPose")
            .WithDescription(
                "Mirë se erdhe në anijen tonë të Komunitetit **ShqipCinema 👒**!\n\n" +
                "🏴‍☠️ | <#1280616378486755378>\n" + // ANNOUNCEMENT
                "🏴‍☠️ | <#1147640602557685952>\n" + // RULES
                "🏴‍☠️ | <#1148977859856183388>\n" + // NEW ANIME / EPISODE
                "🏴‍☠️ | <#1136334194218389685>\n" + // MAIN CHAT
                "🏴‍☠️ | <#1156592067984760872>\n" + // MEMES
                "🏴‍☠️ | <#1149654948041212025>\n" + // SUGGESTIONS
                "_Nuk ka rëndësi destinacioni, por udhëtimi!_ 🌊"
            )
            .WithImageUrl("https://c.tenor.com/wEP-YHFtBLYAAAAC/tenor.gif") // GIF nga Tenor
            .WithColor(Color.Gold)
            .Build();

        await RespondAsync(embed: embed, ephemeral: true);
    }
}
