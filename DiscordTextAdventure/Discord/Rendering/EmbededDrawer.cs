﻿using System.Threading.Tasks;
using chext;
using Discord;
using DiscordTextAdventure.Mechanics;
using DiscordTextAdventure.Mechanics.Rooms;

namespace DiscordTextAdventure.Discord.Rendering
{

    public class EmbededDrawer
    {
        public readonly EmbedBuilder Builder;
        public readonly IMessageChannel Channel;
        
        private Task<IUserMessage> _messageTask;
        private bool _hasDrawnBefore;

        public EmbededDrawer(IMessageChannel channel)
        {
            Builder = new EmbedBuilder();
            Channel = channel;
        }

        public async ValueTask DrawRoom(Room room)
        {
            if (_hasDrawnBefore)
            {
                Program.DebugLog("draw modify start");
                await _messageTask; //makes sure you have the result of the user message returned from the first draw
                await _messageTask.Result.ModifyAsync(properties => { properties.Embed = Builder.Build(); });
            }
            else
            {
                Program.DebugLog("draw create start");
                _messageTask = Channel.SendMessageAsync(null, false, Builder.Build());
                _hasDrawnBefore = true;
                await _messageTask;
            }
            Program.DebugLog("draw done");
        }
    }
}