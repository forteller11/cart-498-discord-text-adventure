using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chext;
using Discord;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class PhraseResponseTable
    {
        public readonly static PhraseResponse[] PhraseResponses;
        public readonly static PhraseResponse GoNorth;
        public readonly static PhraseResponse HelloMemeBot;
        

        static PhraseResponseTable()
        {
     
            #region phrase responses
            GoNorth = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Move, NounTable.North, null, null),
                GoNorthAction
                );
            
            HelloMemeBot = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Salutation, NounTable.DankMemeBot, null, null), MemeBotHelloResponse
                );

            void GoNorthAction(PhraseResponseEventArgs e) => Program.DebugLog("GO NORTH RESPONSE");
            
            void MemeBotHelloResponse(PhraseResponseEventArgs e) => e.RoomOfPhrase.MessageChannel.SendMessageAsync($"Hello {e.Message.Author.Username}, are you a memer aswell?");
            #endregion

            PhraseResponses = Common.ClassMembersToArray<PhraseResponse>(typeof(PhraseResponseTable), null);
        }

    }
}