﻿using System.Collections.Generic;

#nullable enable
namespace TextAdventure.Parsing
{
    public class Parser //todo check for preposition and indirect object
    {
        private Phrase _phrase;
        public Phrase Parse(Token rootToken)
        {

            var tokenIndex = rootToken;
            var verbs = VerbTable.Verbs;
            var nouns = NounTable.Nouns;

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
            
            void CheckForMatch(ref Token? index, SynonymCollection [] synonyms, ref SynonymCollection? wordInPhrase)
            {
                for (int i = 0; i < synonyms.Length; i++)
                {
                    if (synonyms[i].CheckMatch(index, out var result))
                    {
                        wordInPhrase = synonyms[i];
                        index = result;
                        return;
                    }
                }
            }
                
        }
    }
}