using System;

#nullable enable

namespace DiscordTextAdventure.Parsing
{
    //used to represent and support compound nouns/verbs like "pick up"
    public struct CompoundWord
    {
        public string[] Words;
        public static readonly char[] Seperators = { ' ', '-', '\t', '\n'};
        
        public CompoundWord(string words)
        {
            Words = words.Split(Seperators, StringSplitOptions.RemoveEmptyEntries);
        }

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
        
    }
}