using System;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

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

        public virtual void OnLook(Phrase phrase)
        {
            
        }

        public virtual void OnHit(Phrase phrase)
        {
            // IndirectObjectTable.Axe.ContainsCompoundWord(phrase.IndirectObject);
            // if (phrase.IndirectObject.Equals(IndirectObjectTable.Axe))
        }

    }
}