
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext.Mechanics;
using Discord.WebSocket;
using DiscordTextAdventure.Parsing;

#nullable enable

namespace chext.Discord
{
    
    public class GamesManager
    {
        private DiscordSocketClient _client;
        private List<Session> _sessions;

        /// <summary>
        /// assumed to be called after _client is Ready()
        /// </summary>
        /// <param name="client"></param>
        public GamesManager(DiscordSocketClient client)
        {
            _client = client;
            _sessions = new List<Session>(client.Guilds.Count);

            foreach (var guild in client.Guilds)
                _sessions.Add(new Session(_client,guild));
        }


    }
}