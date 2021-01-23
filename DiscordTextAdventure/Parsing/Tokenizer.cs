using System;
using System.Collections.Generic;
using System.Linq;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

#nullable enable
namespace DiscordTextAdventure.Parsing
{
    public class Tokenizer
    {
        //todo, remove articles and sanitize....
        public List<Token> Tokenize(string message)
        {

            var lowerWords = message.ToLower();
            
            var words = lowerWords.Split(Common.SEPERATORS, StringSplitOptions.RemoveEmptyEntries).ToList();

            //remove articles
            for (int i = 0; i < words.Count; i++)
            {
                if (ArticleTable.Articles.Contains(words[i]))
                    words.RemoveAt(i);
            }
            
            var tokens = new List<Token>(words.Count);
            for (int i = 0; i < words.Count; i++)
                tokens.Add(new Token(words[i]));

            return tokens;
        }
        
      
    }
}