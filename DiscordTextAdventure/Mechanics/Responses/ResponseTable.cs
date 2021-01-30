
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;
using DiscordTextAdventure.Reflection;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class PhraseResponseTable
    {
        public readonly List<PhraseResponse> PhraseResponses = new List<PhraseResponse>();

        public readonly PhraseResponse LookResponse;
        public PhraseResponseTable()
        {
            #region phrase responses
            
            LookResponse = new PhraseResponse( new PhraseBlueprint(VerbTable.Inspect, null, null, null, null), null, LookResponseActionAsync);
            #endregion

            Task LookResponseActionAsync(PhraseResponseEventArgs e) =>
                e.RoomOfPhrase.MessageChannel.SendMessageAsync("Look at what?");

            PhraseResponses.AddRange(ReflectionHelpers.ClassMembersToArray<PhraseResponse>(typeof(PhraseResponseTable), this));
           
        }

    }
}