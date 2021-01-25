using System;
using System.Collections.Generic;
using System.Linq;
using chext;
using Discord;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ResponseTable
    {
        public readonly static PhraseResponse GoNorth;
        public readonly static ReactionResponse AcceptUserAgreement;

        static ResponseTable()
        {
     
            #region phrase responses
            GoNorth = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Move, NounTable.North, null, null),
                GoNorthAction
                );

            void GoNorthAction(PhraseResponseEventArgs e) => Program.DebugLog("GO NORTH RESPONSE");
            #endregion
            
            #region emote responses
            AcceptUserAgreement = new ReactionResponse(new Emoji("✅"),null, SetPlayer );
            
            void SetPlayer(ReactionResponseEventArgs e, IUserMessage userMessage)
            {
                e.Player = new Player(userMessage.Author);

                Program.DebugLog("Set player");
            }
            #endregion
        }

        /// <summary>
        /// while the responses themselves are static and shared among all sessions,
        /// the returned list is specefic only to the specefic session, and is therefore safe to add/remove from dynamically
        /// </summary>
        public static List<PhraseResponse> GetStaticPhraseResponseList()
        {
            return Common.ClassMembersToArray<PhraseResponse>(typeof(ResponseTable), null).ToList();
        }
        
        public static List<ReactionResponse> GetStaticReactionResponseList()
        {
            return Common.ClassMembersToArray<ReactionResponse>(typeof(ResponseTable), null).ToList();
        }



    }
}