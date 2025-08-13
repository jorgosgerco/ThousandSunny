using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThousandSunny.Modules
{
    public class DolliModule : InteractionModuleBase<SocketInteractionContext>
    {
        private static readonly Random _random = new Random();

        // Lista e mesazheve të mundshme për Dolli
        private static readonly List<string> DolliMessages = new List<string>
        {
    "{0} e fton {1} në Grand Line për një dolli 🍻!",
    "{0} ngre dolli me {1} duke thënë: 'Për aventurën tonë!'",
    "{0} dhe {1} shtrëngojnë gotat në Sunny 🍺",
    "{0} fton {1} në një toast për 'One Piece Forever!'",
    "{0} ngre dolli dhe thotë: 'Le të shkojmë drejt Skarlet Line!' për {1}",
    "{0} dhe {1} shijojnë një birrë në Thousand Sunny 🍻",
    "{0} thotë: 'Le të festojmë fitoren!' dhe ngre dolli me {1}",
    "{0} ngre gotën për {1} dhe thotë: 'Për Luffy dhe ekuipazhin!'",
    "{0} fton {1} në një dolli nën Shenjën e Jolly Roger 🏴‍☠️",
    "{0} thotë: 'Për të gjetur One Piece!' dhe ngre dolli me {1}",
    "{0} dhe {1} bëjnë një toast duke parë oqeanin e Grand Line 🌊",
    "{0} e thërret {1}: 'Ej, bashkohuni me mua për një birrë në Sunny!' 🍺",
    "{0} ngre dolli me {1} dhe thotë: 'Të gjitha aventurat na presin!'",
    "{0} fton {1} në një toast për Shanks dhe ekuipazhin e tij 🏴‍☠️",
    "{0} dhe {1} bëjnë dolli për Skypiea dhe qiellin e pafund 🌤️",
    "{0} thotë: 'Për thesarët dhe Devil Fruits!' dhe ngre dolli me {1}",
    "{0} dhe {1} ngrejnë gotat në Thousand Sunny duke qeshur 😄",
    "{0} fton {1} në një toast për të kapur të gjitha Devil Fruits 🍇",
    "{0} thotë: 'Për vullnetin e D!' dhe ngre dolli me {1}",
    "{0} ngre dolli me {1} duke thënë: 'Për ShqipCinema dhe aventurat anime! 🎬🍻'"
        };

        [ComponentInteraction("dolli:*")] // * përdoret për të marrë parametra
        public async Task OnDolliClickedAsync(string bountyIdString)
        {
            var clicker = Context.User as SocketGuildUser;

            // Merr personin e bounty nga ID
            SocketGuildUser bountyUser = null;
            if (ulong.TryParse(bountyIdString, out ulong bountyId))
            {
                bountyUser = Context.Guild.GetUser(bountyId);
            }

            if (bountyUser == null)
            {
                await RespondAsync("Nuk u gjet personi për të ngre dolli 😅", ephemeral: true);
                return;
            }

            // Kontroll për të mos e lejuar vetë të ngrejë dolli për vete
            if (clicker.Id == bountyUser.Id)
            {
                await RespondAsync("Dikush tjetër duhet të ngre dolli për ty 🍺", ephemeral: true);
                return;
            }

            // Gjenero mesazh random
            string template = DolliMessages[_random.Next(DolliMessages.Count)];
            string message = string.Format(template, clicker.Mention, bountyUser.Mention);

            // Dërgo mesazhin
            await RespondAsync(message);
        }
    }
}
