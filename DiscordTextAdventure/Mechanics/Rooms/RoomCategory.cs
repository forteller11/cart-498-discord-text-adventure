﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext.Mechanics;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Rooms
{
    public class RoomCategory
    {
        public string Name;
        public RestCategoryChannel? Channel;
        public List<Room> Rooms;


        // public static readonly ChannelPermission ViewAndSendPermission = ChannelPermission.Connect
        //                                                                  | ChannelPermission.AddReactions
        //                                                                  | ChannelPermission.DeafenMembers
        //                                                                  | ChannelPermission.SendMessages
        //                                                                  | ChannelPermission.ViewChannel;
        //
        // public static readonly ChannelPermission NothingPermission = 0; 
        
        public static readonly OverwritePermissions ViewAndSendPermission = new OverwritePermissions(
            addReactions: PermValue.Allow,
            connect: PermValue.Allow,
            sendMessages: PermValue.Allow,
            viewChannel: PermValue.Allow
            );
        
        public static readonly OverwritePermissions NothingPermission = new OverwritePermissions(
            addReactions: PermValue.Deny,
            connect: PermValue.Deny,
            sendMessages: PermValue.Deny,
            viewChannel: PermValue.Deny

        );
        
        
        public RoomCategory(string name)
        {
            Name = name;
            Rooms = new List<Room>();
        }

        public void LinkToDiscord(RestCategoryChannel channel)
        {
            Channel = channel;
        }

        public async Task ChangeRoomVisibilityAsync(Session session, OverwritePermissions overwritePermissions)
        {
    
            await Channel.AddPermissionOverwriteAsync(session.Guild.EveryoneRole, overwritePermissions);
            
            // if (session.Player == null)
            // {
            //     foreach (var user in session.Guild.Users)
            //     {
            //         if (!user.IsBot)
            //         {
            //             Program.DebugLog("Permission overwrite no player");
            //             
            //             await Channel.AddPermissionOverwriteAsync(session.Guild.EveryoneRole, overwritePermissions);
            //         }
            //     }
            // }
            // else
            // {
            //     Program.DebugLog("Permission overwrite category");
            //     await Channel.AddPermissionOverwriteAsync(session.Guild.EveryoneRole, overwritePermissions);
            // }
            
        }
    }
}