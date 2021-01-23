#nullable enable

namespace TextAdventure.Parsing
{
    public class Input
    {
        private Tokenizer _tokenizer;
        private Parser _parser;

        public Input()
        {
            _tokenizer = new Tokenizer();
            _parser = new Parser();
        }
        public void ProcessMessage(string message)
        {
           var tokens = _tokenizer.Tokenize(message);
           var phrase = _parser.Parse(tokens);
           
        }
    }
}