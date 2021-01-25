using Discord;
using Discord.WebSocket;
using DiscordTextAdventure.Discord.Rendering;
using DiscordTextAdventure.Mechanics.Player;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Parsing;

#nullable enable
namespace chext.Mechanics
{
    public class Session
    {
        private DiscordSocketClient _client;
        public readonly SocketGuild Guild;
   
        private Input _input;
        private RoomManager _roomsManager;
        
        public Player Player;
        public Session(DiscordSocketClient client, SocketGuild guild)
        {
            _client = client;
            Guild = guild;
            
            _input = new Input(_client, Guild);
            _roomsManager = new RoomManager(_client, guild);
            Player = new Player();
        }
    }
}