using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext;
using chext.Mechanics;
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
        public Dictionary<ulong, Room> RoomKV;
        public RoomCategory [] Categories;

        
        #region declaring rooms

        public readonly Room DissonanceDM;
        public readonly Room BodyDM;
        public readonly Room MemeDM;
        
        public readonly Room UserAgreement;
        
        public readonly Room DnD;
        public readonly Room Pokemon;
        public readonly Room Animals;
        
        public readonly RoomCategory Intro;
        public readonly RoomCategory Screen;
        #endregion

        public RoomManager(Session session, SocketGuild guild)
        {
            #region dm
            DissonanceDM = Room.CreateDMRoom();
            DissonanceDM = Room.CreateDMRoom();
            DissonanceDM = Room.CreateDMRoom();
            
            
            #endregion
            Intro = new RoomCategory("Welcome");
            Screen = new RoomCategory("Screens");
            
            #region create rooms
            UserAgreement = Room.CreateGuildRoom("User Agreement", Intro)
                .WithStaticDescriptions("don't harm system hardware")
                .WithReaction(new Emoji("✅"));
            
            
            DnD = Room.CreateGuildRoom("DnD", Screen)
                .WithStaticDescriptions("all stuff DnD, tolken, ttrp!")
                .WithObjects(
                   
                    );
            
            Pokemon = Room.CreateGuildRoom("Pokemon", Screen)
                .WithStaticDescriptions("gotta catch em' all");
            
            Animals = Room.CreateGuildRoom("Cute Animals", Screen)
                .WithStaticDescriptions("cats jumping from cucumbers, tiny frogs making impressive noises, we love them all!\nPreferably pictures!");
            
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
                Categories[i].LinkToDiscord(createCategoriesTasks[i].Result);

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
                    var channel = createChannelTasksArr[createChannelTaskIndex].Result;
                    RestGuildChannel? guildChannel = channel as RestGuildChannel; //will be null if dm channel
                    if (guildChannel == null && !Categories[i].Rooms[j].IsDMChannel)
                        throw new Exception("Inconsistent DM usage");
                    
                    Categories[i].Rooms[j].LinkToDiscord(createChannelTasksArr[createChannelTaskIndex].Result, guildChannel);
                    createChannelTaskIndex++;
                }
                
            }
            
            RoomKV = new Dictionary<ulong, Room>(Rooms.Length);
            for (int i = 0; i < Rooms.Length; i++)
                RoomKV.Add(Rooms[i].GuildChannel!.Id, Rooms[i]);

            #endregion
            

        }

        // public void ConnectRoomsToMessageChannels(SocketGuild guild)
        // {
        //     //guild.CreateCategoryChannelAsync()
        // }
   
    }
}