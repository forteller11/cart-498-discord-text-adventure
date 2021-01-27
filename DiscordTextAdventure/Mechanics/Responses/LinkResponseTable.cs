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

        private static readonly string[] DnDTypeWords = new[]
            {"tolken", "orc", "orcs", "elves", "elf", "elvish", "gnome", "dnd", "dungeon", "dungeons", "dragons", "lord", "ring", "sword", "swords"};

        static LinkResponseTable()
        {
            DnD = new LinkResponse(OnDnDLink, null);
            Pokemon = new LinkResponse(PokemonLink, null);
            Animals = new LinkResponse(AnimalsLink, null);
            
            
            
            void OnDnDLink (LinkResponseEventArgs e)
            {
                if (e.PostedRoom.MessageChannel.Id == e.Session.RoomManager.DnD.MessageChannel.Id
                    && e.Link.IsValid)
                {
                    if (ContainsWords(e.Link.Words, DnDTypeWords, 1))
                    {
                        e.PostedRoom.MessageChannel.SendMessageAsync("The dnd members seem to like that");
                    }
                }
            }
            
            void PokemonLink (LinkResponseEventArgs e)
            {
                if (e.PostedRoom.MessageChannel.Id == e.Session.RoomManager.DnD.MessageChannel.Id
                    && e.Link.IsValid)
                {
                    if (ContainsWords(e.Link.Words, DnDTypeWords, 1))
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
                    if (ContainsWords(e.Link.Words, DnDTypeWords, 1))
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