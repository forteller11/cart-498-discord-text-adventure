using System;

#nullable enable

namespace TextAdventure.Parsing
{
    //used to represent and support compound nouns/verbs like "pick up"
    public class CompoundWord : IEquatable<CompoundWord>
    {
        public string[] Words;

        public CompoundWord(string words)
        {
            Words = words.Split(Common.SEPERATORS, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="result"> result null if at last token</param>
        /// <returns></returns>
        public bool CheckMatch(in Token token, out Token? result)
        {
            result = null;
            
            for (int i = 0; i < Words.Length; i++)
            {
                if (Words[i] != token.Raw)
                {
                    result = token;
                    return false;
                }
                else
                {
                    result = token.Next;
                }
            }

            return true;
        }

        public bool Equals(CompoundWord other)
        {
            if (Words.Length != other.Words.Length)
                return false;
            
            for (int i = 0; i < Words.Length; i++)
            {
                if (Words[i] != other.Words[i])
                    return false;
            }

            return true;
        }
        
    }
}