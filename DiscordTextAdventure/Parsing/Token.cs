using System;

#nullable  enable

namespace DiscordTextAdventure.Parsing
{
    public class Token
    {
        public Token? Previous; //will be null if first token
        public Token? Next; //will be null if last token

        public string Raw;

    }
}