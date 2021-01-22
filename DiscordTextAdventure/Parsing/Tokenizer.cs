using System;

#nullable enable
namespace DiscordTextAdventure.Parsing
{
    public static class Tokenizer
    {

        public static Token Tokenize(string message)
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

            return tokens[0];
        }
    }
}