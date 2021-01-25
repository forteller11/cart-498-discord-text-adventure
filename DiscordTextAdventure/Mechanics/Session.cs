using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
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
        
        private List<PhraseResponse> _phraseResponses;
        private List<ReactionResponse> _reactionResponses;
        
        public Player? Player;
        public Session(DiscordSocketClient client, SocketGuild guild)
        {
            _client = client;
            Guild = guild;
            
            _input = new Input();
            _phraseResponses = ResponseTable.GetStaticPhraseResponseList();
            _reactionResponses = ResponseTable.GetStaticReactionResponseList();
            _roomsManager = new RoomManager(_client, guild);
            //Player = new Player();

            client.MessageReceived += OnMessageReceived;
            client.ReactionAdded += OnReactionAdded;
        }

        private Task OnReactionAdded(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            for (int i = 0; i < _reactionResponses.Count; i++)
            {
                if (_reactionResponses[i].ReactionBlueprint.Name == reaction.Emote.Name)
                {
                    var eventArgs = new ReactionResponseEventArgs();
                    eventArgs.Player = Player;
                    eventArgs.SocketReaction = reaction;
                    _reactionResponses[i].Action.Invoke(eventArgs);
                }
            }

            return Task.CompletedTask;
        }

        async Task OnMessageReceived(SocketMessage socketMessage)
        {
            Phrase? phrase = _input.ProcessMessageForThisSession(socketMessage, _client, Guild);
            if (phrase != null)
            {
                Program.DebugLog("phrase relevant");
                for (int i = 0; i < _phraseResponses.Count; i++)
                {
                    if (_phraseResponses[i].PhraseBlueprint.MatchesPhrase(phrase))
                    {
                        var responseArgs = new PhraseResponseEventArgs();
                        responseArgs.Phrase = phrase;
                        responseArgs.Player = Player;
                        responseArgs.Message = socketMessage;
                        _phraseResponses[i].Action.Invoke(responseArgs);
                    }
                }
            }
        }
        
      
        
        
    }
}