using System;
#nullable enable

namespace DiscordTextAdventure.Mechanics
{
    public class AdventureObject
    {
        public string Name = String.Empty;
        public string Description = String.Empty;
        public string Article => Name[0] == 'a' || Name[0] == 'A' ? "an" : "a"; 

        public AdventureObject(string name, string description)
        {
            Name = name;
            Description = description;
        }
        
        public bool Carryable;

    }
}