using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

#nullable enable

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class NounTable
    {
        public static readonly SynonymCollection South = new SynonymCollection("south");
        public static readonly SynonymCollection North = new SynonymCollection("north");

        public static readonly SynonymCollection Arms = new SynonymCollection("legs", "leg");
        public static readonly SynonymCollection Head = new SynonymCollection("legs", "leg");
        public static readonly SynonymCollection Legs = new SynonymCollection("arms", "arm");

        public static readonly SynonymCollection[] Nouns;
        
        static NounTable()
        {
            Nouns = ReflectionHelpers.ClassMembersToArray<SynonymCollection>(typeof(NounTable), null);
        }

        
    }
}

