using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
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
        
        public List<AdventureObject> Objects = new List<AdventureObject>();
        
        public IMessageChannel? Channel;
        public RoomRenderer? Renderer;

        public Room(string name)
        {
            Name = name.Replace(' ', '_');
        }
        

        public void Init(IMessageChannel channel)
        {
            Channel = channel;
            Renderer = new RoomRenderer(this, channel);
            Renderer.DrawRoom();
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
                if (i == 0)
                    builder.Append("There is a");
                else if (i == room.Objects.Count - 1 && room.Objects.Count > 1)
                    builder.Append(", and a ");
                else
                    builder.Append(", a");
                
                builder.Append(room.Objects[i].Name);
                
                if (i == room.Objects.Count - 1)
                    builder.Append(".");
            }

            return builder.ToString();
        }
    }
}