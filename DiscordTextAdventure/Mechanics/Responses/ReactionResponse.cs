using System;
using System.Threading.Tasks;
using chext.Mechanics;
using Discord;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.User;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ReactionResponse
    {
        public readonly IEmote ReactionBlueprint;
        private readonly Action<ReactionResponseEventArgs>? Action;
        private readonly Func<ReactionResponseEventArgs, Task>? ActionAsync;
        
        public ReactionResponse(Emoji reactionBlueprint, Action<ReactionResponseEventArgs> action)
        {
            ReactionBlueprint = reactionBlueprint;
            Action = action;
        }
        
        public ReactionResponse(Emoji reactionBlueprint, Func<ReactionResponseEventArgs, Task> action)
        {
            ReactionBlueprint = reactionBlueprint;
            ActionAsync = action;
        }

        public void CallResponses(ReactionResponseEventArgs args)
        {
            Action?.Invoke(args);
            ActionAsync?.Invoke(args);
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