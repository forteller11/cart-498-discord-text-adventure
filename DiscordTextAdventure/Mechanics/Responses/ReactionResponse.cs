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
        public readonly IEmote ReactionBlueprint;
        private readonly Action<ReactionResponseEventArgs>? Action;
        private readonly Func<ReactionResponseEventArgs, Task>? ActionAsync;
        public Room[] RoomFilter { get; set; }

        [Flags]
        public enum OnReactionTrigger
        {
            OnAdd    = 0b_0001,
            OnRemove = 0b_0010
        }
        public static OnReactionTrigger OnReactionTriggerBoth = OnReactionTrigger.OnAdd | OnReactionTrigger.OnRemove;
        public readonly OnReactionTrigger Trigger;

        public ReactionResponse(IEmote reactionBlueprint, OnReactionTrigger trigger, Action<ReactionResponseEventArgs> action, Func<ReactionResponseEventArgs, Task> actionAsync)
        {
            ReactionBlueprint = reactionBlueprint;
            Trigger = trigger;
            Action = action;
            ActionAsync = actionAsync;
        }
        
        public ReactionResponse WithRoomFilters(params Room[] filter)
        {
            RoomFilter = filter;
            return this;
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

        public ReactionResponseEventArgs(Session session, SocketReaction socketReaction, IUser user, Room postedRoom, bool isAdd)
        {
            Session = session;
            SocketReaction = socketReaction;
            User = user;
            PostedRoom = postedRoom;
        }
    }
}