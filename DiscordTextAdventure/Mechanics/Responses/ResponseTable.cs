
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext.Mechanics;
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

        public readonly PhraseResponse MeganSpeak;
        public readonly PhraseResponse MeganTags;
        public readonly PhraseResponse MeganFridge;
        public readonly PhraseResponse MeganFlags;
        public readonly PhraseResponse MeganSlide;
        public readonly PhraseResponse MeganBot;
        public PhraseResponseTable(RoomManager roomManager)
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
            
            #region megan speak
            MeganSpeak = new PhraseResponse(new PhraseBlueprint(VerbTable.Speak, null, null, null, new []{roomManager.Megan}),
                e =>
                {
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync("I'm listening, ask me about something!");
                }, null);
           
            MeganSlide = new PhraseResponse(new PhraseBlueprint(NounTable.Slide, null, null, null, new []{roomManager.Megan}),
                e =>
                {
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The slide... yeah, well we used to own the entire upstairs floor before everything moved to the ***server farm***. So a slide was a fun, innovative way to encourage interdisciplinary communication.");
                }, null);
            
            
            MeganTags = new PhraseResponse(new PhraseBlueprint(NounTable.MemeBot, null, null, null, new []{roomManager.Megan}),
                e =>
                {
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The name tags were from our former team of engineers. We used to own more of this space and have hundreds of workers, but when everything moved to the cloud, we were able to operate with a fraction of the staff. Now all the brains of the Disonance are housed in the ***Server Farm***, or just ***Farm*** as we call it around the office. This place’s main purpose is PR now, but we have a couple of Engineers still working on the ***Meme Machine***.");
                }, null);
            
            MeganBot = new PhraseResponse(new PhraseBlueprint(NounTable.MemeBot, null, null, null, new []{roomManager.Megan}),
                e =>
                {
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"I’m glad you asked! The ***Meme-Machine*** is a project we’re developing to reimagine advertising in a new digital, interconnected age. Memes spread across international and economic borders, we’re trying to harness that potential to communicate to, and expand our client base. The only problem is that it’s hard to transform products into original, viral memes. So, we’re developing a bot that will be able to intelligently do that for us! The only problem is right now it can only understand and communicate in emojis, and it only currently works on cats and cat-related products. If only Dissonance had access to kitty litter supply chains!");
                }, null);
            
            #endregion

            PhraseResponses.AddRange(ReflectionHelpers.ClassMembersToArray<PhraseResponse>(typeof(PhraseResponseTable), this));
           
        }

    }
}