using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordTextAdventure;
using DiscordTextAdventure.Mechanics;
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
        public Action<Session> SessionReset;
        
        public readonly DiscordSocketClient DissonanceBot;
        public readonly DiscordSocketClient MemeBot;
        public readonly DiscordSocketClient BodyBot;

        public int SucessfulAnimalPosts = 0;
        public int SucessfulDnDPosts = 0;
        public int SucessfulPokemonPosts = 0;

        public int timesMessagedMemeBotNoDM = 0;
        
        public readonly SocketGuild Guild;
        public readonly HttpClient HttpClient;
   
        private Input _input;
        public readonly RoomManager RoomManager;
        public readonly ReactionResponseTable ReactionTable;
        public readonly PhraseResponseTable PhraseResponseTable;

        public Player? Player;
        public Session(DiscordSocketClient dissonanceBot, DiscordSocketClient memeBot, DiscordSocketClient bodyBot, SocketGuild guild, Action<Session> sessionReset)
        {
            DissonanceBot = dissonanceBot;
            MemeBot = memeBot;
            BodyBot = bodyBot;
            Guild = guild;
            SessionReset += sessionReset;
            
            _input = new Input();
            HttpClient = new HttpClient();

            //ORDER MATTERS
            PhraseResponseTable = new PhraseResponseTable();
            RoomManager = new RoomManager(this, PhraseResponseTable, guild);
            PhraseResponseTable.Init(RoomManager);
            
            ReactionTable = new ReactionResponseTable(this);

            DissonanceBot.MessageReceived += OnMessageReceived;
            
            DissonanceBot.ReactionAdded   += OnReactionAdded;
            DissonanceBot.ReactionRemoved += OnReactionRemoved;
            
        }

        public async Task OnReactionAdded(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            OnReactionChanged(potentialMessage, channel, reaction, true, ReactionBlueprint.OnReactionTrigger.OnAdd);
        }

        public async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            OnReactionChanged(potentialMessage, channel, reaction, false, ReactionBlueprint.OnReactionTrigger.OnRemove);
        }
        
        private async Task OnReactionChanged(Cacheable<IUserMessage, ulong> potentialMessage, ISocketMessageChannel channel, SocketReaction reaction, bool isOnAdd, ReactionBlueprint.OnReactionTrigger triggerType)
        {
            //var userMessage = await potentialMessage.GetOrDownloadAsync();
            IUser user = reaction.User.IsSpecified ? reaction.User.Value : Guild.GetUser(reaction.UserId);

            if (!_input.FilterMessage(user, channel, Guild))
                return;
            
            var eventArgs = new ReactionResponseEventArgs(this, reaction, user, RoomManager.RoomKV[channel.Id], isOnAdd, triggerType);
            
            for (int i = 0; i < ReactionTable.ReactionResponses.Length; i++)
            {
                if (ReactionTable.ReactionResponses[i].ReactionBlueprint.Matches(eventArgs))
                    ReactionTable.ReactionResponses[i].CallResponses(eventArgs);
            }
        }

        public async Task OnMessageReceived(SocketMessage socketMessage)
        {
            if (!_input.FilterMessage(socketMessage.Author, socketMessage.Channel, Guild))
                return;

            if (Player == null)
            {
                Program.DebugLog($"No Player No Message!");
                return;
            }

            Program.DebugLog($"Message received at {Guild.Name}");
            Tuple<Phrase?, Link?> parseResult = await _input.ProcessMessage(socketMessage);

            if (parseResult.Item1 != null)
            {
                var phraseResponses = PhraseResponseTable.PhraseResponses;
                for (int i = 0; i < phraseResponses.Count; i++)
                {
                    var roomOfMessage = RoomManager.RoomKV[socketMessage.Channel.Id];

                    if (phraseResponses[i].PhraseBlueprint.MatchesPhrase(parseResult.Item1, roomOfMessage))
                    {
                        phraseResponses[i].CallResponses(new PhraseResponseEventArgs(parseResult.Item1, socketMessage, roomOfMessage, this));
                    }
                }
            }

            if (parseResult.Item2 != null)
            {
                var linkResponses = LinkResponseTable.LinkResponses;
                for (int i = 0; i < linkResponses.Length; i++)
                {
                    linkResponses[i].CallResponses(new LinkResponseEventArgs(this, parseResult.Item2, RoomManager.RoomKV[socketMessage.Channel.Id]));
                }
            }
        }

    }
}