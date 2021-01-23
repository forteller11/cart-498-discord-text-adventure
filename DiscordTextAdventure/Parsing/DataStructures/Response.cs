using System;

namespace DiscordTextAdventure.Parsing.DataStructures
{
    //to respond to phrase with, phrase
    public class Response
    {
        public readonly PhraseBlueprint PhraseBlueprintToMatch;
        public readonly Action<PhraseBlueprint> Action;


        public Response(PhraseBlueprint phraseBlueprintToMatch, Action<PhraseBlueprint> action)
        {
            PhraseBlueprintToMatch = phraseBlueprintToMatch;
            Action = action;
        }
    }
}