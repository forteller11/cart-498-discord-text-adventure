using System;

namespace TextAdventure.Parsing
{
    //to respond to phrase with, phrase
    public class Response
    {
        public readonly Phrase PhraseToMatch;
        public readonly Action<Phrase> Action;


        public Response(Phrase phraseToMatch, Action<Phrase> action)
        {
            PhraseToMatch = phraseToMatch;
            Action = action;
        }
    }
}