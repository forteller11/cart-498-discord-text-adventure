using System.Collections.Generic;

#nullable enable
namespace DiscordTextAdventure.Parsing
{
    public class Parser
    {
        private Phrase _phrase;
        public Phrase Parse(Token rootToken)
        {

            var tokenIndex = rootToken;
            var verbs = WordTables.Verbs;
            var nouns = WordTables.Nouns;

            // for (int i = 0; i < WordTables.Verbs.Count; i++)
            //     if (verbs[i].CheckMatch(currentToken, out result))
            //         _phrase.Verb = verbs[i];
            //
            // for (int i = 0; i < WordTables.Verbs.Count; i++)
            //     if (verbs[i].CheckMatch(currentToken, out result))
            //         _phrase.Verb = verbs[i];

            CheckForMatch(ref tokenIndex, verbs, ref _phrase.Verb);
            CheckForMatch(ref tokenIndex, nouns, ref _phrase.Noun);

            return _phrase;
            
            void CheckForMatch(ref Token? tokenIndex, List<SynonymCollection> synonyms, ref SynonymCollection wordInPhrase)
            {
                for (int i = 0; i < synonyms.Count; i++)
                {
                    if (synonyms[i].CheckMatch(tokenIndex, out var result))
                    {
                        wordInPhrase = synonyms[i];
                        tokenIndex = result;
                        return;
                    }
                }
            }
                
        }
    }
}