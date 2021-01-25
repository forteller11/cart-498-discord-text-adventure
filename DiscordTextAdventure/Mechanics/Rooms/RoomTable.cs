// using Discord;
// using Microsoft.VisualBasic.CompilerServices;
//
// namespace DiscordTextAdventure.Mechanics.Rooms
// {
//     public static class RoomTable
//     {
//         
//         public readonly Room UserAgreement;
//         public readonly Room TestRoom;
//         public readonly Room TestRoom2;
//         
//     void RoomTable()
//     {
//         #region create rooms
//
//         UserAgreement = new Room("User Agreement")
//             .WithStaticDescriptions("*user agreement")
//             .WithReaction(new Emoji("✅"));
//             
//         TestRoom = new Room("test room name")
//             .WithStaticDescriptions("here lies a fun pot")
//             .WithObjects(
//                 new AdventureObject("apple", "the apple has a tooth in it"),
//                 new AdventureObject("pole", "a long thin pole")
//             );
//             
//         TestRoom2 = new Room("funny second room name")
//             .WithStaticDescriptions("i like doughnuts");
//             
//         #endregion
//
//         #region deal with how rooms are connected by default visibility
//     }
//     }
// }