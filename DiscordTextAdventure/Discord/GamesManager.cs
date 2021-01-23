
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
        private Input _input;
        private DiscordSocketClient _client;
        
        private Dictionary<ulong, Session> _games = new Dictionary<ulong, Session>();

        private Dictionary<ulong, GameProposal> _proposals = new Dictionary<ulong, GameProposal>();

        /// <summary>
        /// assumed to be called after _client is Ready()
        /// </summary>
        /// <param name="client"></param>
        public GamesManager(DiscordSocketClient client)
        {
            Program.DebugLog("Game manger created");
            _client = client;
            
            _input = new Input();
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
            
            _input.ProcessMessage(message.Content);
            
            //todo foreach (var gameKV in _games)
            // {
            //     var game = gameKV.Value;
            //     if (message.Channel.Id == game.Channel.Id)
            //     {
            //         game.InChannelNonSelfMessageReceived(message);
            //     }
            // }
            
            return Task.CompletedTask;
        }



      
        
    }
}