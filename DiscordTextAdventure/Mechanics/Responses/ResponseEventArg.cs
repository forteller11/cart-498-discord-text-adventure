using chext.Tables;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Mechanics.Player;

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