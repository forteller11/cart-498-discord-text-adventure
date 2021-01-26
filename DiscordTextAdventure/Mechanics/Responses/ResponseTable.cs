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
            
            void MemeBotHelloResponse(PhraseResponseEventArgs e) => e.RoomOfPhrase.MessageChannel.SendMessageAsync($"Hello {e.Message.Author.Username}, are you a memer aswell?");
            #endregion
            
            #region emote responses
            AcceptUserAgreement = new ReactionResponse(new Emoji("✅"), SetPlayer );
            
            async Task SetPlayer(ReactionResponseEventArgs e)
            {
                e.Session.Player = new Player(e.User);
                e.Session.RoomManager.Screen.ChangeRoomVisibilityAsync(e.Session, RoomCategory.ViewAndSendPermission);
                var dm = await e.Session.Player.SocketUser.GetOrCreateDMChannelAsync();
                await dm.SendMessageAsync($"Welcome to the server {e.Session.Player.User.Username}! " +
                                          $"\nThese are exciting times you're entering the server, we have a special event." +
                                          $"\nThe user with the most contributions to our community, will get to visit our headquarters where " +
                                          $"you'll get to meet our tech-forward crew and cutting edge technology! Message today! ");
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