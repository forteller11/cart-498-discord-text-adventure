using System.Collections.Generic;

#nullable  enable

namespace DiscordTextAdventure.Parsing.DataStructures
{
    public class Token
    {
        public readonly string Raw;

        public Token(string msg)
        {
            Raw = msg;
        }

    }
}