using System.Collections.Generic;
using Discord;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

#nullable enable
namespace DiscordTextAdventure.Parsing
{
    public class Parser
    {
        public Phrase Parse(List<Token> tokens, IMessageChannel channel)
        {
            var verbs = VerbTable.Verbs;
            var nouns = NounTable.Nouns;
            var prepositions = PrepositionTable.Prepositions;
            var IndirectObject = IndirectObjectTable.IndirectObjects;
            
            var phrase = new Phrase(channel);
            int tokenIndex = 0;
            
            phrase.Verb             = CheckForMatch(tokens, ref tokenIndex, verbs);
            phrase.Noun             = CheckForMatch(tokens, ref tokenIndex, nouns);
            phrase.Preposition      = CheckForMatch(tokens, ref tokenIndex, prepositions);
            phrase.IndirectObject   = CheckForMatch(tokens, ref tokenIndex, IndirectObject);

            return phrase;
            
            CompoundWord? CheckForMatch(List<Token> tokens, ref int index, SynonymCollection [] synonyms)
            {
                CompoundWord? matchedWord = null;
                for (int i = 0; i < synonyms.Length; i++)
                {
                    if (index >= tokens.Count)
                        return null;
                    
                    if (synonyms[i].CheckMatch(tokens, ref index, out matchedWord))
                        return matchedWord;
                }
                return matchedWord;
            }
                
        }
    }
}