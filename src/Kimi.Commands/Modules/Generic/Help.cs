using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Milkshake;
using Milkshake.Models;

namespace Kimi.Commands.Modules.Generic
{
    public class Help : InteractionModuleBase<SocketInteractionContext>
    {
        private MilkshakeService _milkshake = new MilkshakeService();

        [SlashCommand("help", "Milkshake comes in clutch!")]
        public async Task HandleHelpCommand()
        {
            await RespondAsync(embed: await HelpEmbed());
        }

        private static async Task<Embed> HelpEmbed()
        {
            var author = new EmbedAuthorBuilder()
                .WithName("Kimi comes in clutch!")
                .WithIconUrl("https://cdn.discordapp.com/emojis/783328274193448981.webp");

            var embed = new EmbedBuilder()
                .WithColor(0xf1c3c7)
                .WithAuthor(author)
                .WithDescription("For more specific information, type `/help <command>`\nFurther questions? Ping `netty#0725`")
                .AddField(info =>
                {
                    info.WithIsInline(true);
                    info.WithName("INFO");
                    info.WithValue("`help` *this embed*\n" +
                                   "`info` *info about the bot*\n" +
                                   "`ping` *gets the latency*");
                })
                .AddField(monark =>
                {
                    monark.WithIsInline(true);
                    monark.WithName("MONARK");
                    monark.WithValue("`generate`\n" +
                                "`force`\n" +
                                "`count`");
                })
                .Build();

            await Task.CompletedTask;
            return embed;
        }

        [SlashCommand("magicktest", "Test")]
        public async Task Test(string a)
        {
            _milkshake.Log += Kimi.Logging.Log.Write;
            var test = new MagickTest(_milkshake);
            await DeferAsync();
            await test.Test(a);
            await FollowupWithFileAsync(@"C:\Users\Netty\Desktop\test.png");
        }

        [SlashCommand("magicktesttwo", "Test")]
        public async Task Test2(string a, ImageTags tag, ImageTags? tag2 = null, ImageTags? tag3 = null)
        {
            _milkshake.Log += Kimi.Logging.Log.Write;
            var test = new MagickTest(_milkshake);
            await DeferAsync();
            await test.Test(a);
            await FollowupWithFileAsync(@"C:\Users\Netty\Desktop\test.png");
        }
    }
}
