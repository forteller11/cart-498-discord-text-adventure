﻿using System;

#nullable  enable

namespace TextAdventure.Parsing
{
    public class Token
    {
        public Token? Previous; //will be null if first token
        public Token? Next; //will be null if last token

        public readonly string Raw;

        public Token(string msg)
        {
            Raw = msg;
        }

    }
}