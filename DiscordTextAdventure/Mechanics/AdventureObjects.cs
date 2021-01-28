using System;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

#nullable enable

namespace DiscordTextAdventure.Mechanics
{
    public class AdventureObject
    {
        public readonly string Name;
        public readonly string Description;
        public readonly bool IsPlural;
        public string Article {
            get
            {
                if (IsPlural) return String.Empty;
                return Name[0] == 'a' || Name[0] == 'A' ? "an" : "a";
            }
        }

    public AdventureObject(string name, string description, bool isPlural=false)
        {
            Name = name;
            Description = description;
            IsPlural = isPlural;
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