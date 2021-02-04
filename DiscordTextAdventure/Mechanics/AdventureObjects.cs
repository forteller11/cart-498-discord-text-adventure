using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext.Mechanics;
using DiscordTextAdventure.Mechanics.Responses;
using DiscordTextAdventure.Mechanics.Rooms;
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
        public Room? CurrentRoom;
        public object State;

        private List<PhraseResponse> _phraseResponsesBuffer = new List<PhraseResponse>();

        // public Action<PhraseResponseEventArgs> LookResponse;
        public string Article
        {
            get
            {
                if (IsPlural) return String.Empty;
                return Name[0] == 'a' || Name[0] == 'A' ? "an" : "a";
            }
        }

        public AdventureObject(SynonymCollection names, string description, bool isPlural = false)
        {
            Names = names;
            Description = description;
            IsPlural = isPlural;
        }
        
        

        AdventureObject WithAddAdventureResponse(Action<PhraseResponseEventArgs>? action, Func<PhraseResponseEventArgs, Task>? actionAsync, SynonymCollection verbs, SynonymCollection? preps = null, SynonymCollection? indirectObj = null)
        {
            _phraseResponsesBuffer.Add(new PhraseResponse(new PhraseBlueprint(verbs, Names, preps, indirectObj, null), action,
                actionAsync));
            return this;
        }

        public void LinkActions(Session session, RoomManager roomManager)
        {
            for (int i = 0; i < _phraseResponsesBuffer.Count; i++)
                roomManager.ResponsesToAddToResponseTable.Add(_phraseResponsesBuffer[i]);
        }

        #region response helpers
        public AdventureObject WithInspectDefault()
        {
            return WithAddAdventureResponse(e 
                    => { e.RoomOfPhrase.Renderer.Channel.SendMessageAsync(Description); },
                null, 
                VerbTable.Inspect);
        }
        public AdventureObject WithPickupNoSense()
        {
            return WithAddAdventureResponse(e 
                => { e.Message.Channel.SendMessageAsync($"What would picking up {Article} {Names} even mean?"); },
                null, 
                VerbTable.Pickup);
        }
        
        public AdventureObject WithCannotPickup()
        {
            return WithAddAdventureResponse(e 
                    => { e.Message.Channel.SendMessageAsync($"Cannot take {Name}"); },
                null, 
                VerbTable.Pickup);
        }
        
        
        #endregion
        

        
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
        
        public void CannotPickupDefault(PhraseResponseEventArgs e)
        {
            e.Message.Channel.SendMessageAsync($"Cannot take {Name}");
        }
        public void NoSensePickupDefault(PhraseResponseEventArgs e)
        {
            e.Message.Channel.SendMessageAsync("What would that even mean?");
        }
        public void CanPickupDefault(Session session) => throw new NotImplementedException();



    }
}