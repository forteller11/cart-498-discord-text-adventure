﻿using System;
using System.Threading.Tasks;
using chext;
using Discord;
using DiscordTextAdventure.Mechanics;
using DiscordTextAdventure.Mechanics.Rooms;
#nullable enable

namespace DiscordTextAdventure.Discord.Rendering
{

    public class RoomRenderer
    {
        public readonly EmbedBuilder Builder;
        public readonly IMessageChannel Channel;
        public readonly Room Room;
        
        private Task<IUserMessage>? _messageTask;
        private bool _hasDrawnBefore;

        public RoomRenderer(Room room, IMessageChannel channel)
        {
            Channel = channel;
            Room = room;
            
            Builder = new EmbedBuilder();
        }

      
        public async Task DrawRoomStateEmbed()
        {
            
            Builder.Title = Room.Subtitle;
            Builder.Description = Room.StaticDescription;
            
            EmbedFieldBuilder fieldBuilder = new EmbedFieldBuilder();
            fieldBuilder.Name = "contains";
            fieldBuilder.Value = Room.DynamicDescription.Invoke(Room);
            fieldBuilder.IsInline = true;
            
            Builder.Fields.Add(fieldBuilder);

            await DrawCustomEmbed(null, Builder);
        }

        public async Task DrawCustomEmbed(string? message, EmbedBuilder? builder)
        {
            if (_hasDrawnBefore && _messageTask == null)
                throw new Exception("Called draw too soon after initial draw...  task wasn't ready");
            
            if (_hasDrawnBefore)
            {
                Program.DebugLog("draw modify start");
                await _messageTask;
                _messageTask.Result.ModifyAsync(properties => { 
                    properties.Embed = builder?.Build();
                    properties.Content = message;
                });
            }
            else
            {
                Program.DebugLog("draw create start");
                _messageTask = Channel.SendMessageAsync(message, false, builder?.Build());
                _hasDrawnBefore = true;
            }

            await _messageTask;
            
            if (Room.Reactions != null)
            {
                var emojiTask = _messageTask.Result.AddReactionsAsync(Room.Reactions);
                await emojiTask;
                
                Program.DebugLog(Room.Reactions);
            }

            Program.DebugLog("draw done");
        }
    }
}