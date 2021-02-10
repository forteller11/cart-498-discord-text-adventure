using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Reflection;

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
                "top", "tabletop", "ttrp", "role", "playing", "game", "games", "dwarf", "rogue", "dwarfs", "rogues", "paladin", "summoner", "warlock",
                "bard", "tale", "lotr"
        };
        
        private static readonly string[] PokemonRelatedWords = {"pokemon", "pokeball", "greninja", "lucario", "mimikyu", "charizard", "umbreon", "sylveon", "pikachu", "squirtle",
                "ash", "catch", "gym", "trainer", "red", "blue", "mew", "mewtwo", "team", "rocket", "koffing", "ivysaur", "poke", "choose", "detective", "snorlax", 
                "anime", "cartoon", "game", "gold", "pokémon", "ryan", "reynolds", "silver", "gameboy", "ds", "nintendo", "ball"};
        
        private static readonly string[] AnimalRelatedWords =
        {
            "animal", "animals", "dog", "dogs", "cat", "cats", "funny",
            "silly", "dancing", "dance", "fur", "bark", "meow", "scratch", "paw", "scratching", "barking", "pet", "petting",
            "cute", "snout", "nature", "outdoor", "outdoors", "walk", "leash", "bird", "parrot", "monkey", "ape", "mammal",
            "horse", "lion", "fish", "bears", "bear", "tiger", "elephant", "shark", "leopard", "wolf", "panda", "snake", "bats", "bat",
            "deer", "whale", "chicken", "dolphin", "eagle", "gorilla", "tree", "grass", "trees", "lama", "weird", "play", "crazy", "cow", "hair", 
            "tail", "tails", "farm", "duck", "ducks", "goose", "geese", "paws", "chickens", "monkeys", "husky", "puppy", "puppies", "kitten", "kitty",
            "kittens", "litter", "ear", "ears" 
        };

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
                
                DnD = new LinkResponse(null, e =>
                        InterestingLinkCheck(e, DnDRelatedWords, e.Session.RoomManager.DnD,
                            relevantPhrasePicker,
                            irrelevantPhrasePicker,
                            1));
            }
            
            {
                
                var relevantPhrasePicker = new RandomPhrasePicker(
                    "A passerby shouts \"GOTTA CATCH EM' ALL.... WOOOOOO!\"", 
                    "A girl excitedly begins haggling you for special Pokémon trading cards, assuming that you're experienced player.",
                    "You find yourself in a heated argument about which starters are the best in each game, they explain why they're partial to water-types.",
                    "In response the channel is bombarded with detective pikachu memes.");
                                
                var irrelevantPhrasePicker = new RandomPhrasePicker(
                    "The woman doesn't look up from her DS",
                    "A man misses a pokéball, he gives you the evil eye, as if it were your fault.",
                    "You overhear a conversation in which you're compared to Magikarp");
                                
                Pokemon = new LinkResponse(null,e =>
                    InterestingLinkCheck(e, PokemonRelatedWords, e.Session.RoomManager.Pokemon,
                            relevantPhrasePicker,
                            irrelevantPhrasePicker,
                            1));
            }
            
            {
                var relevantPhrasePicker = new RandomPhrasePicker(
                    "A woman forgets to pick up after her dog, distracted by the post you sent.",
                    "A child laughs loudly -- telling that they love animals, especially kittens, even though they're not allowed to have any",
                    "\"Awwwwwwww\", that's precious!");
                
                var irrelevantPhrasePicker = new RandomPhrasePicker(
                    "A man fails to look up from his computer, where he's browsing ebay for doggie-coats.",
                    "A dog barks impatiently, their owner appears equally unimpressed.",
                    "A deer was approaching you, but decides there are better front yards to graze.");
                
                Animals = new LinkResponse(null, e =>
                    InterestingLinkCheck(e, AnimalRelatedWords, e.Session.RoomManager.Animals,
                        relevantPhrasePicker,
                        irrelevantPhrasePicker,
                        1));

            }

            Task InterestingLinkCheck (LinkResponseEventArgs e, string[] relevantWords, Room relevantRoom, RandomPhrasePicker phraseOnRelevant, RandomPhrasePicker phraseOnNotRelevant, int wordsThatMustMatch)
            {
                if (e.PostedRoom.RoomOwnerChannel.Id == relevantRoom.RoomOwnerChannel.Id
                    && e.Link.IsValid)
                {
                    if (ContainsWords(e.Link.Words, relevantWords, wordsThatMustMatch))
                    {
                        e.PostedRoom.RoomOwnerChannel.SendMessageAsync(phraseOnRelevant.GetNextPhrase());
                        e.Session.SucessfulGifPosts++;

                        if (e.Session.SucessfulGifPosts >= 3)
                        {
                            e.Session.RoomManager.DissonanceDM.RoomOwnerChannel.SendMessageAsync(
                                "Congrats!!!" +
                                "It's clear that you've made some big contributions to our community here at Dissonance!" +
                                "\nAs a reward, you're invited to our tech-forward headquarters, in ***The Cloud***!!!" +
                                "\nReact to the confetti to Accept."
                                ).ContinueWith(task => { task.Result.AddReactionAsync(new Emoji("🎉")); });

                            e.Session.RoomManager.DissonanceDM.RoomOwnerChannel.SendMessageAsync(
                                ":confetti_ball: :confetti_ball: :confetti_ball:");
           
                        }
                        
                    }
                    else
                        e.PostedRoom.RoomOwnerChannel.SendMessageAsync(phraseOnNotRelevant.GetNextPhrase());
                }

                return Task.CompletedTask;
            }

            void OnAcceptedToScreen()
            {
                
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

            LinkResponses = ReflectionHelpers.ClassMembersToArray<LinkResponse>(typeof(LinkResponseTable), null);


        }
    }
}