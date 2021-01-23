using System;
using System.Collections.Generic;

#nullable enable

namespace TextAdventure.Parsing
{
    //used to represent and support compound nouns/verbs like "pick up"
    
    public class CompoundWord : IEquatable<CompoundWord>
    {
        public static readonly CompoundWord Empty = new CompoundWord();
        public List<string> Words;

        public CompoundWord()
        {
            Words = new List<string>(2);
        }
        
        public CompoundWord(string words)
        {
            
            var wordsSeperated = words.Split(Common.SEPERATORS, StringSplitOptions.RemoveEmptyEntries);
            Words = new List<string>(wordsSeperated.Length);
            
            for (int i = 0; i < wordsSeperated.Length; i++)
            {
                Words.Add(wordsSeperated[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialToken"></param>
        /// <param name="result"> result null if at last token</param>
        /// <returns></returns>
        public bool CheckMatch(in Token initialToken, out Token? result, out CompoundWord? matchedWord)
        {
            Token currentToken = initialToken;
            result = null;
            matchedWord = null;
            
            for (int i = 0; i < Words.Count; i++)
            {
                if (Words[i] == currentToken.Raw)
                {
                    matchedWord ??= new CompoundWord();
                    matchedWord.Words.Add(initialToken.Raw);

                    if (initialToken.Next != null)
                        currentToken = initialToken.Next;
                    else
                        return false;
                }
                
                else
                {
                    result = initialToken;
                    return false;
                }
            }

            return true;
        }

        // public bool Contains(string word)
        // {
        //     for (int i = 0; i < Words.Length; i++)
        //     {
        //         if (Words[i] == word)
        //             return true;
        //     }
        //
        //     return false;
        // }

        public bool Equals(CompoundWord other)
        {
            if (Words.Count != other.Words.Count)
                return false;
            
            for (int i = 0; i < Words.Count; i++)
            {
                if (Words[i] != other.Words[i])
                    return false;
            }

            return true;
        }
        
    }
}