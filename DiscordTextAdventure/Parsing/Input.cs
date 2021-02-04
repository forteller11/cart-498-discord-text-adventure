using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        
        public async Task<Tuple<Phrase?, Link?>> ProcessMessage(SocketMessage message)
        {
            var tokens = _tokenizer.Tokenize(message.Content);
            
            if (Uri.TryCreate(message.Content, UriKind.Absolute, out var validURI))
            {
                Program.DebugLog("Message is a url");
                var isValid = await IsValidLink(validURI);
                Program.DebugLog($"Valid ur: {isValid}");
                return new Tuple<Phrase?, Link?>(null , new Link(message, tokens, isValid));
            }
            else
            {
               return new Tuple<Phrase?, Link?>(_parser.Parse(tokens), null);
            }
        }
        

        async Task<bool> IsValidLink(Uri uri)
        {
            try //gross
            {
                HttpResponseMessage response = await Program.HttpClient.GetAsync(uri);
                return response.IsSuccessStatusCode;
            }
            catch(HttpRequestException e) //ew
            {
                Program.DebugLog("\nException Caught!");	
                Program.DebugLog("Message :{0} ",e.Message);
                return false;
            }
        }
        
        
    }
}