using System;
using System.Collections.Generic;

#nullable enable
namespace DiscordTextAdventure.Parsing
{
    public static class Common
    {
        public static readonly char[] SEPERATORS = {' ', '-', '_', '\t', '\n', '\\', '.', ':', '/'};
        public static readonly string[] HTTPStart = {"http", "https"};
    }
}