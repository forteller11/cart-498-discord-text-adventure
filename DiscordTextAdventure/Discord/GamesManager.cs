
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
        private DiscordSocketClient _disonanceBot;
        private DiscordSocketClient _memeBot;
        private DiscordSocketClient _bodyBot;
        private List<Session> _sessions;

        /// <summary>
        /// assumed to be called after _client is Ready()
        /// </summary>
        /// <param name="client"></param>
        public GamesManager(DiscordSocketClient dissonanceBot, DiscordSocketClient memeBot, DiscordSocketClient bodyBot)
        {
            
            _sessions = new List<Session>(dissonanceBot.Guilds.Count);

            if (dissonanceBot.Guilds.Count != memeBot.Guilds.Count || memeBot.Guilds.Count != bodyBot.Guilds.Count)
                throw new Exception("bots are not added to same amount of servers!");

            foreach (var guild in dissonanceBot.Guilds)
                _sessions.Add(new Session(dissonanceBot, memeBot, bodyBot, guild));
        }


    }
}