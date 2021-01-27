using System;
using System.Collections.Generic;
using System.Linq;
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
        
        //private List<PhraseResponse> _phraseResponses;
        //private List<ReactionResponse> _reactionResponses;
        
        public Player? Player;
        public Session(DiscordSocketClient dissonanceBot, DiscordSocketClient memeBot, DiscordSocketClient bodyBot, SocketGuild guild)
        {
            DissonanceBot = dissonanceBot;
            MemeBot = memeBot;
            BodyBot = bodyBot;
            Guild = guild;
            
            _input = new Input();

            RoomManager = new RoomManager(this, guild);
            RoomManager.Screen.ChangeRoomVisibilityAsync(this, OverwritePermissions.DenyAll(RoomManager.Screen.Channel));

            dissonanceBot.MessageReceived += OnMessageReceived;
            dissonanceBot.ReactionAdded   += OnReactionAdded;
            dissonanceBot.ReactionRemoved += OnReactionRemoved;
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            OnReactionChanged(ReactionResponseTable.OnReactionAddedResponseEvents, potentialMessage, channel, reaction);
        }
        
        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            OnReactionChanged(ReactionResponseTable.OnReactionRemovedResponseEvents, potentialMessage, channel, reaction);
        }
        
        private async Task OnReactionChanged(List<ReactionResponse> reactionResponses, Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            //var userMessage = await potentialMessage.GetOrDownloadAsync();
            IUser user = reaction.User.IsSpecified ? reaction.User.Value : Guild.GetUser(reaction.UserId);

            if (!_input.FilterMessage(user, channel, Guild))
                return;

            var eventArgs = new ReactionResponseEventArgs(this, reaction, user, RoomManager.RoomKV[channel.Id]);
            
            for (int i = 0; i < reactionResponses.Count; i++)
            {
                if (reactionResponses[i].ReactionBlueprint.Name == reaction.Emote.Name)
                    reactionResponses[i].CallResponses(eventArgs);
            }
        }

        async Task OnMessageReceived(SocketMessage socketMessage)
        {
            if (!_input.FilterMessage(socketMessage.Author, socketMessage.Channel, Guild))
                return;

            Program.DebugLog($"Message received at {Guild.Name}");
            Tuple<Phrase?, Link?> parseResult = await _input.ProcessMessage(socketMessage);

            if (parseResult.Item1 != null)
            {
                var phraseResponses = PhraseResponseTable.PhraseResponses;
                for (int i = 0; i < phraseResponses.Length; i++)
                {
                    if (phraseResponses[i].PhraseBlueprint.MatchesPhrase(parseResult.Item1))
                    {
                        phraseResponses[i].CallResponses(new PhraseResponseEventArgs(parseResult.Item1, socketMessage, RoomManager.RoomKV[socketMessage.Channel.Id], this));
                    }
                }
            }

            if (parseResult.Item2 != null)
            {
                var linkResponses = LinkResponseTable.LinkResponses;
                for (int i = 0; i < linkResponses.Length; i++)
                {
                    linkResponses[i].CallResponses(new PhraseResponseEventArgs(parseResult.Item1, socketMessage, RoomManager.RoomKV[socketMessage.Channel.Id], this));
                }
            }
        }
        
        
        
      
        
        
    }
}