using System;
using System.Collections.Generic;
using System.Linq;
using chext;
using DiscordTextAdventure.Parsing;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ResponseTable
    {
        public readonly static Response GoNorth;

        static ResponseTable()
        {
     
            GoNorth = new Response(
                new PhraseBlueprint(VerbTable.Move, NounTable.North, null, null),
                GoNorthAction
                );

            void GoNorthAction(ResponseEventArg e) => Program.DebugLog("GO NORTH RESPONSE");
        }

        /// <summary>
        /// while the responses themselves are static and shared among all sessions,
        /// the returned list is specefic only to the specefic session, and is therefore safe to add/remove from dynamically
        /// </summary>
        public static List<Response> GetStaticResponseList()
        {
            return Common.ClassMembersToArray<Response>(typeof(ResponseTable), null).ToList();
        }



    }
}