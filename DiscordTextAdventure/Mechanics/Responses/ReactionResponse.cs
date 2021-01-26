using System;
using chext.Mechanics;
using Discord;
using Discord.WebSocket;
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
        public Session Session;
        public SocketReaction SocketReaction;
        //public IUserMessage UserMessage;
        public IUser User;

        public ReactionResponseEventArgs(Session session, SocketReaction socketReaction, IUser user)
        {
            Session = session;
            SocketReaction = socketReaction;
            User = user;
        }
    }
}