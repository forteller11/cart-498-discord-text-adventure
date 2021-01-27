using Discord.WebSocket;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class VerbTable
    {
        public static readonly SynonymCollection Move = new SynonymCollection("go", "travel", "walk", "run", "move");
        public static readonly SynonymCollection Pickup = new SynonymCollection("pick up", "grab", "take");
        public static readonly SynonymCollection Salutation = new SynonymCollection("hello", "hey", "bonjour", "salut", "what's up", "hi", "howdy");

        public readonly static SynonymCollection[] Verbs;

        static VerbTable()
        {
            Verbs = ReflectionHelpers.ClassMembersToArray<SynonymCollection>(typeof(VerbTable), null);
        }
    }
}