using System.Collections.Generic;

#nullable enable
namespace TextAdventure.Parsing
{
    public class Parser //todo check for preposition and indirect object
    {
        //private Phrase _phrase;
        public Phrase Parse(Token rootToken)
        {
            var phrase = new Phrase();
            var tokenIndex = rootToken;
            
            var verbs = VerbTable.Verbs;
            var nouns = NounTable.Nouns;
            // var prepositions = NoaunTable.Nouns;
            // var IndirectObject = NounTdasable.Nouns;
            

            phrase.Verb = CheckForMatch(ref tokenIndex, verbs);
            phrase.Noun = CheckForMatch(ref tokenIndex, nouns);

            return phrase;
            
            CompoundWord? CheckForMatch(ref Token? index, SynonymCollection [] synonyms)
            {
                CompoundWord? matchedWord = null;
                for (int i = 0; i < synonyms.Length; i++)
                {
                    if (synonyms[i].CheckMatch(index, out var result, out matchedWord))
                    {
                        index = result;
                        return matchedWord;
                    }
                }
                return matchedWord;
            }
                
        }
    }
}