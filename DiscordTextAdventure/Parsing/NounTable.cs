#nullable enable

namespace TextAdventure.Parsing
{
    public static class NounTable
    {
        
        public static readonly SynonymCollection[] Nouns;
        
        static NounTable()
        {
            Nouns = Common.ClassMembersToArray<SynonymCollection>(typeof(VerbTable));
        }

        
    }
}