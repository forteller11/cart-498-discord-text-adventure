
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using chext.Discord.Parsing;
using Discord;
using Discord.WebSocket;
using Game = chext.Mechanics.Game;

#nullable enable

namespace chext.Discord
{
    
    public class GamesManager
    {
        private Parsing.pa _parser;
        private DiscordSocketClient _client;
        
        private Dictionary<ulong, Game> _games = new Dictionary<ulong, Game>();

        private Dictionary<ulong, GameProposal> _proposals = new Dictionary<ulong, GameProposal>();

        /// <summary>
        /// assumed to be called after _client is Ready()
        /// </summary>
        /// <param name="client"></param>
        public GamesManager(DiscordSocketClient client)
        {
            Program.DebugLog("Game manger created");
            _client = client;
            
            _parser = new PreGameParser();
            // _parser.GameProposalHandler += OnGameProposalProposal;
            // _parser.JoinHandler += OnJoin;
            // _parser.JoinSideHandler += OnJoinSide;
            
            _client.MessageReceived += OnMessageReceived;
        }

        

        public Task OnMessageReceived(SocketMessage message)
        {
            Program.DebugLog("Game Manager message received");
            if (message.Author.Id == _client.CurrentUser.Id)
                return Task.CompletedTask;

            Parser.Parse(message);
            
            foreach (var gameKV in _games)
            {
                var game = gameKV.Value;
                if (message.Channel.Id == game.Channel.Id)
                {
                    game.InChannelNonSelfMessageReceived(message);
                }
            }
            
            return Task.CompletedTask;
        }



      
        
    }
}