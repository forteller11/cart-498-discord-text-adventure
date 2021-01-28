using DiscordTextAdventure.Mechanics.Rooms;

#nullable enable

namespace DiscordTextAdventure.Parsing.DataStructures
{
    public class PhraseBlueprint
    {
        public SynonymCollection? Verb;
        public SynonymCollection? Noun;
        public SynonymCollection? Preposition;
        public SynonymCollection? IndirectObject;
        public Room [] ? RoomsFilter; //null == all rooms


        public PhraseBlueprint(SynonymCollection? verb, SynonymCollection? noun, SynonymCollection? preposition, SynonymCollection? indirectObject, params Room []? roomsFilter)
        {
            Verb = verb;
            Noun = noun;
            Preposition = preposition;
            IndirectObject = indirectObject;
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
                    if (RoomsFilter[i].MessageChannel!.Id == room.MessageChannel!.Id)
                    {
                        roomMatches = true;
                        break;
                    }
                }

                if (!roomMatches)
                    return false;
            }

            return DoesWordMatch(Verb, phrase.Verb) &&
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