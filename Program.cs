using System;
using System.IO;
using dotenv.net;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ThousandSunny.Services;
using ThousandSunny.Utilis;
using ThousandSunny.Modules;

namespace ThousandSunny
{
    class Program
    {
        private DiscordSocketClient _client;
        private InteractionService _interactionService;
        private IServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {
            var program = new Program();
            await program.MainAsync();
        }

        public async Task MainAsync()
        {
            string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            Directory.SetCurrentDirectory(projectRoot);

            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.Guilds |
                                 GatewayIntents.GuildMembers |
                                 GatewayIntents.GuildMessages |
                                 GatewayIntents.MessageContent |
                                 GatewayIntents.GuildMessageReactions
            };

            _client = new DiscordSocketClient(config);

            _serviceProvider = ConfigureServices();
            _interactionService = _serviceProvider.GetRequiredService<InteractionService>();

            _client.Log += LogAsync;

            _client.Ready += OnReadyAsync;
            _client.InteractionCreated += OnInteractionCreatedAsync;

            // --- ADD THE NEW LINE HERE ---
            // This gets the service from the container, which runs the constructor
            // and subscribes to the event.
            _serviceProvider.GetRequiredService<AutoRoleMonitor>();
            // -----------------------------


            _client.UserJoined += async member =>
            {
                var bountyModule = _serviceProvider.GetRequiredService<BountyModule>();
                await bountyModule.OnUserJoinedAsync(member);
            };

            // Rregulli i ri: I jep berries çdo 30 sekonda
            _client.MessageReceived += async message =>
            {
                if (message.Author.IsBot) return;

                var bountyService = _serviceProvider.GetRequiredService<BountyService>();
                var userId = message.Author.Id.ToString();

                var lastMessageTime = await bountyService.GetLastMessageTimeAsync(userId);
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // Kontrollon nëse kanë kaluar 30 sekonda
                if (currentTime - lastMessageTime > 30000)
                {
                    await bountyService.AddBerriesAsync(userId, 10);
                    await bountyService.SetLastMessageTimeAsync(userId, currentTime);
                }
            };


            // Set the new current directory
            Directory.SetCurrentDirectory(projectRoot);

            // Load .env from the root folder
            DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { ".env" }));

            // Read token
            string token = Environment.GetEnvironmentVariable("TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Tokeni nuk u gjet në variablat e mjedisit.");
                return;
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task OnReadyAsync()
        {
            Console.WriteLine($"🤖 Sunny u lidh si {_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator}");

            // Add your modules
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

            // Register commands to a specific guild for instant updates during development
            // This is much faster and prevents commands from getting stuck.
            await _interactionService.RegisterCommandsToGuildAsync(1136333653576781904);

            // Optional: Print a message to confirm commands were registered
            Console.WriteLine("Slash commands have been registered.");
        }


        private async Task OnInteractionCreatedAsync(SocketInteraction interaction)
        {
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<BountyService>()
                .AddSingleton<WelcomeService>()
                .AddSingleton<AutoRoleMonitor>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>().Rest))
                .AddSingleton<BountyModule>()
                .BuildServiceProvider();
        }

        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}