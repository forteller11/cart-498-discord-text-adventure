using Discord;
using Discord.WebSocket;
using DiscordTextAdventure.Discord.Rendering;
using DiscordTextAdventure.Parsing;

#nullable enable
namespace chext.Mechanics
{
    public class Session
    {
        private DiscordSocketClient _client;
        public readonly SocketGuild Guild;
        private EmbededDrawer _drawer;
        private Input _input;
        
        public SocketGuildUser Player;
        public Session(DiscordSocketClient client, SocketGuild guild)
        {
            _client = client;
            Guild = guild;
            
            _input = new Input(_client, Guild);
        }
    }
}