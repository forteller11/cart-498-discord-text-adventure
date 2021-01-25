using System;
using Discord;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ReactionResponse
    {
        public readonly IEmote ReactionBlueprint;
        public readonly Action<ReactionResponseEventArgs> Action;
        
        public ReactionResponse(Emoji reactionBlueprint, Action<ReactionResponseEventArgs> action)
        {
            ReactionBlueprint = reactionBlueprint;
            Action = action;
        }
    }
    
    public class ReactionResponseEventArgs
    {
        public Player? Player;
        public SocketReaction SocketReaction;


    }
}