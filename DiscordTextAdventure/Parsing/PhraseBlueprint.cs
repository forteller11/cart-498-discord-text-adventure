using System;

namespace TextAdventure.Parsing
{
    public class PhraseBlueprint
    {
        public SynonymCollection? Verb;
        public SynonymCollection? Noun;
        public SynonymCollection? Preposition;
        public SynonymCollection? IndirectObject;

        public bool MatchesPhrase(Phrase phrase)
        {
           return 
               Verb.ContainsCompoundWord(phrase.Verb) &&
           Noun.ContainsCompoundWord(phrase.Noun) &&
           Preposition.ContainsCompoundWord(phrase.Preposition) &&
           IndirectObject.ContainsCompoundWord(phrase.IndirectObject);

        }

    }
}