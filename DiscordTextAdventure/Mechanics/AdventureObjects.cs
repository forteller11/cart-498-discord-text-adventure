using System;
using System.Threading.Tasks;
using chext.Mechanics;
using DiscordTextAdventure.Mechanics.Responses;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

#nullable enable

namespace DiscordTextAdventure.Mechanics
{
    public class AdventureObject
    {
        public string Name => Names.Synonyms[0].ToString();
        public readonly SynonymCollection Names;
        public readonly string Description;
        public readonly bool IsPlural;
        
        // public Action<PhraseResponseEventArgs> LookResponse;
        public string Article {
            get
            {
                if (IsPlural) return String.Empty;
                return Name[0] == 'a' || Name[0] == 'A' ? "an" : "a";
            }
        }

    public AdventureObject(SynonymCollection names, Session session, string description, bool isPlural=false)
    {
        Names = names;
        Description = description;
        IsPlural = isPlural;
        
        session.PhraseResponseManager.PhraseResponses.Add(
            new PhraseResponse(new PhraseBlueprint(VerbTable.Inspect, names, null, null, null), null, LookAtDefault));
    }
        
        public virtual void OnLook(PhraseResponseEventArgs e)
        {
            // if (e.RoomOfPhrase.)
        }

        public virtual void OnHit(Phrase phrase)
        {
            // IndirectObjectTable.Axe.ContainsCompoundWord(phrase.IndirectObject);
            // if (phrase.IndirectObject.Equals(IndirectObjectTable.Axe))
        }

        public async Task LookAtDefault(PhraseResponseEventArgs e)
        {
            await e.RoomOfPhrase.Renderer.Channel.SendMessageAsync(Description);
        }



    }
}