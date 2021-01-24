using System.Threading.Tasks;
using chext;
using Discord;
using DiscordTextAdventure.Mechanics;
using DiscordTextAdventure.Mechanics.Rooms;

namespace DiscordTextAdventure.Discord.Rendering
{

    public class RoomRenderer
    {
        public readonly EmbedBuilder Builder;
        public readonly IMessageChannel Channel;
        public readonly Room Room;
        
        private Task<IUserMessage> _messageTask;
        private bool _hasDrawnBefore;

        public RoomRenderer(Room room, IMessageChannel channel)
        {
            Channel = channel;
            Room = room;
            
            Builder = new EmbedBuilder();
        }

        public async Task DrawRoom()
        {
            //Builder.Title = Room.Name;
            Builder.Description = Room.StaticDescription;
            
            EmbedFieldBuilder fieldBuilder = new EmbedFieldBuilder();
            fieldBuilder.Name = "contains";
            fieldBuilder.Value = Room.DynamicDescription.Invoke(Room);
            fieldBuilder.IsInline = true;
            
            Builder.Fields.Add(fieldBuilder);

            await Draw();
        }

        private async Task Draw()
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