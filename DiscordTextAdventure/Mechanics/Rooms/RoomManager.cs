using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext;
using chext.Mechanics;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordTextAdventure.Parsing.Tables;
using DiscordTextAdventure.Reflection;

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

        //these are set after a new player has joined
        [ReflectionHelpers.DontIncludeInMembersToArray] public Room DissonanceDM;
        [ReflectionHelpers.DontIncludeInMembersToArray] public Room BodyDM;
        [ReflectionHelpers.DontIncludeInMembersToArray] public Room MemeDM;
        
        public readonly Room UserAgreement;
        
        public readonly Room DnD;
        public readonly Room Pokemon;
        public readonly Room Animals;
        
        public readonly Room Office;

        public readonly RoomCategory Intro;
        public readonly RoomCategory Screen;
        public readonly RoomCategory TheCloud;
        #endregion

        public RoomManager(Session session, SocketGuild guild)
        {
            #region dm

            DissonanceDM = Room.CreateDMRoom();

            BodyDM = Room.CreateDMRoom().WithSubtitle("It's yours, warts and all")
                .WithStaticDescriptions(
                    "Things have been better. You should probably drink more water, or go for a walk... pretty much anything is better than sitting at a computer all day.")
                .WithObjects(
                    new AdventureObject(NounTable.Arms, "All this typing can't be good for your wrists, you should look into getting a better mouse pad or wrist brace or something.", true)
                        .WithInspectDefault()
                        .WithPickupNoSense(),
                    
                    new AdventureObject(NounTable.Legs, "Your knees ache from being bent so long", true)
                        .WithCannotPickup(),
                
                    new AdventureObject(NounTable.Head, "You can't see your own head, but you do take note of growing pain of your the headphones on its ears. Forcing itself into conscious thought.", true)
                        .WithPickupNoSense()
                );


                MemeDM = Room.CreateDMRoom();
            #endregion
            
            Intro = new RoomCategory("Welcome");
            Screen = new RoomCategory("Screens");
            TheCloud = new RoomCategory("The Cloud");
            
            #region create rooms
            UserAgreement = Room.CreateGuildRoom("User Agreement", Intro)
                .WithStaticDescriptions("Here at Dissonance we believe in free communication and sharing. \n" +
                                        "Communities are stronger when ideas and conversations are allowed to flow." +
                                        "\nThat's why our terms of service are very simple:" +
                                        "\n***Don't harm the hardware used to run our servers!***" +
                                        "\n Otherwise have fun!")
                .WithReaction(new Emoji("✅"));
            
            
            DnD = Room.CreateGuildRoom("DnD", Screen)
                .WithStaticDescriptions("all stuff DnD, tolken, ttrp!")
                .WithObjects(
                   
                    );
            
            Pokemon = Room.CreateGuildRoom("Pokemon", Screen)
                .WithStaticDescriptions("Gotta catch em' all! Pokemon Memes go here.");
            
            Animals = Room.CreateGuildRoom("Cute Animals", Screen)
                .WithStaticDescriptions("Dogs chasing their tails, cucumbers scaring cats, tiny frogs making impressive noises -- we love them all!\nPreferably .gifs, so we can see them in action!");

            
            Office = Room.CreateGuildRoom("Office", TheCloud)
                .WithStaticDescriptions("there is a fancy office space")
                .WithObjects(
                );
            
            #endregion
            
            Rooms = ReflectionHelpers.ClassMembersToArray<Room>(typeof(RoomManager), this);
            Categories = ReflectionHelpers.ClassMembersToArray<RoomCategory>(typeof(RoomManager), this);

            #region tie channels and categories to their discord entities (by creating them), and put channels in their proper categories
        
            //todo could make this faster by immediately creating the rooms of a category channel when it's done
            //todo instead of wait for ALL the categories to be done and then creating text channels
            
            
            #region clean slate
            Task [] deleteTasks = new Task[guild.Channels.Count];
            int index = 0;
            foreach (var channel in guild.Channels)
            {
                deleteTasks[index] = channel.DeleteAsync();
                index++;
            }
            #endregion

            Task<RestCategoryChannel> [] createCategoriesTasks = new Task<RestCategoryChannel>[Categories.Length];
            for (int i = 0; i < Categories.Length; i++)
                createCategoriesTasks[i] = guild.CreateCategoryChannelAsync(Categories[i].Name);
            
            Task.WaitAll(createCategoriesTasks);
            
            
            //create categories and link them to discord
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

            //wait for creation of text channel categories
            var createChannelTasksArr = createChannelTasks.ToArray();
            Task.WaitAll(createChannelTasksArr);
            
            //init room, tying them to discord and the phrase responses
            RoomKV = new Dictionary<ulong, Room>(Rooms.Length);
            int createChannelTaskIndex = 0;
            for (int i = 0; i < Categories.Length; i++)
            {
                for (int j = 0; j < Categories[i].Rooms.Count; j++)
                {
                    var channel = createChannelTasksArr[createChannelTaskIndex].Result;
                    RestGuildChannel? guildChannel = channel; //will be null if dm channel
                    if (guildChannel == null && !Categories[i].Rooms[j].IsDMChannel)
                        throw new Exception("Inconsistent DM usage");

                    var room = Categories[i].Rooms[j];
                    room.InitAndDraw(session, createChannelTasksArr[createChannelTaskIndex].Result, guildChannel);
                    RoomKV.Add(room.MessageChannel!.Id, room);
                    createChannelTaskIndex++;
                }
                
            }
            
            Screen.ChangeRoomVisibilityAsync(session, OverwritePermissions.DenyAll(Screen.Channel));
            TheCloud.ChangeRoomVisibilityAsync(session, OverwritePermissions.DenyAll(TheCloud.Channel));
            
            #endregion

            
            Animals.Renderer.Builder.ImageUrl = "https://miro.medium.com/max/11520/0*pAypSD1ZSCCw0NcL";
            Pokemon.Renderer.Builder.ImageUrl = "https://i.guim.co.uk/img/media/66e444bff77d9c566e53c8da88591e4297df0896/120_0_1800_1080/master/1800.png?width=1200&height=1200&quality=85&auto=format&fit=crop&s=69b22b4292160faf91cb45ad024fc649";
            DnD.Renderer.Builder.ImageUrl = "https://cdn.shopify.com/s/files/1/1634/0113/products/AgedMithiralBoulder_1500x.jpg?v=1602599762";

            Animals.Renderer.DrawRoomStateEmbed();
            Pokemon.Renderer.DrawRoomStateEmbed();
            DnD.Renderer.DrawRoomStateEmbed();
            
            Animals.MessageChannel.SendMessageAsync("https://tenor.com/view/busu05-funny-animals-gif-8130594");
            Animals.MessageChannel.SendMessageAsync("https://tenor.com/view/shummer-netflix-cat-cute-gif-12702077");
            
            Pokemon.MessageChannel.SendMessageAsync("https://tenor.com/view/jigglypuff-singing-karaoke-sleep-put-to-sleep-gif-13598825");
            Pokemon.MessageChannel.SendMessageAsync("https://tenor.com/view/your-argument-is-invalid-gif-11766814");
            
            DnD.MessageChannel.SendMessageAsync("https://tenor.com/view/tiktok-cat-dungeon-master-dnd-dm-gif-16454081");
            DnD.MessageChannel.SendMessageAsync("https://tenor.com/view/lilo-pelekai-im-sorry-lilo-and-stitch-apologize-gif-8930173");

        }

        // public void ConnectRoomsToMessageChannels(SocketGuild guild)
        // {
        //     //guild.CreateCategoryChannelAsync()
        // }
   
    }
}