namespace TextAdventure.Parsing
{
    public static class PrepositionTable
    {

        public static SynonymCollection With = new SynonymCollection("with", "using", "via");
        public static readonly SynonymCollection[] Prepositions;
        static PrepositionTable()
        {
            Prepositions = Common.ClassMembersToArray<SynonymCollection>(typeof(PrepositionTable));
        }
    }
}