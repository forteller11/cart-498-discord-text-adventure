using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using chext;
using chext.Mechanics;
using Discord;
using Discord.Rest;
using DiscordTextAdventure.Discord.Rendering;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Rooms
{
    public class Room
    {
        public string Name;
        public string Subtitle = String.Empty;
        public string StaticDescription = String.Empty;
        public Func<Room, string> DynamicDescription = DefaultDynamicDescription;
        public Emoji []? Reactions;

        private const string IS = "is";
        private const string ARE = "are";
        
        
        public List<AdventureObject> Objects = new List<AdventureObject>();
        
        public readonly bool  IsDMChannel;
        public IMessageChannel? RoomOwnerChannel;

        //These are only guaranteed to be not null if the room is NOT a DM channel
        public IMessageChannel? DissoanceChannel;
        public IMessageChannel? BodyChannel;
        public IMessageChannel? MemeChannel;


        public RoomRenderer? Renderer;

        private Room(bool isDmChannel)
        {
            IsDMChannel = isDmChannel;
        }
 
        public static Room CreateGuildRoom(string name, RoomCategory category)
        {
        
            var room = new Room(false);
            room.Name = name.Replace(' ', '_');
            category.Rooms.Add(room);
            return room;
        }

        public static Room CreateDMRoom()
        {
            return new Room(true);
        }
        

        public void InitAndDraw(Session session, IMessageChannel channelOwner)
        {
            
            //get channel from each bots perspective (if not dm channel) so a room can send message from whatever bot it wants
            IMessageChannel? disChannel = null;
            IMessageChannel? BodyChannel = null;
            IMessageChannel? memeChannel = null;
         
            if (!IsDMChannel)
            {
                disChannel  = (IMessageChannel) session.DissonanceBot.GetChannel(channelOwner.Id);
                BodyChannel = (IMessageChannel) session.BodyBot.GetChannel(channelOwner.Id);
                memeChannel = (IMessageChannel) session.MemeBot.GetChannel(channelOwner.Id);
            }
            
            LinkToDiscord(channelOwner, disChannel, BodyChannel, memeChannel);
            LinkActions(session);
            Renderer.DrawRoomStateEmbed();
        }
        
        void LinkToDiscord(IMessageChannel ownerOfRoom, IMessageChannel? dissonanceBot, IMessageChannel? bodyBot, IMessageChannel? memeBot)
        {
            // if (IsDMChannel && guildChannel != null)
            //     throw new ArgumentException("This is a DM channel but was fed a guild channel");
            // if (!IsDMChannel && guildChannel == null)
            //     throw new ArgumentException("This is a guild channel but was NOT fed a guild channel");
            
            RoomOwnerChannel = ownerOfRoom;
            
            DissoanceChannel = dissonanceBot;
            BodyChannel = bodyBot;
            MemeChannel = memeBot;

            Renderer = new RoomRenderer(this, ownerOfRoom);
        }

        void LinkActions(Session session)
        {
            for (int i = 0; i < Objects.Count; i++)
                Objects[i].LinkActions(session);

        }
        
        #region builder helpers
        public Room WithStaticDescriptions(string staticDescription) 
        {
            StaticDescription = staticDescription;
            return this;
        }
        
        public Room WithSubtitle(string subtitle) 
        {
            Subtitle = subtitle;
            return this;
        }

        public Room WithDynamicDescription(Func<Room, string> dynamicDescription)
        {
            DynamicDescription = dynamicDescription;
            return this;
        }
        
        public Room WithObjects(params AdventureObject[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
                objects[i].CurrentRoom = this;
            Objects.AddRange(objects);
            return this;
        }

        public Room WithReaction(params Emoji[] reactions)
        {
            for (int i = 0; i < reactions.Length; i++)
            {
                Program.DebugLog (reactions[i].Name) ;
            }
            Reactions = reactions;
            return this;
        }

        #endregion
        
        public AdventureObject? TryFindFirstObject(SynonymCollection synonyms)
        {
            foreach (var obj in Objects)
            {
                if (obj.Names == synonyms)
                {
                    return obj;
                }
            }

            Program.DebugLog("maybe equality check on memory position should be replaced with .Equals() on value?");
            return null;
        }

        static string DefaultDynamicDescription(Room room)
        {
            StringBuilder builder = new StringBuilder("");

            if (room.Objects.Count == 0)
            {
                builder.Append("The room is empty.");
            }
            for (int i = 0; i < room.Objects.Count; i++)
            {
                var obj = room.Objects[i];
                bool isLastElement = i == room.Objects.Count - 1;
                
                if (i == 0)
                    builder.Append($"There {(obj.IsPlural ? "are" : "is")}");
                else if (isLastElement) //else if means that this will only apply if last element and there is MORE than one object
                    builder.Append(", and");
                else
                    builder.Append(",");
                
                
                builder.Append(" " + obj.Article + " " + room.Objects[i].Name);
                
                if (isLastElement)
                    builder.Append(".");
            }

            return builder.ToString();
        }
    }
}