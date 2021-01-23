using System;
using System.Collections.Generic;

#nullable enable

namespace DiscordTextAdventure.Parsing.DataStructures
{

    public class SynonymCollection : IEquatable<SynonymCollection>
    {
        public readonly CompoundWord[] Synonyms;

        public SynonymCollection(params string[] words)
        {
            var sanitizedWords = new string[words.Length];
            for (int i = 0; i < words.Length; i++)
                sanitizedWords[i] = words[i].ToLower();
            
            //todo allow for compound words using "word-word" in inpuyt

            Synonyms = new CompoundWord[sanitizedWords.Length];
            for (int i = 0; i < sanitizedWords.Length; i++)
            {
                Synonyms[i] = new CompoundWord(sanitizedWords[i]);
            }
        }


        public bool Equals(SynonymCollection other)
        {
            for (int i = 0; i < Synonyms.Length; i++)
                if (!Synonyms[i].Equals(other.Synonyms[i]))
                    return false;

            return true;
        }

        

        public bool CheckMatch(List<Token> tokens, ref int index, out CompoundWord? matchedWord)
        {
            matchedWord = null;
            
            for (int i = 0; i < Synonyms.Length; i++)
            {
                if (index >= tokens.Count)
                    return false;

                if (Synonyms[i].CheckMatch(tokens, ref index, out matchedWord))
                    return true;
            }
            return false;
        }

        public bool ContainsCompoundWord(CompoundWord compoundWord)
        {
            for (int i = 0; i < Synonyms.Length; i++)
            {
                if (Synonyms[i].Equals(compoundWord))
                    return true;
            }

            return false;
        }
        
        public bool ContainsString(string str)
        {
            for (int i = 0; i < Synonyms.Length; i++)
                if (Synonyms[i].Equals(str))
                    return true;
            return false;
        }
    }
}