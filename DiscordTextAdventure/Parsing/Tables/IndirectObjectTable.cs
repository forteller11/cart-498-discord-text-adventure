
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class IndirectObjectTable
    {

        public static readonly SynonymCollection Axe = new SynonymCollection("axe", "sword");
  
        public static readonly  SynonymCollection[] IndirectObjects;

        static IndirectObjectTable()
        {
            IndirectObjects = ReflectionHelpers.ClassMembersToArray<SynonymCollection>(typeof(IndirectObjectTable), null);
        }
    }
}
