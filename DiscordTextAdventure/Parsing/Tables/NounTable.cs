using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

#nullable enable

namespace DiscordTextAdventure.Parsing.Tables
{
    public static class NounTable
    {
        public static readonly SynonymCollection South = new SynonymCollection("south");
        public static readonly SynonymCollection North = new SynonymCollection("north");

        public static readonly SynonymCollection Arms = new SynonymCollection("arms", "arm");
        public static readonly SynonymCollection Head = new SynonymCollection("head");
        public static readonly SynonymCollection Legs = new SynonymCollection("legs", "leg");
        
        public static readonly SynonymCollection Flags = new SynonymCollection("flags", "flag", "nationalities", "national-flags");
        public static readonly SynonymCollection NameTags = new SynonymCollection("names", "name tags","tags");
        public static readonly SynonymCollection Tarp = new SynonymCollection("tarp", "cover","blanket");
        public static readonly SynonymCollection MemeBot = new SynonymCollection("meme bot", "bot", "memer","meme machine", "meme-machine");
        public static readonly SynonymCollection Slide = new SynonymCollection("slide");
        public static readonly SynonymCollection Fridge = new SynonymCollection("fridge", "refrigerator");

        public static readonly SynonymCollection[] Nouns;

        static NounTable()
        {
            Nouns = ReflectionHelpers.ClassMembersToArray<SynonymCollection>(typeof(NounTable), null);
        }

        
    }
}

