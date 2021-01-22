using System;
using DiscordTextAdventure.Parsing;

#nullable enable
namespace chext.Parser
{
    public class Tokenizer
    {
        public static readonly char[] Seperators = {' ',};
        public Token Tokenize(string message)
        {
            var words = message.Split(Seperators, StringSplitOptions.RemoveEmptyEntries);
            var tokens = new Token[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                Token? previous = i != 0 ? words[i - 1] : null; 
                Token? next     = i <= words.Length-2 ? words[i+1] : null; 
                tokens[i] = new Token(words[i], previous, next);
            }
            
            //todo set previous next,
        }
    }
}