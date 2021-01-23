﻿using System;

namespace TextAdventure.Parsing
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