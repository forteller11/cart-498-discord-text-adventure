using System.Collections.Generic;

#nullable enable

namespace DiscordTextAdventure.Parsing
{

    
    public static class WordTables
    {
        public static readonly List<SynonymCollection> Nouns;
        public static readonly List<SynonymCollection> Verbs;
        
        static WordTables()
        {
            Nouns = new List<SynonymCollection>
            {
               new SynonymCollection( "north"),
               new SynonymCollection( "west"),
               new SynonymCollection( "east"),
               new SynonymCollection( "south")
            };
            
            Verbs = new List<SynonymCollection>
            {
                new SynonymCollection( "look"),
                new SynonymCollection( "type"),
                new SynonymCollection( "pony"),
                new SynonymCollection( "troll")
            };
          
        }


    }
}