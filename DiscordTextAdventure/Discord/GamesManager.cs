
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
        
        private Dictionary<ulong, Session> _sessions = new Dictionary<ulong, Session>();

        /// <summary>
        /// assumed to be called after _client is Ready()
        /// </summary>
        /// <param name="client"></param>
        public GamesManager(DiscordSocketClient client)
        {
            Program.DebugLog("Game manger created");
            _client = client;
            
            //todo create sessions for each joined server that isn't currently playing??
        }

    }
}