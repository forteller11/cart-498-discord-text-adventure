namespace TextAdventure.Parsing
{
    public static class IndirectObjectTable
    {

        public static SynonymCollection Axe = new SynonymCollection("axe", "sword");
        public static readonly SynonymCollection[] IndirectObjects;

        static IndirectObjectTable()
        {
            IndirectObjects = Common.ClassMembersToArray<SynonymCollection>(typeof(IndirectObjectTable));
        }
    }
}