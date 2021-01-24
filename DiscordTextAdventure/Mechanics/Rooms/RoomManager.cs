using System.Collections.Generic;
using chext;
using Discord;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Rooms
{
    public class RoomManager
    {
        public Room [] Rooms;
        
        #region declaring rooms
        public readonly Room TestRoom;
        public readonly Room TestRoom2;
        #endregion

        public RoomManager(IMessageChannel channel)
        {
            //todo foreach room, check if room name exists, if not, create, if it does, clear the room, then add.
            TestRoom = new Room("test room name", "here lies a fun pot", channel);
            TestRoom2 = new Room("funny second room name", "i like doughnuts", channel);

            Rooms = Common.ClassMembersToArray<Room>(typeof(RoomManager), this);
            Program.DebugLog(Rooms.Length);
        }
   
    }
}