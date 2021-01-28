using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using chext;
using Discord;
using Discord.Rest;
using DiscordTextAdventure.Discord.Rendering;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Rooms
{
    public class Room
    {
        public string Name;
        public string Subtitle = String.Empty;
        public string StaticDescription = String.Empty;
        public Func<Room, string> DynamicDescription = DefaultDynamicDescription;
        public Emoji []? Reactions;

        private const string IS = "is";
        private const string ARE = "are";
        
        
        public List<AdventureObject> Objects = new List<AdventureObject>();
        
        public readonly bool  IsDMChannel;
        public IMessageChannel? MessageChannel;

        public IGuildChannel? GuildChannel;
        private IGuildChannel? _guildChannel
        {
            get => IsDMChannel ? throw new Exception("can't access guild of dm channel") : _guildChannel;
            set
            {
                if (IsDMChannel)
                    throw new Exception("can't access guild of dm channel");
                _guildChannel = value;
            }
        }

  
        public RoomRenderer? Renderer;

        private Room(bool isDmChannel)
        {
            IsDMChannel = isDmChannel;
        }

        public static Room CreateGuildRoom(string name, RoomCategory category)
        {
            var room = new Room(false);
            room.Name = name.Replace(' ', '_');
            category.Rooms.Add(room);
            return room;
        }

        public static Room CreateDMRoom()
        {
            return new Room(true);
        }
        

        public void LinkToDiscordAndDraw(IMessageChannel channel, IGuildChannel? guildChannel)
        {
            LinkToDiscord(channel, guildChannel);
            Renderer.DrawRoomStateEmbed();
        }
        
        public void LinkToDiscord(IMessageChannel channel, IGuildChannel? guildChannel)
        {
            if (IsDMChannel && guildChannel != null)
                throw new ArgumentException("This is a DM channel but was fed a guild channel");
            if (!IsDMChannel && guildChannel == null)
                throw new ArgumentException("This is a guild channel but was NOT fed a guild channel");
            
            MessageChannel = channel;
            GuildChannel = guildChannel;

            Renderer = new RoomRenderer(this, channel);
        }
        
        #region builder helpers
        public Room WithStaticDescriptions(string staticDescription) 
        {
            StaticDescription = staticDescription;
            return this;
        }
        
        public Room WithSubtitle(string subtitle) 
        {
            Subtitle = subtitle;
            return this;
        }

        public Room WithDynamicDescription(Func<Room, string> dynamicDescription)
        {
            DynamicDescription = dynamicDescription;
            return this;
        }
        
        public Room WithObjects(params AdventureObject[] objects)
        {
            Objects = objects.ToList();
            return this;
        }

        public Room WithReaction(params Emoji[] reactions)
        {
            for (int i = 0; i < reactions.Length; i++)
            {
                Program.DebugLog (reactions[i].Name) ;
            }
            Reactions = reactions;
            return this;
        }

        #endregion
        
        

        static string DefaultDynamicDescription(Room room)
        {
            StringBuilder builder = new StringBuilder("");

            if (room.Objects.Count == 0)
            {
                builder.Append("The room is empty.");
            }
            for (int i = 0; i < room.Objects.Count; i++)
            {
                var obj = room.Objects[i];
                bool isLastElement = i == room.Objects.Count - 1;
                
                if (i == 0)
                    builder.Append($"There {(obj.IsPlural ? "are" : "is")}");
                else if (isLastElement) //else if means that this will only apply if last element and there is MORE than one object
                    builder.Append(", and");
                else
                    builder.Append(",");
                
                
                builder.Append(" " + obj.Article + " " + room.Objects[i].Name);
                
                if (isLastElement)
                    builder.Append(".");
            }

            return builder.ToString();
        }
    }
}