using System;

namespace TextAdventure.Parsing
{
    public struct PhraseHook
    {
        public readonly Phrase Phrase;
        public readonly event Action<Phrase> DoSomething;

        public PhraseHook(Phrase phrase)
        {
            Phrase = phrase;
        }
    }
}