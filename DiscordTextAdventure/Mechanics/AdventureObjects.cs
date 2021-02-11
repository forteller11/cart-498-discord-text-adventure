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
        
        

        AdventureObject WithAddAdventureResponse(PhraseResponseTable table, Action<PhraseResponseEventArgs>? action, Func<PhraseResponseEventArgs, Task>? actionAsync, SynonymCollection verbs, SynonymCollection? preps = null, SynonymCollection? indirectObj = null)
        {
            table.PhraseResponses.Add(new PhraseResponse(new PhraseBlueprint(verbs, Names, preps, indirectObj, null), action,
                actionAsync));
            return this;
        }

        #region response helpers
        public AdventureObject WithInspectDefault(PhraseResponseTable table)
        {
            return WithAddAdventureResponse(table,
                e => { e.RoomOfPhrase.Renderer.Channel.SendMessageAsync(Description); },
                null, 
                VerbTable.Inspect);
        }
        public AdventureObject WithPickupNoSense(PhraseResponseTable table)
        {
            return WithAddAdventureResponse(table,
                e => { e.Message.Channel.SendMessageAsync($"What would picking up {Article} {Names} even mean?"); },
                null, 
                VerbTable.Pickup);
        }
        
        public AdventureObject WithCannotPickup(PhraseResponseTable table)
        {
            return WithAddAdventureResponse(table,
                e =>
                {
                    if (e.RoomOfPhrase != CurrentRoom)
                        return;
                    
                    e.Message.Channel.SendMessageAsync($"Cannot take {Name}");
                },
                null, 
                VerbTable.Pickup);
        }

        public AdventureObject WithDamageCustom(PhraseResponseTable table, Action<PhraseResponseEventArgs> action)
        {
            return WithAddAdventureResponse(table, action, null, VerbTable.Destroy);
        }
        
        public AdventureObject WithCannotDamage(PhraseResponseTable table, string message="I don't want to do that")
        {
            return WithAddAdventureResponse(table, CannotDamage, null, VerbTable.Destroy);

            void CannotDamage(PhraseResponseEventArgs e)
            {
                if (e.RoomOfPhrase.IsDMChannel)
                    e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(message);
                else
                    e.RoomOfPhrase.BodyChannel.SendMessageAsync(message);
            }
        }
        

        public AdventureObject OnPickup(PhraseResponseTable table, Action<PhraseResponseEventArgs> action)
        {
            return WithAddAdventureResponse(table, action, null, VerbTable.Pickup);
        }
        
        
        #endregion



    }
}