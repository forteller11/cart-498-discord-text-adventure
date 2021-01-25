using DiscordTextAdventure.Parsing.DataStructures;

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class PrepositionTable
    {

        public static readonly SynonymCollection With = new SynonymCollection("with", "using", "via");
        public static readonly SynonymCollection[] Prepositions;
        static PrepositionTable()
        {
            Prepositions = DiscordTextAdventure.Common.ClassMembersToArray<SynonymCollection>(typeof(PrepositionTable), null);
        }
    }
}