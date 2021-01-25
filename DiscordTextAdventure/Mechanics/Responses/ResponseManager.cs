// using System.Collections.Generic;
// using DiscordTextAdventure.Mechanics.User;
// using DiscordTextAdventure.Parsing.DataStructures;
//
// #nullable enable
// namespace DiscordTextAdventure.Mechanics.Responses
// {
//     public class ResponseManager
//     {
//         List<PhraseResponse> _responses;
//         List<ReactionResponse> _responses;
//
//         public ResponseManager()
//         {
//             _responses = ResponseTable.GetStaticResponseList();
//             _responses = ResponseTable.GetStaticResponseList();
//         }
//         
//         public void CallResponseFromPhrase(Phrase phrase, Player player)
//         {
//             for (int i = 0; i < _responses.Count; i++)
//             {
//                 if (_responses[i].PhraseBlueprint.MatchesPhrase(phrase))
//                 {
//                     var responseArgs = new PhraseResponseEventArgs();
//                     responseArgs.Phrase = phrase;
//                     responseArgs.Player = player;
//                     _responses[i].Action.Invoke(responseArgs);
//                 }
//             }
//         }
//     }
// }