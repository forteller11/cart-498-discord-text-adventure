using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class NounTable
    {
        public static SynonymCollection South = new SynonymCollection("south");
        public static SynonymCollection North = new SynonymCollection("north");
        
        public static readonly SynonymCollection[] Nouns;
        
        static NounTable()
        {
            Nouns = DiscordTextAdventure.Common.ClassMembersToArray<SynonymCollection>(typeof(NounTable), null);
        }

        
    }
}