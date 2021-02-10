using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using chext;
using chext.Mechanics;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Responses;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing.DataStructures;
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
        public readonly Room Megan;

        public readonly Room Servers;
        public readonly Room ControlRoom;

        public readonly RoomCategory Intro;
        public readonly RoomCategory Screen;
        public readonly RoomCategory TheCloud;
        public readonly RoomCategory TheFarm;
        #endregion

        public RoomManager(Session session, PhraseResponseTable table, SocketGuild guild)
        {
            #region dms

            DissonanceDM = Room.CreateDMRoom(false);

            BodyDM = Room.CreateDMRoom(true).WithSubtitle("It's ours, warts and all")
                .WithStaticDescriptions(
                    "Things have been better. We should probably drink more water, or go for a walk... pretty much anything is better than sitting at a computer all day.")
                .WithObjects(
                    new AdventureObject(NounTable.Arms, "All this typing can't be good for the wrists, we should look into getting a better mouse pad or wrist brace.", true)
                        .WithInspectDefault(table)
                        .WithPickupNoSense(table)
                        .WithCannotDamage(table, "We smash the arms on the table, the sharp pain on impact fades into a dull ache."),
                    
                    new AdventureObject(NounTable.Legs, "The knees ache from being bent so long", true)
                        .WithInspectDefault(table)
                        .WithCannotPickup(table)
                        .WithCannotDamage(table, "We punch our thighs, it leaves a bruise."),
                
                    new AdventureObject(NounTable.Head, "We can't see the head, but we do take note of growing pain of the headphones on its ears. Forcing itself into conscious thought.", true)
                        .WithInspectDefault(table)
                        .WithPickupNoSense(table)
                        .WithCannotDamage(table, "We're not going to do that. We really do need the head, and that is not a sincere solution to our problems.")
                );


                MemeDM = Room.CreateDMRoom(false);
            #endregion
            
            Intro = new RoomCategory("Welcome");
            Screen = new RoomCategory("Screens");
            TheCloud = new RoomCategory("The Cloud");
            TheFarm = new RoomCategory("The Farm");
            
            
            #region useragreement
            UserAgreement = Room.CreateGuildRoom("User Agreement", Intro)
                .WithStaticDescriptions("Here at Dissonance we believe in free communication and sharing." +
                                        "\nCommunities are stronger when ideas and conversations are allowed to flow." +
                                        "\nThat's why our terms of service are very simple:" +
                                        "\n > ***Don't damage the hardware used to run our servers and store our user data!***" +
                                        "\n Otherwise have fun!")
                .WithReactions(new Emoji("✅"));
            #endregion
            
            #region the screens
            DnD = Room.CreateGuildRoom("DnD", Screen)
                .WithStaticDescriptions("All stuff DnD, TTRP, and high fantasy.\nGrab a beer a join us at the pub!\nYour quest awaits!\nAvailable Roles: Dwarf.")
                .WithReactions(new Emoji("🪓"))
                .WithObjects(
                   
                    )
                ;

            Pokemon = Room.CreateGuildRoom("Pokemon", Screen)
                    .WithStaticDescriptions("Gotta catch em' all!\nWe mean the memes, give us your Poké memes now.\nAvailable roles: Magikarp.")
                    .WithReactions(new Emoji("🐠"))
                ;

            Animals = Room.CreateGuildRoom("Cute Animals", Screen)
                    .WithStaticDescriptions(
                        "Dogs chasing their tails, cucumbers scaring cats, tiny frogs making impressive noises -- we love them all!\nPreferably .gifs, so we can see them in action!\nAvailable Roles: Cat.")
                    .WithReactions(new Emoji("🐱"))
                ;
            #endregion

            #region the office
            Office = Room.CreateGuildRoom("Office", TheCloud)
                .WithStaticDescriptions(
                    "The feng shui of space is decided by a full sized, tubular slide which protrudes through the myriad of pipes and vents on the ceiling, coming to rest in the center of the room. On one side of the room, a glass fridge with a big cursive label \"Organic\" houses mason jars with green juices and smoothies. Another wall is covered in flags, all of them national, except for a larger flag in the center which wields the company logo instead. Bean bag chairs litter the hardwood. A tarp with *Pepe The Frog* printed on it is draped over a pedestal next to the slide’s entrance.")
                .WithObjects(
                    new AdventureObject(
                        NounTable.Tarp,
                        "The tarp is clearly over top of something. Odd that a Pepe meme is printed on the fabric...")
                        .WithInspectDefault(table)
                        .WithCannotDamage(table, "Let's not be violent needlessly")
                        .OnPickup(table, 
                            e =>
                        {
                            var room = e.RoomOfPhrase;
                            
                            if (room.TryFindFirstObject(NounTable.Tarp) == null)
                                return;
                            
                            room.DissoanceChannel.SendMessageAsync(
                                "You take off the tarp to reveal a metal box with a small screen, engraved on its chassis: \"⚙ The Meme Machine  ⚙, by Dissonance R&D\"");        
                            room.MemeChannel.SendMessageAsync("👋");

                            var tarp = room.TryFindFirstObject(NounTable.Tarp);
                            if (tarp != null)
                            {
                                room.Objects.Remove(tarp);
                            }
                            
                            room.Objects.Add(new AdventureObject(NounTable.MemeBot, 
                                "A small box, you get the feeling it's listening to you.")
                                .WithCannotPickup(table)
                                .WithInspectDefault(table)
                                .WithCannotDamage(table, "This isn't where user data is stored, this won't get us banned... it seems pretty sturdy either way.\n"));
                            
                            room.Renderer.DrawRoomStateEmbed();

                        })
                    
                )
                ;
            


            
            Megan = Room.CreateGuildRoom("Megan's Office", TheCloud)
                .WithStaticDescriptions(
                    "A person in a striped suit is working on a macbook in the center of a small office space. They're forced awkwardly into a fetus position by a overly spongy beanbag chair.")
                .WithObjects(
                    new AdventureObject(
                        NounTable.Megan,
                        "I feel awkward staring, I should say something to break the silence.")
                        .WithCannotDamage(table, "I'm not going to hurt a person... It wouldn't even break the user-agreement")
               
                );

            #endregion
            
            #region the farm
            Servers = Room.CreateGuildRoom("Servers", TheFarm)
                .WithStaticDescriptions(
                    "A huge warehouse. In every direction, long black aisles of server server racks, little indicator lights beeping. The ceiling isn't visible above the countless multi-coloured cables. The hum of hundreds of cooling fans and ACs combine to generate a deafening roar.")
                .WithObjects(
                    new AdventureObject(
                            NounTable.ServerRacks,
                            "An array of computer racks, where Dissonance stores their user data and servers to run their services")
                        .WithDamageCustom(table, damageServer));

            ControlRoom = Room.CreateGuildRoom("Routing", TheFarm)
                .WithStaticDescriptions(
                    "A darkly lit room, in its center lies a metal cabinet. Thousands of wires enter the box from its top and bottom, packed tightly together, forming a sort of trunk. All connecting to a large modem which connects The Farm to the larger internet. There's a small, purple LCD screen on the cabinet which reads \"Firewall Password\".")
                .WithReactions(ReactionResponseTable.BlueSquare, ReactionResponseTable.RedSquare)
                .WithObjects(
                    new AdventureObject(
                            NounTable.Panel,
                            "There doesn't seem to be any accessories and the purple panel isn't a touch screen, there must be some other way to interact with the modem's firewall.")

                        .WithCannotDamage(table));

            void damageServer(PhraseResponseEventArgs e)
            {
                switch (e.Session.Player.Role)
                {
                    case Player.RoleTypes.Cat:
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("Paws scratch at the racks uselessly.");
                        e.RoomOfPhrase.BodyChannel.SendMessageAsync(
                            "Chipped paint won't count as *hardware damage*, we need a more appropriate, stronger form to do any real damage");
                        break;
                    case Player.RoleTypes.Human:
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("You punch and kick at the racks, they don't seem to flinch.");
                        e.RoomOfPhrase.BodyChannel.SendMessageAsync(
                            "We're going to break something before any real damage is done, we need a stronger form to do some real damage");
                        break;
                    case Player.RoleTypes.Dwarf:
                        e.Session.RoomManager.Screen.ChangeRoomVisibilityAsync(e.Session, OverwritePermissions.DenyAll(e.Session.RoomManager.Screen.Channel));
                        e.Session.RoomManager.TheCloud.ChangeRoomVisibilityAsync(e.Session, OverwritePermissions.DenyAll(e.Session.RoomManager.TheCloud.Channel));
                        e.Session.RoomManager.TheFarm.ChangeRoomVisibilityAsync(e.Session, OverwritePermissions.DenyAll(e.Session.RoomManager.TheFarm.Channel));
                        e.Session.RoomManager.Intro.ChangeRoomVisibilityAsync(e.Session, OverwritePermissions.DenyAll(e.Session.RoomManager.Intro.Channel));
                        
                        e.Session.RoomManager.DissonanceDM.RoomOwnerChannel.SendMessageAsync(
                            $"```On {DateTime.Now.ToLongTimeString()}, your discord account, associated with the username {e.Session.Player.User.Username}, with UID {e.Session.Player.User.Discriminator}, broke into The Farm's facilities. An axe was used to hack into server rack 0A4B, leading to dents, rips and tearings in the metal caging, revealing wires and bare PCB to open air. These actions constitute damage to company hardware and a threat to client data which may have been corrupted and/or lost over the course of these events. This behavior represents a breakage of the Dissonance user-agreement as determined by Dissonance's legal team. You are henceforth banned from Dissonance's services including, but not limited to: the discord server run and operated by Dissonance corporation.\nReact to the ⛔ below to acknowledge.```").ContinueWith(
                            task =>
                            {
                                task.Result.AddReactionAsync(new Emoji("⛔"));
                            });
                        break;
                }
            }
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
            Task<RestTextChannel> [] createChannelTasksArr = createChannelTasks.ToArray();
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
                    room.InitAndDraw(session, createChannelTasksArr[createChannelTaskIndex].Result, this);
                    RoomKV.Add(room.RoomOwnerChannel!.Id, room);
                    createChannelTaskIndex++;
                }
                
            }
            
            Screen.ChangeRoomVisibilityAsync(session, RoomCategory.NothingPermission);
            TheCloud.ChangeRoomVisibilityAsync(session, RoomCategory.NothingPermission);
            TheFarm.ChangeRoomVisibilityAsync(session, RoomCategory.NothingPermission);

            #endregion

            #region static gifs
            Animals.Renderer.Builder.ImageUrl = "https://miro.medium.com/max/11520/0*pAypSD1ZSCCw0NcL";
            Pokemon.Renderer.Builder.ImageUrl = "https://i.guim.co.uk/img/media/66e444bff77d9c566e53c8da88591e4297df0896/120_0_1800_1080/master/1800.png?width=1200&height=1200&quality=85&auto=format&fit=crop&s=69b22b4292160faf91cb45ad024fc649";
            DnD.Renderer.Builder.ImageUrl = "https://cdn.shopify.com/s/files/1/1634/0113/products/AgedMithiralBoulder_1500x.jpg?v=1602599762";

            Animals.Renderer.DrawRoomStateEmbed();
            Pokemon.Renderer.DrawRoomStateEmbed();
            DnD.Renderer.DrawRoomStateEmbed();
            
            Animals.RoomOwnerChannel.SendMessageAsync("https://tenor.com/view/busu05-funny-animals-gif-8130594");
            Animals.RoomOwnerChannel.SendMessageAsync("https://tenor.com/view/shummer-netflix-cat-cute-gif-12702077");
            
            Pokemon.RoomOwnerChannel.SendMessageAsync("https://tenor.com/view/jigglypuff-singing-karaoke-sleep-put-to-sleep-gif-13598825");
            Pokemon.RoomOwnerChannel.SendMessageAsync("https://tenor.com/view/your-argument-is-invalid-gif-11766814");
            
            DnD.RoomOwnerChannel.SendMessageAsync("https://tenor.com/view/tiktok-cat-dungeon-master-dnd-dm-gif-16454081");
            DnD.RoomOwnerChannel.SendMessageAsync("https://tenor.com/view/lilo-pelekai-im-sorry-lilo-and-stitch-apologize-gif-8930173");
            #endregion
            


        }

        public void MoveAdventureObject(AdventureObject obj, Room target)
        {
            // bool removed = RoomKV.Remove(obj.CurrentRoom.MessageChannel.Id);
            // if (!removed)
            //     Program.DebugLog("no room removed from moveadventureobject, probably a bug");
            
            //RoomKV.ad
            obj.CurrentRoom.Objects.Remove(obj);
            obj.CurrentRoom = target;
            target.Objects.Add(obj);
        }

        // public void ConnectRoomsToMessageChannels(SocketGuild guild)
        // {
        //     //guild.CreateCategoryChannelAsync()
        // }
   
    }
}