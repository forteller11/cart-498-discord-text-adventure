using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Responses;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable

namespace DiscordTextAdventure.Parsing
{
    public class Input
    {
        private Tokenizer _tokenizer;
        private Parser _parser;

        public Input()
        {
            _tokenizer = new Tokenizer();
            _parser = new Parser();

        }
        
        #region onClientEvents
        public Phrase? ProcessMessageForThisSession(SocketMessage message, DiscordSocketClient client, SocketGuild guild)
        {
            #region make sure message is appropriate for session
            Program.DebugLog("Game Manager message received");
            if (message.Author.Id == client.CurrentUser.Id)
                return null;
            
            SocketGuildChannel? guildChannel = message.Channel as SocketGuildChannel;

            if (guildChannel == null)
            {
                Program.DebugLog("message sent by DMS?");
                return null;
            }

            if (guildChannel.Guild.Id != guild.Id)
            {
                Program.DebugLog("message not part of relevant guild");
            }
            #endregion

            return ProcessMessage(message);
      
        }
        #endregion

        private Phrase ProcessMessage(SocketMessage message)
        {
           var tokens = _tokenizer.Tokenize(message.Content);
           var phrase = _parser.Parse(tokens, message.Channel);
           return phrase;
        }
        
        
    }
}