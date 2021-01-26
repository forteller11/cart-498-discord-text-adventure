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
            //Player = new Player();

            dissonanceBot.MessageReceived += OnMessageReceived;
            dissonanceBot.ReactionAdded   += OnReactionAdded;
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
                    _reactionResponses[i].CallResponses(eventArgs);
            }
        }

        async Task OnMessageReceived(SocketMessage socketMessage)
        {
            Phrase? phrase = _input.ProcessMessageForThisSession(socketMessage, DissonanceBot, Guild);

            if (phrase == null) //if phrase was meant for another guild, or was sent by self
                return;
            
            for (int i = 0; i < _phraseResponses.Count; i++)
            { 
                if (_phraseResponses[i].PhraseBlueprint.MatchesPhrase(phrase)) 
                {
                    //todo calculate room of phrase... or use room of phrase as part of the response signature
                    _phraseResponses[i].CallResponses(new PhraseResponseEventArgs(phrase, socketMessage, RoomManager.RoomKV[socketMessage.Channel.Id], this));
                }
            }
            
        }
        
      
        
        
    }
}