using System;
using System.Collections.Generic;
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
        public OverwritePermissions OverwritePermissions;

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
        
        
        public RoomCategory(string name, OverwritePermissions permissions)
        {
            Name = name;
            Rooms = new List<Room>();
            OverwritePermissions = permissions;
        }

        public void LinkToDiscord(RestCategoryChannel channel)
        {
            Channel = channel;
        }

        public void ChangeRoomVisibilityAsync(Session session, OverwritePermissions overwritePermissions)
        {
            OverwritePermissions = overwritePermissions;
            
            for (int i = 0; i < Rooms.Count; i++)
            {
                if (Rooms[i].IsDMChannel)
                    throw new Exception("Can't change visibility of dm channel!");


                if (session.Player == null)
                {
                    foreach (var user in session.Guild.Users)
                    {
                        if (!user.IsBot)
                        {
                            Rooms[i].GuildChannel!.AddPermissionOverwriteAsync(user, overwritePermissions);
                            Channel.AddPermissionOverwriteAsync(user, overwritePermissions);
                            Program.DebugLog("Permission overwrite");
                        }
                    }
                }
                else
                {
                    Program.DebugLog("Permission overwrite");
                    Channel.AddPermissionOverwriteAsync(session.Player.User, overwritePermissions);
                    Rooms[i].GuildChannel!.AddPermissionOverwriteAsync(session.Player.User, overwritePermissions);
                }
            }
        }
    }
}