using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using chext.Mechanics;
using Discord;
using Discord.Rest;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Reflection;
using Timer = System.Timers.Timer;

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ReactionResponseTable
    {
        public readonly ReactionResponse AcceptUserAgreement;
        public readonly ReactionResponse AttemptVoidUserAgreement;
        public readonly ReactionResponse AcceptInvitation;
        
        public readonly ReactionResponse[] ReactionResponses;

        public readonly ReactionResponse CatRole;
        public readonly ReactionResponse DwarfRole;
        public readonly ReactionResponse MagikarpRole;

        public GuildEmote FarmEmote { get; private set; }
        
        public ReactionResponseTable(Session session)
        {
            System.Threading.Timer timer;
            
            
            #region get or create emote

            bool isThereAFarmEmote = false;
            foreach (var emote in session.Guild.Emotes)
            {

                if (emote.Name == "the_farm")
                {
                    isThereAFarmEmote = true;
                    FarmEmote = emote;
                    break;
                }
            }

            if (!isThereAFarmEmote)
            {
                throw new DataException("Must upload the_farm emote to server!!!!");
                // var image = new Image(Program.AssetsPath + "/farm_emote.png");
                // session.Guild.CreateEmoteAsync("the_farm", image).ContinueWith(
                //     task =>
                //     {
                //         FarmEmote = task.Result;
                //     });
            }
            #endregion
            #region intro
            IEmote checkmark = new Emoji("✅");
            AcceptUserAgreement = new ReactionResponse(new ReactionBlueprint(checkmark, ReactionBlueprint.OnReactionTrigger.OnAdd), null, SetPlayerAndCreateDMChannelsAsync);
            AttemptVoidUserAgreement = new ReactionResponse(new ReactionBlueprint(checkmark, ReactionBlueprint.OnReactionTrigger.OnRemove), null, AttemptVoidAgreementAsync);
            AcceptInvitation = new ReactionResponse(new ReactionBlueprint(new Emoji("🎉"), ReactionBlueprint.OnReactionTrigger.OnAdd), null, AcceptInvitationAction);
            #endregion

            #region the screens

            CatRole = new ReactionResponse(new ReactionBlueprint(new Emoji( "🐱"), ReactionBlueprint.OnReactionTriggerBoth), AddRoleCat, null);
            DwarfRole = new ReactionResponse(new ReactionBlueprint(new Emoji( "🪓"), ReactionBlueprint.OnReactionTriggerBoth), AddRoleDwarf, null);
            MagikarpRole = new ReactionResponse(new ReactionBlueprint(new Emoji( "🐠"), ReactionBlueprint.OnReactionTriggerBoth), null, AddRoleMagikarp);

            #endregion


            #region response intro

            async Task SetPlayerAndCreateDMChannelsAsync(ReactionResponseEventArgs e)
            {
                if (e.Session.Player == null || e.Session?.Player.User.Id != e.User.Id)
                {
                    var guildUserId = e.User.Id;
                    ulong userId = e.SocketReaction.UserId;
                    e.Session.Player = new Player(e.User);

                    e.Session.RoomManager.Screen.ChangeRoomVisibilityAsync(e.Session,
                        RoomCategory.ViewAndSendPermission);

                    var user = e.Session.Guild.GetUser(userId);
                    var dissonanceDMTask = e.Session.DissonanceBot.GetUser(userId).GetOrCreateDMChannelAsync()
                        .ContinueWith(
                            task =>
                            {
                                var room = e.Session.RoomManager.DissonanceDM;
                                room.InitAndDraw(e.Session, task.Result, e.Session.RoomManager);
                                e.Session.RoomManager.RoomKV.Add(room.RoomOwnerChannel!.Id, room);
                                task.Result.SendMessageAsync(
                                    $"Welcome {e.Session.Player.User.Username}!" +
                                    $"\nThese are exciting times in which you're entering Dissonance server, as we have a special event currently taking place!." +
                                    $"\nThe user with the most contributions to our community, will have the privilege to visit ***The Cloud***. " +
                                    $"\nOur headquarters where you'll get to me out ambitious crew and have a chance to get to know our cutting edge technology." +
                                    $"\nMake sure to participate! We recommend getting to know each one of our communities and posting relevant content!");
                            });
                    

                //requires intents
                    var bodyDMTask = e.Session.BodyBot.GetUser(userId).GetOrCreateDMChannelAsync().ContinueWith(task =>
                    {
                        var room = e.Session.RoomManager.BodyDM;
                        room.InitAndDraw(e.Session, task.Result, e.Session.RoomManager);
                        e.Session.RoomManager.RoomKV.Add(room.RoomOwnerChannel!.Id, room);

                        timer = new System.Threading.Timer((args) => BodyMessage01(room.RoomOwnerChannel), null, 10_000, -1);

                        e.Session.BodyBot.MessageReceived += (args) =>
                        {
                            //only listen for dm channel
                            if (args.Channel.Id == e.Session.RoomManager.BodyDM.RoomOwnerChannel.Id)
                                return e.Session.OnMessageReceived(args);

                            return Task.CompletedTask;
                        };
                    });

                    var memeDMTask = e.Session.MemeBot.GetUser(userId).GetOrCreateDMChannelAsync().ContinueWith(task =>
                    {
                        var room = e.Session.RoomManager.MemeDM;
                        room.InitAndDraw(e.Session, task.Result, e.Session.RoomManager);
                        e.Session.RoomManager.RoomKV.Add(room.RoomOwnerChannel!.Id, room);
                        Program.DebugLog("Meme task");
                    });

                    await dissonanceDMTask;
                    await bodyDMTask;
                    await memeDMTask;
                }
            }
            
            #region body intro messages
            async Task BodyMessage01(IMessageChannel channel)
            {
                channel.SendMessageAsync(
                    $"We sit down at the computer, shoulder's slouched forward, neck craned." +
                    $"\nExcitement trickles through your veins, as we begin scrolling...");
                timer = new System.Threading.Timer((args) => BodyMessage02(channel), null, 20_000, -1);
            }
   
            async Task BodyMessage02(IMessageChannel channel)
            {
                channel.SendMessageAsync($". . ." +
                                         $"\nThe neck is killing."+
                    "\nGet some rest rest, vision is blurring");
                timer = new System.Threading.Timer((args) => BodyMessage03(channel), null, 20_000, -1);
            }
            
            async Task BodyMessage03(IMessageChannel channel)
            {
                channel.SendMessageAsync($". . ." +
                                         $"\nYou have to find a way off of this server." +
                                         $"\nLeaving won't be enough, I know us, we'll become addicted, we are addicted." +
                                         $"\nLeaving won't even be enough, we won't be able to help but come back, for our daily dose of DnD memes, funny animal content, and news on the upcoming Pokemon games." +
                                         $"\nWe have to find a way to get banned, so we can't come back. " +
                                         $"\nWe need to go for a walk, we're falling apart." +
                                         "\n" +
                                         $"\n > **Find a way to get banned.**");
                
                timer = null;
            }
            
            #endregion
            
            async Task AttemptVoidAgreementAsync(ReactionResponseEventArgs e)
            {
                if (e.Session.Player?.User.Id == e.User.Id) //player already not null and player is the user who reacted
                {
                    var player = e.Session.Player;
                    var dmChannel = await e.Session.Player.SocketUser.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync(
                        $"```diff" +
                        $"\n=======================================================================================" +
                        $"\n{player.User.Username}, it looks like you are trying to VOID the user-agreement you accepted" +
                        $"\nat {player.AcceptUserAgreement.ToLocalTime().ToLongTimeString()} on {player.AcceptUserAgreement.ToShortDateString()}." +
                        $"\nWe regret to inform you you can not revoke these terms of service." +
                        $"\nYour data may be stored in our servers and third party affiliates indefinitely.\n" +
                        "=======================================================================================" +
                        "" +
                        "\n\n- The Dissonance Legal Team```");
                }
            }
            
            async Task AcceptInvitationAction(ReactionResponseEventArgs e)
            {
                e.Session.RoomManager.TheCloud.ChangeRoomVisibilityAsync(e.Session, RoomCategory.ViewAndSendPermission);

            }
            
            
            #endregion
            
            #region response screen

            bool ShouldContinueWithAddRole(ReactionResponseEventArgs e, Room roomFilter)
            {
                if (!e.IsAdd)
                {
                    AddRoleHuman(e);
                    return false;
                }

                if (e.PostedRoom != roomFilter)
                {
                    return false;
                }

                return true;
            }
            void AddRoleCat(ReactionResponseEventArgs e)
            {
                if (!ShouldContinueWithAddRole(e, e.Session.RoomManager.Animals))
                    return;
                
                e.Session.Player.Role = Player.RoleTypes.Cat;
                e.PostedRoom.BodyChannel.SendMessageAsync(
                    "The back is brought forward, spine extending into a tail." +
                    "\nSkin enveloped in fur" +
                    "\nSenses become clearer." +
                    "\nA sudden urge for a bowl of milk." +
                    "\nWe're a cat.");
            }
            
            void AddRoleDwarf(ReactionResponseEventArgs e)
            {
                if (!ShouldContinueWithAddRole(e, e.Session.RoomManager.DnD))
                    return;
                
                e.Session.Player.Role = Player.RoleTypes.Dwarf;
                e.PostedRoom.BodyChannel.SendMessageAsync(
                    "A red braid emerges from the chin." +
                    "\nAn axe appears in the dominant hand." +
                    "\nA huge snoz dominates our visage" +
                    "\nWe're a dwarf.");
            }
            
            async Task AddRoleMagikarp(ReactionResponseEventArgs e)
            {
                if (!ShouldContinueWithAddRole(e, e.Session.RoomManager.Pokemon))
                {
                    e.Session.RoomManager.Intro.ChangeRoomVisibilityAsync(session, OverwritePermissions.AllowAll( e.Session.RoomManager.Intro.Channel));
                    
                    e.Session.RoomManager.DnD.ChangeRoomVisibilityAsync(session, OverwritePermissions.AllowAll(e.Session.RoomManager.DnD.RoomOwnerChannel));
                    e.Session.RoomManager.Animals.ChangeRoomVisibilityAsync(session, OverwritePermissions.AllowAll(e.Session.RoomManager.Animals.RoomOwnerChannel));
                    return;
                }

                e.Session.Player.Role = Player.RoleTypes.Magikarp;
                await e.PostedRoom.BodyChannel.SendMessageAsync(
                    "Smooth red scales shoot up from the skin" +
                    "\nWe fall onto the floor as legs dissolve into a fin." +
                    "\nLips fade into bone" +
                    "\nWe flop uselessly on the floor, breaths become painful, and mobility is hopeless." +
                    "\nWe're a Magikarp");
                
                e.PostedRoom.DissoanceChannel.SendMessageAsync("It's not very effective!");
                
                
                e.Session.RoomManager.Intro.ChangeRoomVisibilityAsync(session, OverwritePermissions.DenyAll( e.Session.RoomManager.Intro.Channel));
                
                e.Session.RoomManager.DnD.ChangeRoomVisibilityAsync(session, OverwritePermissions.DenyAll(e.Session.RoomManager.DnD.RoomOwnerChannel));
                e.Session.RoomManager.Animals.ChangeRoomVisibilityAsync(session, OverwritePermissions.DenyAll(e.Session.RoomManager.Animals.RoomOwnerChannel));
            
                
            }

            

            void AddRoleHuman(ReactionResponseEventArgs e)
            {
                e.Session.Player.Role = Player.RoleTypes.Human;
                e.PostedRoom.BodyChannel.SendMessageAsync(
                    "A refreshing sense of normalcy has returned, but many aches with it." +
                    "\nWe are human again.");
            }
            #endregion
            #region sort by triggers and add to static lists
            ReactionResponses = ReflectionHelpers.ClassMembersToArray<ReactionResponse>(typeof(ReactionResponseTable), this);
            
            
            
            #endregion
        }
    }
}