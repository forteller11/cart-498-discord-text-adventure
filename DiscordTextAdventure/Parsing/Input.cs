using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext;
using Discord;
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
        
        public bool FilterMessage(IUser author, IChannel channel, SocketGuild guild)
        {
            
            if (author.IsBot)
                return false;
            
            SocketGuildChannel? guildChannel = channel as SocketGuildChannel;

            if (guildChannel == null)
                Program.DebugLog("message sent by DMS?");

            else if (guildChannel.Guild.Id != guild.Id)
            {
                Program.DebugLog("message not part of relevant guild");
                return false;
            }

            return true;
        }

        
        public Phrase ProcessMessage(SocketMessage message)
        {
           var tokens = _tokenizer.Tokenize(message.Content);
           var phrase = _parser.Parse(tokens);
           return phrase;
        }
        
        
    }
}