using System;

namespace DiscordTextAdventure.Parsing.DataStructures
{
    //to respond to phrase with, phrase
    public class Response
    {
        public readonly PhraseBlueprint PhraseBlueprint;
        public readonly Action<Phrase> Action;


        public Response(PhraseBlueprint phraseBlueprint, Action<Phrase> action)
        {
            PhraseBlueprint = phraseBlueprint;
            Action = action;
        }
    }
}