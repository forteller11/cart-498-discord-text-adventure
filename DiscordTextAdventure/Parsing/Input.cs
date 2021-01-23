using System;
using System.Collections.Generic;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable

namespace DiscordTextAdventure.Parsing
{
    public class Input
    {
        public List<Response> Responses;
        
        private Tokenizer _tokenizer;
        private Parser _parser;

        public Input()
        {
            _tokenizer = new Tokenizer();
            _parser = new Parser();
            Responses = new List<Response>();
        }
        
        public void ProcessMessage(string message)
        {
           var tokens = _tokenizer.Tokenize(message);
           var phrase = _parser.Parse(tokens);
           Console.WriteLine(phrase.ToString());
           
           for (int i = 0; i < Responses.Count; i++)
           {
               if (phrase.Equals(Responses[i].Action))
               {
                   
               }
           }
           
        }
    }
}