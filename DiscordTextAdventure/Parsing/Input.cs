﻿using System.Collections.Generic;

#nullable enable

namespace TextAdventure.Parsing
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
           
           for (int i = 0; i < Responses.Count; i++)
           {
               if (phrase.Equals(Responses[i].Action))
               {
                   
               }
           }
           
        }
    }
}