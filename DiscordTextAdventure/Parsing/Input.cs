using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext;
using chext.Mechanics;
using Discord.WebSocket;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable

namespace DiscordTextAdventure.Parsing
{
    public class Input
    {
        private List<Response> _responses;
        
        private Tokenizer _tokenizer;
        private Parser _parser;
        private readonly SocketGuild _guild;
        private readonly DiscordSocketClient _client;

        public Input(DiscordSocketClient client, SocketGuild guild)
        {
            _client = client;
            _guild = guild;
            
            _tokenizer = new Tokenizer();
            _parser = new Parser();
            _responses = new List<Response>();

            _client.MessageReceived += ClientOnMessageReceived;
        }

        public void AddResponse(Response response)
        {
            _responses.Add(response);
        }
        
        #region onClientEvents
        private Task ClientOnMessageReceived(SocketMessage message)
        {
            #region make sure message is appropriate for session
            Program.DebugLog("Game Manager message received");
            if (message.Author.Id == _client.CurrentUser.Id)
                return Task.CompletedTask;
            
            SocketGuildChannel? guildChannel = message.Channel as SocketGuildChannel;

            if (guildChannel == null)
            {
                Program.DebugLog("message sent by DMS?");
                return Task.CompletedTask;
            }

            if (guildChannel.Id != _guild.Id)
            {
                Program.DebugLog("message not part of relevant guild");
                return Task.CompletedTask;
            }
            #endregion

            ProcessMessage(message.Content);
            return Task.CompletedTask;
        }
        
        
        #endregion
        
        
        public void ProcessMessage(string message)
        {
           var tokens = _tokenizer.Tokenize(message);
           var phrase = _parser.Parse(tokens);

           for (int i = 0; i < _responses.Count; i++)
           {
               if (_responses[i].PhraseBlueprint.MatchesPhrase(phrase))
                   _responses[i].Action.Invoke(phrase);
           }
           
        }
    }
}