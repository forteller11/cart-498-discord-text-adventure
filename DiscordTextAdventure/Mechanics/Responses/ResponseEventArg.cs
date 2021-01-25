using chext.Tables;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ResponseEventArg
    {
        public Phrase Phrase;
        public Player Player;
        public Room RoomOfPhrase;
    }
}