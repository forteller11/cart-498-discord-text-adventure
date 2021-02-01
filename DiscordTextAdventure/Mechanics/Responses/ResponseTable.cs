
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordTextAdventure.Mechanics.Rooms;
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
        public readonly PhraseResponse MoveResponse;
        public PhraseResponseTable()
        {
            #region phrase responses
            
            MoveResponse = new PhraseResponse( new PhraseBlueprint(VerbTable.Inspect, null, null, null, null), LookResponseAction, null);
            LookResponse = new PhraseResponse( new PhraseBlueprint(VerbTable.Move, NounTable.Legs, null, null, null), MoveResponseAction, null);
            
            #endregion

            void LookResponseAction(PhraseResponseEventArgs e) => e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("Look at what?");

            void MoveResponseAction(PhraseResponseEventArgs e)
            {
                var legs = e.RoomOfPhrase.TryFindFirstObject(NounTable.Legs);
                if (legs != null)
                {
                    Program.DebugLog("not null legs");
                    e.Session.RoomManager.MoveAdventureObject(legs, e.Session.RoomManager.Animals);

                    e.RoomOfPhrase.Renderer.DrawRoomStateEmbed();
                    e.Session.RoomManager.Animals.Renderer.DrawRoomStateEmbed();
                    e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("moved leg.");

                }
  

            }

            PhraseResponses.AddRange(ReflectionHelpers.ClassMembersToArray<PhraseResponse>(typeof(PhraseResponseTable), this));
           
        }

    }
}