using Discord.WebSocket;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class VerbTable
    {
        public static readonly SynonymCollection Move = new SynonymCollection("go", "travel", "walk", "run", "move");
        public static readonly SynonymCollection Inspect = new SynonymCollection("look", "inspect", "observe");
        public static readonly SynonymCollection Pickup = new SynonymCollection("pick up", "grab", "take", "remove", "snatch");

        public static readonly SynonymCollection Salutation =
            new SynonymCollection("hello", "hey", "bonjour", "salut", "what's up", "hi", "howdy");
        public static readonly SynonymCollection Speak = new SynonymCollection("speak", "talk", "converse");
        
        public static readonly SynonymCollection SessionReset = new SynonymCollection("__reset__");
        
        public static readonly SynonymCollection Destroy = new SynonymCollection("attack", "destroy", "hit", "hurt", "kill", "damage", "strike", "cut", "punch", "damage", "break", "🪓");

        public readonly static SynonymCollection[] Verbs;

        static VerbTable()
        {
            Verbs = ReflectionHelpers.ClassMembersToArray<SynonymCollection>(typeof(VerbTable), null);
        }
    }
}