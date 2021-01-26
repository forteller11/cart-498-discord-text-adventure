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
             
                IDMChannel? dmChannel;
                if (e.Session.Player?.User.Id == e.User.Id) //player already not null and player is the user who reacted
                {
                    var player = e.Session.Player;
                    dmChannel = await e.Session.Player.SocketUser.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync(
                        $"{player.User.Username}, it looks like you are trying to VOID the user-agreement you accepted" +
                        $"at {player.AcceptUserAgreement.ToLocalTime().ToLongTimeString()} on {player.AcceptUserAgreement.ToShortDateString()}" +
                        $"\nWe regret to inform you at the Dissonance legal team, that you can not revoke these terms of service." +
                        $"\nYour data is now stored in our servers and third party affiliate.");
                }
                else if (e.Session.Player == null)
                {
                    e.Session.Player = new Player(e.User);
                    e.Session.RoomManager.Screen.ChangeRoomVisibilityAsync(e.Session,
                        RoomCategory.ViewAndSendPermission);
                    dmChannel = await  e.Session.Player.SocketUser.GetOrCreateDMChannelAsync();
                    await dmChannel.SendMessageAsync(
                        $"Welcome { e.Session.Player.User.Username}!" +
                        $"\nThese are exciting times in which you're entering Dissonance server, as we have a special event currently taking place!." +
                        $"\nThe user with the most contributions to our community, will have the privilege to visit *The Cloud*. " +
                        $"\nOur headquarters where you'll get to me out ambitious crew and have a chance to get to know our cutting edge technology." +
                        $"\nMake sure to participate! We recommend getting to know each one of our communities and posting relevant content!");
                    //you can read about more here: 404
                }
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