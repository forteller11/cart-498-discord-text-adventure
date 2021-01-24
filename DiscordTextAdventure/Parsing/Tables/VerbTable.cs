using Discord.WebSocket;
using DiscordTextAdventure.Parsing.DataStructures;

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class VerbTable
    {
        public static SynonymCollection Move = new SynonymCollection("go", "travel", "walk", "run", "move");
        public static SynonymCollection Pickup = new SynonymCollection("pick up", "grab", "take");

        public readonly static SynonymCollection[] Verbs;

        static VerbTable()
        {
            Verbs = DiscordTextAdventure.Common.ClassMembersToArray<SynonymCollection>(typeof(VerbTable), null);
        }
    }
}