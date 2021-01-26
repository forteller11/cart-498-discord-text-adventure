using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordTextAdventure;
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
            client.ReactionAdded   += OnReactionAdded;
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            //var userMessage = await potentialMessage.GetOrDownloadAsync();
            IUser user = (reaction.User.IsSpecified) ? reaction.User.Value : Guild.GetUser(reaction.UserId);
            
            #region qualify response

            if (user.IsBot)
            {
                Program.DebugLog("reaction is from a bot");
                return;
            }
            
            SocketGuildChannel? guildChannel = channel as SocketGuildChannel;
            if (guildChannel == null)
            {
                Program.DebugLog("reaction dm?");
            }
            else if (guildChannel.Guild.Id != Guild.Id)
            {
                Program.DebugLog("reaction didnt occur in this guild!");
                return;
            }
            
            var eventArgs = new ReactionResponseEventArgs(this, reaction, user);
            #endregion
            
            for (int i = 0; i < _reactionResponses.Count; i++)
            {
                if (_reactionResponses[i].ReactionBlueprint.Name == reaction.Emote.Name)
                    _reactionResponses[i].Action.Invoke(eventArgs);
            }
        }

        async Task OnMessageReceived(SocketMessage socketMessage)
        {
            Phrase? phrase = _input.ProcessMessageForThisSession(socketMessage, _client, Guild);

            if (phrase == null) //if phrase was meant for another guild, or was sent by self
                return;
            
            for (int i = 0; i < _phraseResponses.Count; i++)
            { 
                if (_phraseResponses[i].PhraseBlueprint.MatchesPhrase(phrase)) 
                {
                    //todo calculate room of phrase... or use room of phrase as part of the response signature
                    _phraseResponses[i].Action.Invoke(new PhraseResponseEventArgs(phrase, socketMessage, _roomsManager.RoomKV[socketMessage.Channel.Id], this));
                }
            }
            
        }
        
      
        
        
    }
}