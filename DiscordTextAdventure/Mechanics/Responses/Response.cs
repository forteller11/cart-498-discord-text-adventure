using System;
using DiscordTextAdventure.Parsing.DataStructures;

namespace DiscordTextAdventure.Mechanics.Responses
{
    //to respond to phrase with, phrase
    public class Response
    {
        public readonly PhraseBlueprint PhraseBlueprint;
        public readonly Action<ResponseEventArg> Action;


        public Response(PhraseBlueprint phraseBlueprint, Action<ResponseEventArg> action)
        {
            PhraseBlueprint = phraseBlueprint;
            Action = action;
        }
    }
}