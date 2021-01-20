using System.Collections.Generic;
#nullable enable

namespace DiscordTextAdventure.Parsing
{
    public static class WordTables
    {
        public static readonly Dictionary<int, string> Nouns;
        public static readonly Dictionary<int, string> Verb;

        //todo, synonomyms > string
        static WordTables()
        {
            string[] nouns =
            {
                "room",
                "bot",
                "north",
                "south"
            };
            
            string[] verbs =
            {
                "look",
                "walk",
            };
            
            AddWords(nouns, out Nouns);
            AddWords(nouns, out Verb);
        }

        static void AddWords(string [] words, out Dictionary<int, string> dictionary)
        {
            dictionary = new Dictionary<int, string>(words.Length);
            for (int i = 0; i < words.Length; i++)
                dictionary.Add(words[i].GetHashCode(), words[i]);
        }
    }
}