namespace DiscordTextAdventure.Parsing
{
    public class Input
    {
        private Parser _parser;
        private Tokenizer _tokenizer;

        public void ProcessMessage(string message)
        {
           var tokens = _tokenizer.Tokenize(message);
           _parser.Parse(tokens);
        }
    }
}