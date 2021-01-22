using DiscordTextAdventure.Parsing;

namespace chext.Parser
{
    public class Parser
    {
        private Phrase _phrase;
        public Phrase Parse(Token rootToken)
        {
            var currentToken = rootToken;
            var nouns = WordTables.Nouns;
            var verbs = WordTables.Verbs;

            Token result = null;
            
                for (int i = 0; i < WordTables.Nouns.Count; i++)
                {
                    if (!nouns[i].CheckMatch(currentToken, out result))
                        continue;
                    else
                    {
                        _phrase.Noun = nouns[i];
                    }
                }
            
                
        }
}
}