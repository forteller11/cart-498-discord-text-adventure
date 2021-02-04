
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
        private DiscordSocketClient _dissonanceBot;
        private DiscordSocketClient _memeBot;
        private DiscordSocketClient _bodyBot;
        private List<Session> _sessions;

        /// <summary>
        /// assumed to be called after _client is Ready()
        /// </summary>
        /// <param name="client"></param>
        public GamesManager(DiscordSocketClient dissonanceBot, DiscordSocketClient memeBot, DiscordSocketClient bodyBot)
        {
            _dissonanceBot = dissonanceBot;
            _memeBot = memeBot;
            _bodyBot = bodyBot;
            _sessions = new List<Session>(_dissonanceBot.Guilds.Count);

            if (_dissonanceBot.Guilds.Count != _memeBot.Guilds.Count || _memeBot.Guilds.Count != _bodyBot.Guilds.Count)
                throw new Exception("bots are not added to same amount of servers!");

            foreach (var guild in _dissonanceBot.Guilds)
                _sessions.Add(new Session(_dissonanceBot, _memeBot, _bodyBot, guild, OnSessionReset));
        }

        void OnSessionReset(Session session)
        {
            _sessions.Remove(session);
            foreach (var guild in _dissonanceBot.Guilds)
            {
                bool guildHasCorrespondingSession = false;
                for (int i = 0; i < _sessions.Count; i++)
                {
                    if (_sessions[i].Guild.Id == guild.Id)
                    {
                        guildHasCorrespondingSession = true;
                        break;
                    }
                }
                
                if (!guildHasCorrespondingSession)
                    _sessions.Add(new Session(_dissonanceBot, _memeBot, _bodyBot, guild, OnSessionReset));
            }
            
        }

    }
}