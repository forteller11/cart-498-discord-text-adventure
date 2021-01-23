using System;
using System.Collections.Generic;

#nullable enable

namespace DiscordTextAdventure.Parsing.DataStructures
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

      
        
        public bool CheckMatch(List<Token> tokens, ref int index, out CompoundWord? matchedWord)
        {

            matchedWord = null;
            
            for (int i = 0; i < Words.Count; i++)
            {
                if (Words[i] == tokens[index].Raw)
                {
                    index++;
                    
                    if (index >= tokens.Count)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            matchedWord = this;
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

        public override string ToString()
        {
            var output = String.Empty;
            for (int i = 0; i < Words.Count; i++)
                output += Words[i];
            
            return output;
        }
    }
}