using System.Collections.Generic;
using System.Threading.Tasks;
using chext;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

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

        public RoomManager(DiscordSocketClient client, SocketGuild guild)
        {
            #region blueprint
            TestRoom = new Room("test room name", "here lies a fun pot");
            TestRoom2 = new Room("funny second room name", "i like doughnuts");
            #endregion
            
            Rooms = Common.ClassMembersToArray<Room>(typeof(RoomManager), this);
            
            #region MyRegion

            List<Task> createChannelTasks = new List<Task>();
            
            for (int i = 0; i < Rooms.Length; i++)
            {
                var currentRoom = Rooms[i];
                bool foundExistingChannelWithSameName = false;
                foreach (var channel in guild.TextChannels)
                {
                    if (currentRoom.Name == channel.Name)
                    {
                        currentRoom.Init((IMessageChannel) channel);
                        foundExistingChannelWithSameName = true;
                        break;
                    }
                }
                
                //if haven't found channel, create new channels
                if (!foundExistingChannelWithSameName)
                {
                    var task = guild.CreateTextChannelAsync(Rooms[i].Name, null, null);
                    var task2 = task.ContinueWith((e) =>
                    {
                        currentRoom.Init((IMessageChannel) e.Result);
                    });
                    createChannelTasks.Add(task2);
                }

                Task.WaitAll(createChannelTasks.ToArray());
                //wait for tasks
            } 
       
           

            #endregion
            //todo foreach room, check if room name exists, if not, create, if it does, clear the room, then add.
            TestRoom = new Room("test room name", "here lies a fun pot");
            TestRoom2 = new Room("funny second room name", "i like doughnuts");

            //todo init
            Program.DebugLog(Rooms.Length);
        }
   
    }
}