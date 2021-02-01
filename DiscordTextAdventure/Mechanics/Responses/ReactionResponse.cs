using System;
using System.Threading.Tasks;
using chext.Mechanics;
using Discord;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ReactionResponse : IResponse
    {
        public readonly ReactionBlueprint ReactionBlueprint;
        private readonly Action<ReactionResponseEventArgs>? Action;
        private readonly Func<ReactionResponseEventArgs, Task>? ActionAsync;
        public Room[] RoomFilter { get; set; }

        public ReactionResponse(ReactionBlueprint reactionBlueprint, Action<ReactionResponseEventArgs> action, Func<ReactionResponseEventArgs, Task> actionAsync)
        {
            ReactionBlueprint = reactionBlueprint;
            Action = action;
            ActionAsync = actionAsync;
        }

        public void CallResponses(ReactionResponseEventArgs args)
        {
            Action?.Invoke(args);
            ActionAsync?.Invoke(args);
        }

        
    }
    
    public class ReactionResponseEventArgs
    {
        public readonly Session Session;
        public readonly SocketReaction SocketReaction;
        //public IUserMessage UserMessage;
        public readonly Room PostedRoom;
        public readonly IUser User;
        public readonly bool IsAdd;
        public readonly ReactionBlueprint.OnReactionTrigger TriggerFilter;

        public ReactionResponseEventArgs(Session session, SocketReaction socketReaction, IUser user, Room postedRoom, bool isAdd, ReactionBlueprint.OnReactionTrigger triggerFilter)
        {
            Session = session;
            SocketReaction = socketReaction;
            User = user;
            PostedRoom = postedRoom;
            IsAdd = isAdd;
            TriggerFilter = triggerFilter;
        }
    }
}