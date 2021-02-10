using System;
using DiscordTextAdventure.Mechanics.Rooms;

#nullable enable

namespace DiscordTextAdventure.Parsing.DataStructures
{
    public class PhraseBlueprint
    {
        public enum BlueprintType
        {
            Phrase,
            MustContains,
            AnythingInChannel
        }

        public readonly BlueprintType Type;
        public readonly SynonymCollection? MustContain;
        
        public readonly SynonymCollection? Verb;
        public readonly SynonymCollection? Noun;
        public readonly SynonymCollection? Preposition;
        public readonly SynonymCollection? IndirectObject;
        public readonly Room [] ? RoomsFilter; //null == all rooms


        public PhraseBlueprint(SynonymCollection? verb, SynonymCollection? noun, SynonymCollection? preposition, SynonymCollection? indirectObject, Room []?  roomsFilter)
        {
            Type = BlueprintType.Phrase;
            
            MustContain = null;
            
            Verb = verb;
            Noun = noun;
            Preposition = preposition;
            IndirectObject = indirectObject;
            
            RoomsFilter = roomsFilter;
        }
        
        public PhraseBlueprint(SynonymCollection mustContain, Room []?  roomsFilter)
        {
            Type = BlueprintType.MustContains;
            
            MustContain = mustContain;
            
            Verb = null;
            Noun = null;
            Preposition = null;
            IndirectObject = null;
            
            RoomsFilter = roomsFilter;
        }
        
        public PhraseBlueprint(Room []?  roomsFilter)
        {
            Type = BlueprintType.AnythingInChannel;
            
            MustContain = null;
            Verb = null;
            Noun = null;
            Preposition = null;
            IndirectObject = null;
            
            RoomsFilter = roomsFilter;
        }

        public override string ToString()
        {
            string a = Verb != null ? Verb.ToString()! : "";
            string b = Noun != null ? Noun.ToString()! : "";
            string c = Preposition != null ? Preposition.ToString()! : "";
            string d = IndirectObject != null ? IndirectObject.ToString()! : "";
            return a + " " + b + " " + c + " "+ d;
        }

        public bool MatchesPhrase(Phrase phrase, Room room)
        {
            if (RoomsFilter != null)
            {
                bool roomMatches = false;
                for (int i = 0; i < RoomsFilter.Length; i++)
                {
                    if (RoomsFilter[i].RoomOwnerChannel!.Id == room.RoomOwnerChannel!.Id)
                    {
                        roomMatches = true;
                        break;
                    }
                }

                if (!roomMatches)
                    return false;
            }

            switch (Type)
            {
               case BlueprintType.AnythingInChannel:
                   return true;
               case BlueprintType.MustContains:
                   return
                       DoesWordMatch(MustContain, phrase.Verb) ||
                       DoesWordMatch(MustContain, phrase.Noun) ||
                       DoesWordMatch(MustContain, phrase.Preposition) ||
                       DoesWordMatch(MustContain, phrase.IndirectObject);
               case BlueprintType.Phrase:
                   return
                       DoesWordMatch(Verb, phrase.Verb) &&
                       DoesWordMatch(Noun, phrase.Noun) &&
                       DoesWordMatch(Preposition, phrase.Preposition) &&
                       DoesWordMatch(IndirectObject, phrase.IndirectObject);

               default:
                   throw new Exception("No Type specefied");
                   
            }
            
            bool DoesWordMatch(SynonymCollection? blueprint, CompoundWord? word)
            {
                if (blueprint == null && word == null)
                    return true;
                
                if (blueprint != null && word != null)
                    return blueprint.ContainsCompoundWord(word);
                else
                    return false;
            }
        }
    }
}