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
        public readonly Action<ReactionResponseEventArgs, IUserMessage> ActionWithUserMessage;
        
        public ReactionResponse(Emoji reactionBlueprint, Action<ReactionResponseEventArgs> action, Action<ReactionResponseEventArgs, IUserMessage> actionWithUserMessage)
        {
            ReactionBlueprint = reactionBlueprint;
            Action = action;
            ActionWithUserMessage = actionWithUserMessage;
        }
    }
    
    public class ReactionResponseEventArgs
    {
        public Player? Player;
        public SocketReaction SocketReaction;


    }
}