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
        public readonly static PhraseResponse HelloMemeBot;
        
        public readonly static ReactionResponse AcceptUserAgreement;

        static ResponseTable()
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
            
            void MemeBotHelloResponse(PhraseResponseEventArgs e) => e.RoomOfPhrase.Channel.SendMessageAsync($"Hello {e.Message.Author.Username}, are you a memer aswell?");
            #endregion
            
            #region emote responses
            AcceptUserAgreement = new ReactionResponse(new Emoji("✅"), SetPlayer );
            
            void SetPlayer(ReactionResponseEventArgs e)
            {
     
                e.Session.Player = new Player(e.User);
                Program.DebugLog("Player set");
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