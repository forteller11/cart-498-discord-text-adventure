using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

#nullable enable

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class NounTable
    {
        public static readonly SynonymCollection South = new SynonymCollection("south");
        public static readonly SynonymCollection North = new SynonymCollection("north");
        public static readonly SynonymCollection DankMemeBot = new SynonymCollection("meme bot", "meme generator", "memer");
        
        public static readonly SynonymCollection[] Nouns;
        
        static NounTable()
        {
            Nouns = ReflectionHelpers.ClassMembersToArray<SynonymCollection>(typeof(NounTable), null);
        }

        
    }
}

