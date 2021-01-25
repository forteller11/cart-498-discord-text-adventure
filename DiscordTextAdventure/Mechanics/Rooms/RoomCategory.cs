using System.Collections.Generic;
using Discord.Commands;
using Discord.Rest;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Rooms
{
    public class RoomCategory
    {
        public string Name;
        public RestCategoryChannel? Channel;
        public List<Room> Rooms;

        public RoomCategory(string name)
        {
            Name = name;
            Rooms = new List<Room>();
        }

        public void Init(RestCategoryChannel channel)
        {
            Channel = channel;
        }
    }
}