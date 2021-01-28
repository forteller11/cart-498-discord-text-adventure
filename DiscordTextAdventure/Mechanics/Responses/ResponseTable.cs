
using System;
using System.Collections.Generic;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;
using DiscordTextAdventure.Reflection;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class PhraseResponseTable
    {
        public readonly List<PhraseResponse> PhraseResponses = new List<PhraseResponse>();

        public PhraseResponseTable()
        {
     
            #region phrase responses
            new PhraseBlueprint(VerbTable.Inspect, NounTable.Arms, null, null);
            #endregion

            try
            {
                PhraseResponses.AddRange(
                    ReflectionHelpers.ClassMembersToArray<PhraseResponse>(typeof(PhraseResponseTable), null));
            }
            catch (ArgumentException)
            {
                //its fine, just no members
            }
        }

    }
}