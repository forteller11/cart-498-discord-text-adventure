using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Parsing.DataStructures;

namespace DiscordTextAdventure.Mechanics.Responses
{
    #nullable enable
    public class LinkResponseTable
    {
        public static readonly LinkResponse DnD;
        public static readonly LinkResponse Pokemon;
        public static readonly LinkResponse Animals;
        public static readonly LinkResponse [] LinkResponses;

        private static RandomPhrasePicker picker = new RandomPhrasePicker(
            "A rogue breaks away from their argument over what constitues a skill check, seemingly intrigued by the *content* you posted.",
            "A Dwarf stops admiring their gold bars momentarily as your *content* is noticed.");
        private static readonly string[] DnDRelatedWords = {
            "tolken", "orc", "orcs", "elves", "elf", "elvish", "gnome", "dnd", "dungeon", "dungeons", "dragons", 
                "lord", "ring", "sword", "swords", "axe", "battle", "war", "skill", "dice", "d20", "skillcheck", "gnome", 
                "gnomes", "bar", "drink", "quest", "quests", "gold", "tolken", "fantasy", "druid", "magic", "spells", "table", 
                "top", "tabletop", "ttrp", "role", "playing", "game", "games"};
        
        private static readonly string[] PokemonRelatedWords = new[]
            {"tolken", "orc", "orcs", "elves", "elf", "elvish", "gnome", "dnd", "dungeon", "dungeons", "dragons", "lord", "ring", "sword", "swords"};

        static LinkResponseTable()
        {
            
            //have to instantiate outside lamdba for some reason or else behavior breaks
            {
                var relevantPhrasePicker = new RandomPhrasePicker(
                    "A rogue breaks away from their argument over what constitutes a skill check, seemingly intrigued by the *content* you posted.",
                    "A Dwarf stops admiring their gold bars momentarily as your *content* is noticed.",
                    "You hear laughter come from around the tables");
                
                var irrelevantPhrasePicker = new RandomPhrasePicker(
                    "The members' seem to be more interested in their D20's...",
                    "This didn't seem to scratch anyone's high-fantasy flavoured itch",
                    "The member's stayed focused on their main quests, unmoved by the distraction.");
                
                DnD = new LinkResponse(e =>
                        InterestingLinkCheck(e, DnDRelatedWords, e.Session.RoomManager.DnD,
                            relevantPhrasePicker,
                            irrelevantPhrasePicker),
                    null);
            }
            
            {
                var relevantPhrasePicker = new RandomPhrasePicker(
                    "A rogue breaks away from their argument over what constitutes a skill check, seemingly intrigued by the *content* you posted.",
                    "A Dwarf stops admiring their gold bars momentarily as your *content* is noticed.",
                    "You hear laughter come from around the tables");
                
                var irrelevantPhrasePicker = new RandomPhrasePicker(
                    "The members' seem to be more interested in their D20's...",
                    "This didn't seem to scratch anyone's high-fantasy flavoured itch",
                    "The member's stayed focused on their main quests, unmoved by the distraction.");
                
                Pokemon = new LinkResponse(e =>
                        InterestingLinkCheck(e, PokemonTypeWords, e.Session.RoomManager.Pokemon,
                            relevantPhrasePicker,
                            irrelevantPhrasePicker),
                    null);
            }
            //Animals = new LinkResponse(AnimalsLink, null);
            
            
            void InterestingLinkCheck (LinkResponseEventArgs e, string[] relevantWords, Room relevantRoom, 
                RandomPhrasePicker phraseOnRelevant, RandomPhrasePicker phraseOnNotRelevant)
            {
                if (e.PostedRoom.MessageChannel.Id == relevantRoom.MessageChannel.Id
                    && e.Link.IsValid)
                {
                    if (ContainsWords(e.Link.Words, relevantWords, 1))
                        e.PostedRoom.MessageChannel.SendMessageAsync(phraseOnRelevant.GetNextPhrase());
                    else
                        e.PostedRoom.MessageChannel.SendMessageAsync(phraseOnNotRelevant.GetNextPhrase());
                }
            }
            
            void PokemonLink (LinkResponseEventArgs e)
            {
                if (e.PostedRoom.MessageChannel.Id == e.Session.RoomManager.DnD.MessageChannel.Id
                    && e.Link.IsValid)
                {
                    if (ContainsWords(e.Link.Words, DnDRelatedWords, 1))
                    {
                        e.PostedRoom.MessageChannel.SendMessageAsync("The pokemon members seem to like that");
                    }
                }
            }
            
            void AnimalsLink (LinkResponseEventArgs e)
            {
                if (e.PostedRoom.MessageChannel.Id == e.Session.RoomManager.DnD.MessageChannel.Id
                    && e.Link.IsValid)
                {
                    if (ContainsWords(e.Link.Words, DnDRelatedWords, 1))
                    {
                        e.PostedRoom.MessageChannel.SendMessageAsync("The animal lovers seem to like that");
                    }
                }
            }

            bool ContainsWords(List<Token> tokens, string[] words, int wordsMustMatchMin)
            {
                int wordsContained = 0;
                for (int i = 0; i < tokens.Count; i++)
                {
                    for (int j = 0; j < words.Length; j++)
                    {
                        if (tokens[i].Raw == words[j])
                        {
                            wordsContained++;
                            if (wordsContained >= wordsMustMatchMin)
                                return true;
                        }
                    }
                }

                return false;
            }

            LinkResponses = Common.ClassMembersToArray<LinkResponse>(typeof(LinkResponseTable), null);


        }
    }
}