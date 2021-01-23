using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace TextAdventure.Parsing
{
    public class Tokenizer
    {
        //todo, remove articles and sanitize....
        public List<Token> Tokenize(string message)
        {
            var words = message.Split(Common.SEPERATORS, StringSplitOptions.RemoveEmptyEntries);
            var tokens = new Token[words.Length];
            
            for (int i = 0; i < words.Length; i++)
            {
                var currentToken = new Token(words[i]);
                tokens[i] = currentToken;

                if (i != 0)
                {
                    var previousToken = tokens[i - 1];
                   
                    previousToken.Next = currentToken;
                    currentToken.Previous = previousToken;
                }
            }

            return tokens.ToList();
        }
    }
}