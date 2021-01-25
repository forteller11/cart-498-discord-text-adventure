using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
//https://www.fileformat.info/info/emoji/white_check_mark/index.htm for emoji unicode representations
#nullable enable
namespace DiscordTextAdventure.Mechanics.Rooms
{
    public class RoomManager
    {
        public Room [] Rooms;
        public RoomCategory [] Categories;

        
        #region declaring rooms
        
        public readonly Room UserAgreement;
        public readonly Room TestRoom;
        public readonly Room TestRoom2;
        
        public readonly RoomCategory Intro;
        public readonly RoomCategory Screen;
        #endregion

        public RoomManager(DiscordSocketClient client, SocketGuild guild)
        {
            Intro = new RoomCategory("Welcome");
            Screen = new RoomCategory("Screens");
            
            #region create rooms
            UserAgreement = new Room("User Agreement", Intro)
                .WithStaticDescriptions("don't harm system hardware")
                .WithReaction(new Emoji("✅"));
            
            
            TestRoom = new Room("test room name", Screen)
                .WithStaticDescriptions("here lies a fun pot")
                .WithObjects(
                    new AdventureObject("apple", "the apple has a tooth in it"),
                    new AdventureObject("pole", "a long thin pole")
                    );
            
            TestRoom2 = new Room("funny second room name", Screen)
                .WithStaticDescriptions("i like doughnuts");
            
            #endregion
            
            Rooms = Common.ClassMembersToArray<Room>(typeof(RoomManager), this);
            Categories = Common.ClassMembersToArray<RoomCategory>(typeof(RoomManager), this);
            
            #region tie channels and categories to their discord entities (by creating them), and put channels in their proper categories
        
            //todo could make this faster by immediately creating the rooms of a category channel when it's done
            //todo instead of wait for ALL the categories to be done and then creating text channels
            
            //clean slate
            Task [] deleteTasks = new Task[guild.Channels.Count];
            int index = 0;
            foreach (var channel in guild.Channels)
            {
                deleteTasks[index] = channel.DeleteAsync();
                index++;
            }

            //Task.WaitAll(deleteTasks);

          
            Task<RestCategoryChannel> [] createCategoriesTasks = new Task<RestCategoryChannel>[Categories.Length];
            for (int i = 0; i < Categories.Length; i++)
                createCategoriesTasks[i] = guild.CreateCategoryChannelAsync(Categories[i].Name);
            
            Task.WaitAll(createCategoriesTasks);
            
            List<Task<RestTextChannel>> createChannelTasks = new List<Task<RestTextChannel>>();
            for (int i = 0; i < Categories.Length; i++)
            {
                var category = Categories[i];
                Categories[i].Init(createCategoriesTasks[i].Result);
             
                for (int j = 0; j < Categories[i].Rooms.Count; j++)
                {
                    var room = category.Rooms[j];

                    Program.DebugLog(room.Name);
                    var textCreateTask = guild.CreateTextChannelAsync(room.Name, props =>
                    {
                        props.CategoryId = Categories[i].Channel.Id;
                    });

                    createChannelTasks.Add(textCreateTask);
                }
                
            }

            var createChannelTasksArr = createChannelTasks.ToArray();
            Task.WaitAll(createChannelTasksArr);

            int createChannelTaskIndex = 0;
            for (int i = 0; i < Categories.Length; i++)
            {
                for (int j = 0; j < Categories[i].Rooms.Count; j++)
                {
                    Categories[i].Rooms[j].Init(createChannelTasksArr[createChannelTaskIndex].Result);
                    createChannelTaskIndex++;
                }
            }

            #endregion
            

        }

        // public void ConnectRoomsToMessageChannels(SocketGuild guild)
        // {
        //     //guild.CreateCategoryChannelAsync()
        // }
   
    }
}