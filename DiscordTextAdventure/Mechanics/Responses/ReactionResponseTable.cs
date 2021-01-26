﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ReactionResponseTable
    {
        public readonly static ReactionResponse AcceptUserAgreement;
        public readonly static ReactionResponse AttemptVoidUserAgreement;
        
        public readonly static List<ReactionResponse> OnReactionAddedResponseEvents;
        public readonly static List<ReactionResponse> OnReactionRemovedResponseEvents;
        
        static ReactionResponseTable()
        {
            #region signatures
            
            IEmote checkmark = new Emoji("✅");
            AcceptUserAgreement      = new ReactionResponse(checkmark, ReactionResponse.OnReactionTrigger.OnAdd,    null, SetPlayer );
            AttemptVoidUserAgreement = new ReactionResponse(checkmark, ReactionResponse.OnReactionTrigger.OnRemove, null, AttemptVoidAgreement );
            
            #endregion
            
            
            #region response logic
            async Task SetPlayer(ReactionResponseEventArgs e)
            {
                if (e.Session.Player == null || e.Session?.Player.User.Id != e.User.Id)
                {
                    e.Session.Player = new Player(e.User);
                    e.Session.RoomManager.Screen.ChangeRoomVisibilityAsync(e.Session,
                        RoomCategory.ViewAndSendPermission);
                    var dmChannel = await  e.Session.Player.SocketUser.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync(
                        $"Welcome { e.Session.Player.User.Username}!" +
                        $"\nThese are exciting times in which you're entering Dissonance server, as we have a special event currently taking place!." +
                        $"\nThe user with the most contributions to our community, will have the privilege to visit *The Cloud*. " +
                        $"\nOur headquarters where you'll get to me out ambitious crew and have a chance to get to know our cutting edge technology." +
                        $"\nMake sure to participate! We recommend getting to know each one of our communities and posting relevant content!");
                    //you can read about more here: 404
                }
            }

            async Task AttemptVoidAgreement(ReactionResponseEventArgs e)
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
            
            #endregion
            
            #region sort by triggers and add to static lists
            var allReactionResponses = Common.ClassMembersToArray<ReactionResponse>(typeof(ReactionResponseTable), null);
            
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