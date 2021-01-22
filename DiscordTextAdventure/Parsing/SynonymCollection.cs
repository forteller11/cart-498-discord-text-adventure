using System;
using System.Collections.Generic;

#nullable enable

namespace DiscordTextAdventure.Parsing
{

    public struct SynonymCollection //: IEquatable<SynonymCollection>
    {
        public readonly CompoundWord[] CompoundWords;

        public SynonymCollection(params string[] words)
        {
            CompoundWords = new CompoundWord[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                CompoundWords[i] = new CompoundWord(words[i]);
            }
        }


        public bool Equals(SynonymCollection other)
        {
            for (int i = 0; i < CompoundWords.Length; i++)
                if (!CompoundWords[i].Equals(other.CompoundWords[i]))
                    return false;

            return true;
        }


        
        public bool CheckMatch(in Token token, out Token? result)
        {
            for (int i = 0; i < CompoundWords.Length; i++)
            {
                if (CompoundWords[i].CheckMatch(token, out result))
                    return true;
            }

            result = null;
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