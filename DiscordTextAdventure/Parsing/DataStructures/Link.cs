using System.Collections.Generic;
using Discord.WebSocket;

namespace DiscordTextAdventure.Parsing.DataStructures
{
    public class Link
    {
        public readonly bool IsValid;
        public readonly List<Token> Words;
        public readonly SocketMessage Message;

        public Link(SocketMessage message, List<Token> words, bool isValid)
        {
            IsValid = isValid;
            Words = words;
            Message = message;
        }
    }
}