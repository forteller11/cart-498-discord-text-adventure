using DiscordTextAdventure.Mechanics.Rooms;

#nullable enable

namespace DiscordTextAdventure.Parsing.DataStructures
{
    public class PhraseBlueprint
    {
        public readonly bool ContainsTypeBlueprint;
        public readonly SynonymCollection? MustContain;
        
        public readonly SynonymCollection? Verb;
        public readonly SynonymCollection? Noun;
        public readonly SynonymCollection? Preposition;
        public readonly SynonymCollection? IndirectObject;
        public readonly Room [] ? RoomsFilter; //null == all rooms


        public PhraseBlueprint(SynonymCollection? verb, SynonymCollection? noun, SynonymCollection? preposition, SynonymCollection? indirectObject, Room []?  roomsFilter)
        {
            ContainsTypeBlueprint = false;
            MustContain = null;
            
            Verb = verb;
            Noun = noun;
            Preposition = preposition;
            IndirectObject = indirectObject;
            
            RoomsFilter = roomsFilter;
        }
        
        public PhraseBlueprint(SynonymCollection mustContain, Room []?  roomsFilter)
        {
            ContainsTypeBlueprint = true;
            MustContain = mustContain;
            
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

            if (ContainsTypeBlueprint)
            {
                return
                    DoesWordMatch(MustContain, phrase.Verb) &&
                    DoesWordMatch(MustContain, phrase.Noun) &&
                    DoesWordMatch(MustContain, phrase.Preposition) &&
                    DoesWordMatch(MustContain, phrase.IndirectObject);
            }
            
            return 
                DoesWordMatch(Verb, phrase.Verb) &&
                   DoesWordMatch(Noun, phrase.Noun) &&
                   DoesWordMatch(Preposition, phrase.Preposition) &&
                   DoesWordMatch(IndirectObject, phrase.IndirectObject);
            
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