#nullable enable

namespace DiscordTextAdventure.Parsing.DataStructures
{
    public class Phrase
    {
        public CompoundWord? Verb;
        public CompoundWord? Noun;
        public CompoundWord? Preposition;
        public CompoundWord? IndirectObject;

        public override string ToString()
        {
            string a = Verb != null ? Verb.ToString() : "";
            string b = Noun != null ? Noun.ToString() : "";
            string c = Preposition != null ? Preposition.ToString() : "";
            string d = IndirectObject != null ? IndirectObject.ToString() : "";
            return a + " " + b + " " + c + " "+ d;
        }
    }
}