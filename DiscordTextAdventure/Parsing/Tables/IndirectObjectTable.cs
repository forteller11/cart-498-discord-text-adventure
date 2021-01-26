using Discord.WebSocket;
using DiscordTextAdventure.Parsing.DataStructures;

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class IndirectObjectTable
    {

        public static readonly SynonymCollection Axe = new SynonymCollection("axe", "sword");
  
        public static readonly  SynonymCollection[] IndirectObjects;

        static IndirectObjectTable()
        {
            IndirectObjects = DiscordTextAdventure.Common.ClassMembersToArray<SynonymCollection>(typeof(IndirectObjectTable), null);
        }
    }
}