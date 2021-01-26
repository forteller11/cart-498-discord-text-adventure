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
        public readonly DiscordSocketClient DissonanceBot;
        public readonly DiscordSocketClient MemeBot;
        public readonly DiscordSocketClient BodyBot;
        
        public readonly SocketGuild Guild;
   
        private Input _input;
        public readonly RoomManager RoomManager;
        
        private List<PhraseResponse> _phraseResponses;
        private List<ReactionResponse> _reactionResponses;
        
        public Player? Player;
        public Session(DiscordSocketClient dissonanceBot, DiscordSocketClient memeBot, DiscordSocketClient bodyBot, SocketGuild guild)
        {
            DissonanceBot = dissonanceBot;
            MemeBot = memeBot;
            BodyBot = bodyBot;
            Guild = guild;
            
            _input = new Input();
            _phraseResponses = ResponseTable.GetStaticPhraseResponseList();
            _reactionResponses = ResponseTable.GetStaticReactionResponseList();
            RoomManager = new RoomManager(this, guild);
            RoomManager.Screen.ChangeRoomVisibilityAsync(this, OverwritePermissions.DenyAll(RoomManager.Screen.Channel));

            dissonanceBot.MessageReceived += OnMessageReceived;
            dissonanceBot.ReactionAdded   += OnReactionAdded;
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            //var userMessage = await potentialMessage.GetOrDownloadAsync();
            IUser user = reaction.User.IsSpecified ? reaction.User.Value : Guild.GetUser(reaction.UserId);

            if (!_input.FilterMessage(user, channel, Guild))
                return;

            var eventArgs = new ReactionResponseEventArgs(this, reaction, user);

            for (int i = 0; i < _reactionResponses.Count; i++)
            {
                if (_reactionResponses[i].ReactionBlueprint.Name == reaction.Emote.Name)
                    _reactionResponses[i].CallResponses(eventArgs);
            }
        }

        async Task OnMessageReceived(SocketMessage socketMessage)
        {
            if (!_input.FilterMessage(socketMessage.Author, socketMessage.Channel, Guild))
                return;
            
            Phrase phrase = _input.ProcessMessage(socketMessage);

            for (int i = 0; i < _phraseResponses.Count; i++)
            { 
                if (_phraseResponses[i].PhraseBlueprint.MatchesPhrase(phrase))
                    _phraseResponses[i].CallResponses(new PhraseResponseEventArgs(phrase, socketMessage, RoomManager.RoomKV[socketMessage.Channel.Id], this));
            }
        }
        
        
        
      
        
        
    }
}