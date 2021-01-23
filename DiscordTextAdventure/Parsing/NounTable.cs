#nullable enable

namespace TextAdventure.Parsing
{
    public static class NounTable
    {
        
        public static SynonymCollection North = new SynonymCollection("north");
        public static SynonymCollection South = new SynonymCollection("south");


        public static readonly SynonymCollection[] Nouns;
        
        static NounTable()
        {
            Nouns = Common.ClassMembersToArray<SynonymCollection>(typeof(NounTable));
        }

        
    }
}