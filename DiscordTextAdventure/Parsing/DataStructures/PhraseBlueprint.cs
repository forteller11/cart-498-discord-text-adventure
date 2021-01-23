#nullable enable

namespace DiscordTextAdventure.Parsing.DataStructures
{
    public class PhraseBlueprint
    {
        public SynonymCollection? Verb;
        public SynonymCollection? Noun;
        public SynonymCollection? Preposition;
        public SynonymCollection? IndirectObject;

        public override string ToString()
        {
            string a = Verb != null ? Verb.ToString()! : "";
            string b = Noun != null ? Noun.ToString()! : "";
            string c = Preposition != null ? Preposition.ToString()! : "";
            string d = IndirectObject != null ? IndirectObject.ToString()! : "";
            return a + " " + b + " " + c + " "+ d;
        }

        public bool MatchesPhrase(Phrase phrase)
        {
            return DoesWordMatch(Verb, phrase.Verb) &&
                   DoesWordMatch(Noun, phrase.Noun) &&
                   DoesWordMatch(Preposition, phrase.Preposition) &&
                   DoesWordMatch(IndirectObject, phrase.IndirectObject);
            
            bool DoesWordMatch(SynonymCollection? blueprint, CompoundWord? word)
            {
                if (blueprint == null && word == null)
                    return true;
                
                if (blueprint != null && word != null)
                    return blueprint.ContainsCompoundWord(word);
                else
                    return false;
            }
        }
    }
}