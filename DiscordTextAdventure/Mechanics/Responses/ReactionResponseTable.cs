using System;
using System.Collections.Generic;
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
        public readonly static ReactionResponse AcceptUserAgreement;
        public readonly static ReactionResponse AttemptVoidUserAgreement;
        public readonly static ReactionResponse AcceptInvitation;
        
        public readonly static List<ReactionResponse> OnReactionAddedResponseEvents;
        public readonly static List<ReactionResponse> OnReactionRemovedResponseEvents;
    

        static ReactionResponseTable()
        {
            #region signatures

            IEmote checkmark = new Emoji("✅");
            AcceptUserAgreement = new ReactionResponse(checkmark, ReactionResponse.OnReactionTrigger.OnAdd, null, SetPlayerAndCreateDMChannelsAsync);
            AttemptVoidUserAgreement = new ReactionResponse(checkmark, ReactionResponse.OnReactionTrigger.OnRemove, null, AttemptVoidAgreementAsync);
            
            AcceptInvitation = new ReactionResponse(new Emoji("🎉"), ReactionResponse.OnReactionTrigger.OnAdd, null, AcceptInvitationAction);
            
            System.Threading.Timer timer;

            #endregion


            #region response logic

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
                                room.InitAndDraw(e.Session, task.Result);
                                e.Session.RoomManager.RoomKV.Add(room.RoomOwnerChannel!.Id, room);
                                task.Result.SendMessageAsync(
                                    $"Welcome {e.Session.Player.User.Username}!" +
                                    $"\nThese are exciting times in which you're entering Dissonance server, as we have a special event currently taking place!." +
                                    $"\nThe user with the most contributions to our community, will have the privilege to visit *The Cloud*. " +
                                    $"\nOur headquarters where you'll get to me out ambitious crew and have a chance to get to know our cutting edge technology." +
                                    $"\nMake sure to participate! We recommend getting to know each one of our communities and posting relevant content!");
                            });


                    //requires intents
                    var bodyDMTask = e.Session.BodyBot.GetUser(userId).GetOrCreateDMChannelAsync().ContinueWith(task =>
                    {
                        var room = e.Session.RoomManager.BodyDM;
                        room.InitAndDraw(e.Session, task.Result);
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
                        room.InitAndDraw(e.Session, task.Result);
                        e.Session.RoomManager.RoomKV.Add(room.RoomOwnerChannel!.Id, room);
                    });

                    await dissonanceDMTask;
                    await bodyDMTask;
                    await memeDMTask;
                }
            }
            
            async Task BodyMessage01(IMessageChannel channel)
            {
                channel.SendMessageAsync(
                    $"You sit down at the computer, shoulder's slouched forward, neck craned." +
                    $"\nExcitement trickles through your veins, as you begin scrolling...");
                timer = new System.Threading.Timer((args) => BodyMessage02(channel), null, 15_000, -1);
            }
   
            async Task BodyMessage02(IMessageChannel channel)
            {
                channel.SendMessageAsync($"You're neck is killing."+
                    "\nGet some rest rest, your vision is blurring");
                timer = new System.Threading.Timer((args) => BodyMessage03(channel), null, 20_000, -1);
            }
            
            async Task BodyMessage03(IMessageChannel channel)
            {
                channel.SendMessageAsync($"You have to find a way off of this server." +
                                         $"\nLeaving won't be enough, I know you, and I know that you'll become addicted, you are addicted." +
                                         $"\nLeaving won't even be enough, you won't be able to help but come back, for your daily dose of DnD memes, funny animal content, and news on the upcoming Pokemon games." +
                                         $"\nYou have to find a way to get banned, so you can't come back. " +
                                         $"\nI need to go for a walk, we're falling apart." +
                                         $"\nFind a way to get banned.");
                timer = null;
            }
            
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
            
            #region sort by triggers and add to static lists
            var allReactionResponses = ReflectionHelpers.ClassMembersToArray<ReactionResponse>(typeof(ReactionResponseTable), null);
            
            OnReactionAddedResponseEvents = new List<ReactionResponse>(allReactionResponses.Length/2);
            OnReactionRemovedResponseEvents = new List<ReactionResponse>(allReactionResponses.Length/2);
            
            for (int i = 0; i < allReactionResponses.Length; i++)
            {
                var s = allReactionResponses[i].Trigger & ReactionResponse.OnReactionTrigger.OnAdd;
                if ((allReactionResponses[i].Trigger & ReactionResponse.OnReactionTrigger.OnAdd) == ReactionResponse.OnReactionTrigger.OnAdd)
                
                    OnReactionAddedResponseEvents.Add(allReactionResponses[i]);

                if ((allReactionResponses[i].Trigger & ReactionResponse.OnReactionTrigger.OnRemove) == ReactionResponse.OnReactionTrigger.OnRemove)
                    OnReactionRemovedResponseEvents.Add(allReactionResponses[i]);
            }
            
            #endregion
        }
    }
}