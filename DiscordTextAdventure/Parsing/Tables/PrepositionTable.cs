using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class PrepositionTable
    {

        public static readonly SynonymCollection With = new SynonymCollection("with", "using", "via");
        public static readonly SynonymCollection[] Prepositions;
        static PrepositionTable()
        {
            Prepositions = ReflectionHelpers.ClassMembersToArray<SynonymCollection>(typeof(PrepositionTable), null);
        }
    }
}