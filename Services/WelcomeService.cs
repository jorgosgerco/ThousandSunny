using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SkiaSharp;
using ThousandSunny.Utilis;
using System.Linq;

namespace ThousandSunny.Services
{
    public class WelcomeService : IDisposable
    {
        private readonly SKTypeface _playfairFont;
        private readonly SKTypeface _alwaysInMyHeartFont;
        private readonly string _assetsPath;
        private readonly HttpClient _httpClient;
        private readonly Random _random;

        public WelcomeService()
        {
            // Now that the working directory is set to the project root,
            // we can simply point to the "assets" folder.
            _assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");

            _playfairFont = SKTypeface.FromFile(Path.Combine(_assetsPath, "PlayfairDisplay-Bold.ttf"));
            _alwaysInMyHeartFont = SKTypeface.FromFile(Path.Combine(_assetsPath, "AlwaysInMyHeart.ttf"));
            _httpClient = new HttpClient();
            _random = new Random();
        }

        public async Task<MemoryStream> CreateBountyPosterAsync(SocketGuildUser member, int bountyValue)
        {
            return await GenerateBountyPosterAsync(member, bountyValue);
        }

        public async Task SendWelcomeBountyAsync(SocketGuildUser member, int bountyValue, string messageContent)
        {
            if (member == null)
            {
                Console.WriteLine("Error: member is null in SendWelcomeBountyAsync.");
                return;
            }

            var imageStream = await GenerateBountyPosterAsync(member, bountyValue);

            if (imageStream != null)
            {
                var channel = member.Guild.GetTextChannel(1136334194218389685);
                if (channel != null)
                {
                    var component = new ComponentBuilder()
                        .WithButton("🧭 Log Pose", "welcome_log_pose", ButtonStyle.Success);

                    await channel.SendFileAsync(imageStream, "bounty.png", messageContent, components: component.Build());
                }
                else
                {
                    Console.WriteLine("Welcome channel not found.");
                }
            }
        }

        private async Task<MemoryStream> GenerateBountyPosterAsync(SocketGuildUser member, int bountyValue)
        {
            if (member == null) return null;

            return await Task.Run(async () =>
            {
                using var backgroundImageStream = File.OpenRead(Path.Combine(_assetsPath, "wanted-bosh.png"));
                using var backgroundImage = SKBitmap.Decode(backgroundImageStream);
                using var surface = SKSurface.Create(new SKImageInfo(backgroundImage.Width, backgroundImage.Height));
                var canvas = surface.Canvas;

                canvas.DrawBitmap(backgroundImage, 0, 0);

                var avatarUrl = member.GetAvatarUrl(size: 1024) ?? member.GetDefaultAvatarUrl();
                if (string.IsNullOrEmpty(avatarUrl))
                    avatarUrl = "https://cdn.discordapp.com/embed/avatars/0.png";

                byte[] avatarBytes;
                try { avatarBytes = await _httpClient.GetByteArrayAsync(avatarUrl); }
                catch (Exception ex) { Console.WriteLine($"Error downloading avatar image: {ex.Message}"); return null; }

                using var avatarStream = new MemoryStream(avatarBytes);
                using var avatarImage = SKBitmap.Decode(avatarStream);

                var destRect = new SKRect(82, 268, 82 + 712, 268 + 515);
                var avatarRatio = (float)avatarImage.Width / avatarImage.Height;
                var destRatio = destRect.Width / destRect.Height;

                SKRect srcRect;
                if (avatarRatio > destRatio)
                {
                    var srcHeight = avatarImage.Height;
                    var srcWidth = (int)(srcHeight * destRatio);
                    var srcX = (avatarImage.Width - srcWidth) / 2;
                    srcRect = new SKRect(srcX, 0, srcX + srcWidth, srcHeight);
                }
                else
                {
                    var srcWidth = avatarImage.Width;
                    var srcHeight = (int)(srcWidth / destRatio);
                    var srcY = (avatarImage.Height - srcHeight) / 2;
                    srcRect = new SKRect(0, srcY, srcWidth, srcY + srcHeight);
                }

                canvas.DrawBitmap(avatarImage, srcRect, destRect);
                var displayName = member.Nickname ?? member.Username;
                if (string.IsNullOrEmpty(displayName))
                    displayName = member.Guild.Name;

                displayName = CleanUsername(displayName).ToUpper();
                if (displayName.Length > 10)
                    displayName = displayName.Substring(0, 10);

                using var playfairPaint = new SKPaint
                {
                    Color = new SKColor(0x41, 0x2f, 0x17),
                    TextSize = 105,
                    Typeface = _playfairFont,
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center
                };

                using var alwaysInMyHeartPaint = new SKPaint
                {
                    Color = new SKColor(0x41, 0x2f, 0x17),
                    TextSize = 100,
                    Typeface = _alwaysInMyHeartFont,
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center
                };

                canvas.DrawText(displayName, 440, 990f, playfairPaint);
                canvas.DrawText(bountyValue.ToString("N0"), 425, 1120, alwaysInMyHeartPaint);

                var outputStream = new MemoryStream();
                surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100).SaveTo(outputStream);
                outputStream.Position = 0;

                return outputStream;
            });
        }

        private string CleanUsername(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            var normalized = input.Normalize(System.Text.NormalizationForm.FormKD);
            var resultChars = Array.FindAll(normalized.ToCharArray(), c =>
                char.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark &&
                (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)));

            return new string(resultChars).Trim();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}