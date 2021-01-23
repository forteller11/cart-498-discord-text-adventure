using System;
using System.Collections.Generic;

#nullable enable

namespace TextAdventure.Parsing
{

    public class SynonymCollection : IEquatable<SynonymCollection>
    {
        public readonly CompoundWord[] Synonyms;

        public SynonymCollection(params string[] words)
        {
            var sanitizedWords = new string[words.Length];
            for (int i = 0; i < words.Length; i++)
                sanitizedWords[i] = words[i].ToLower();

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

        
        /// <param name="token"> the initial token</param>
        /// <param name="result"> the token after a parse, will be identical if no match</param>
        /// <returns></returns>
        public bool CheckMatch(in Token token, out Token? result, out CompoundWord? matchedWord)
        {
            result = token;
            matchedWord = null;
            
            for (int i = 0; i < Synonyms.Length; i++)
            {
                if (Synonyms[i].CheckMatch(token, out result, out matchedWord))
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