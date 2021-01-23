using System;

namespace TextAdventure.Parsing
{
    public class Phrase : IEquatable<Phrase>
    {
        public SynonymCollection? Verb;
        public SynonymCollection? Noun;
        public SynonymCollection? Preposition;
        public SynonymCollection? IndirectObject;

        public bool Equals(Phrase other)
        {
            return 
                      Verb.Equals(other.Verb) 
                   && Noun.Equals(other.Noun) 
                   && Preposition.Equals(other.Preposition) 
                   && IndirectObject.Equals(other.IndirectObject);
        }

    }
}