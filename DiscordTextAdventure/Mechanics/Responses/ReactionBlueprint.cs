using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Rooms;

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ReactionBlueprint
    {
        public readonly IEmote Emoji;
        [Flags]
        public enum OnReactionTrigger
        {
            Never    = 0b_0000,
            OnAdd    = 0b_0001,
            OnRemove = 0b_0010
        }
        public static OnReactionTrigger OnReactionTriggerBoth = OnReactionTrigger.OnAdd | OnReactionTrigger.OnRemove;
        public readonly OnReactionTrigger TriggerFilter;
        public readonly List<Room>? RoomFilter;

        public ReactionBlueprint(IEmote emoji, OnReactionTrigger trigger)
        {
            Emoji = emoji;
            TriggerFilter = trigger;
            RoomFilter = null;
        }
        
        public ReactionBlueprint(IEmote emoji, OnReactionTrigger trigger, Room roomFilter)
        {
            Emoji = emoji;
            TriggerFilter = trigger;
            RoomFilter = new List<Room>{roomFilter};
        }
        
        public ReactionBlueprint(IEmote emoji, OnReactionTrigger trigger, Room [] roomFilter)
        {
            Emoji = emoji;
            TriggerFilter = trigger;
            RoomFilter = new List<Room>(roomFilter.Length);
            RoomFilter.AddRange(roomFilter);
        }

        public bool Matches(ReactionResponseEventArgs e)
        {
            if (RoomFilter != null)
            {
                if (!RoomFilter.Contains(e.PostedRoom))
                    return false;
            }
            
            OnReactionTrigger combined = e.TriggerFilter & this.TriggerFilter;
            if (combined != e.TriggerFilter)
                return false;
            
            if (e.SocketReaction.Emote.Name != Emoji.Name)
                return false;

            return true;
        }
        
        
    }
}