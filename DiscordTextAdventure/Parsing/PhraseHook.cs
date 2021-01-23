using System;

namespace TextAdventure.Parsing
{
    public struct PhraseHook
    {
        public readonly PhraseBlueprint PhraseBlueprint;
        //public readonly event Action<PhraseBlueprint> DoSomething;

        public PhraseHook(PhraseBlueprint phraseBlueprint)
        {
            PhraseBlueprint = phraseBlueprint;
        }
    }
}