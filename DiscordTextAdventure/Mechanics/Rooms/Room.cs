﻿using System;
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
        public string StaticDescription;
        public Func<Room, string> DynamicDescription;
        
        public List<AdventureObject> Objects = new List<AdventureObject>();
        
        public IMessageChannel? Channel;
        public RoomRenderer? Renderer;

        public Room(string name, string staticDescription, params AdventureObject [] objects)
        {
            Name = name.Replace(' ', '_');
            StaticDescription = staticDescription;
            
            Objects = objects.ToList();
        }

        public void Init(IMessageChannel channel)
        {
            Channel = channel;
            Renderer = new RoomRenderer(channel);
        }
        
        

        static string DefaultDynamicDescription(Room room)
        {
            StringBuilder builder = new StringBuilder();
            
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