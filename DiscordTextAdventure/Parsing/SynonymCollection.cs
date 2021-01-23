using System;
using System.Collections.Generic;

#nullable enable

namespace TextAdventure.Parsing
{

    public class SynonymCollection : IEquatable<SynonymCollection>
    {
        public readonly CompoundWord[] CompoundWords;

        public SynonymCollection(params string[] words)
        {
            var sanitizedWords = new string[words.Length];
            for (int i = 0; i < words.Length; i++)
                sanitizedWords[i] = words[i].ToLower();

            CompoundWords = new CompoundWord[sanitizedWords.Length];
            for (int i = 0; i < sanitizedWords.Length; i++)
            {
                CompoundWords[i] = new CompoundWord(sanitizedWords[i]);
            }
        }


        public bool Equals(SynonymCollection other)
        {
            for (int i = 0; i < CompoundWords.Length; i++)
                if (!CompoundWords[i].Equals(other.CompoundWords[i]))
                    return false;

            return true;
        }

        
        /// <param name="token"> the initial token</param>
        /// <param name="result"> the token after a parse, will be identical if no match</param>
        /// <returns></returns>
        public bool CheckMatch(in Token token, out Token? result)
        {
            result = token;
            for (int i = 0; i < CompoundWords.Length; i++)
            {
                if (CompoundWords[i].CheckMatch(token, out result))
                    return true;
            }
            return false;
        }
        
        public bool ContainsString(string str)
        {
            for (int i = 0; i < CompoundWords.Length; i++)
                if (CompoundWords[i].Equals(str))
                    return true;
            return false;
        }
    }
}