namespace TextAdventure.Parsing
{
    public static class VerbTable
    {
        public static SynonymCollection Move = new SynonymCollection("go", "travel", "walk", "run", "move");

        public readonly static SynonymCollection[] Verbs;

        static VerbTable()
        {
            Verbs = Common.ClassMembersToArray<SynonymCollection>(typeof(VerbTable));
        }
    }
}