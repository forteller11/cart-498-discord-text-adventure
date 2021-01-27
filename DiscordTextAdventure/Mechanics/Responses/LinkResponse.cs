using System;
using System.Threading.Tasks;
using chext.Mechanics;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Parsing.DataStructures;
#nullable enable

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class LinkResponse 
    {
        public Action<LinkResponseEventArgs>? Action;
        public Func<LinkResponseEventArgs, Task>? ActionAsync;


        public LinkResponse(Action<LinkResponseEventArgs>? action, Func<LinkResponseEventArgs, Task>? actionAsync)
        {
            Action = action;
            ActionAsync = actionAsync;
        }

        public void CallResponses(LinkResponseEventArgs eventArgs)
        {
            Action?.Invoke(eventArgs);
            ActionAsync?.Invoke(eventArgs);
        }
    }

    public class LinkResponseEventArgs
    {
        public readonly Link Link;
        public readonly Session Session;
        public readonly Room PostedRoom;

        public LinkResponseEventArgs(Session session, Link link, Room postedRoom)
        {
            Link = link;
            Session = session;
            PostedRoom = postedRoom;
        }
    }
}