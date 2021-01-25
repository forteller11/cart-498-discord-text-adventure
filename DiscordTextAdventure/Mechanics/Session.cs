using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Responses;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable
namespace chext.Mechanics
{
    public class Session
    {
        private DiscordSocketClient _client;
        public readonly SocketGuild Guild;
   
        private Input _input;
        private RoomManager _roomsManager;
        private ResponseManager _responseManager;
        
        public Player? Player;
        public Session(DiscordSocketClient client, SocketGuild guild)
        {
            _client = client;
            Guild = guild;
            
            _input = new Input();
            _responseManager = new ResponseManager();
            _roomsManager = new RoomManager(_client, guild);
            //Player = new Player();

            client.MessageReceived += OnMessageReceived;
        }

        async Task OnMessageReceived(SocketMessage socketMessage)
        {
            Phrase? phrase = _input.ProcessMessageForThisSession(socketMessage, _client, Guild);
            if (phrase != null)
            {
                Program.DebugLog("phrase relevant");
                _responseManager.CallResponseFromPhrase(phrase!, Player);
            }
        }
        
        
    }
}