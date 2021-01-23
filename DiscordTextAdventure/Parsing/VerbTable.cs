namespace TextAdventure.Parsing
{
    public static class VerbTable
    {
        public static SynonymCollection north = new SynonymCollection("dog");
        public static SynonymCollection south = new SynonymCollection("southexample");

        public readonly static SynonymCollection[] Verbs;

        static VerbTable()
        {
            Verbs = Common.ClassMembersToArray<SynonymCollection>(typeof(VerbTable));
        }
    }
}