using System;
using System.Collections.Generic;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable

namespace DiscordTextAdventure.Parsing
{
    public class Input
    {
        private List<Response> _responses;
        
        private Tokenizer _tokenizer;
        private Parser _parser;

        public Input()
        {
            _tokenizer = new Tokenizer();
            _parser = new Parser();
            _responses = new List<Response>();
        }

        public void AddResponse(Response response)
        {
            _responses.Add(response);
        }
        
        public void ProcessMessage(string message)
        {
           var tokens = _tokenizer.Tokenize(message);
           var phrase = _parser.Parse(tokens);

           for (int i = 0; i < _responses.Count; i++)
           {
               if (_responses[i].PhraseBlueprint.MatchesPhrase(phrase))
                   _responses[i].Action.Invoke(phrase);
           }
           
        }
    }
}