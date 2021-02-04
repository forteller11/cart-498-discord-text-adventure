
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using chext.Mechanics;
using Discord;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing.DataStructures;
using DiscordTextAdventure.Parsing.Tables;
using DiscordTextAdventure.Reflection;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    public class PhraseResponseTable
    {
        public readonly List<PhraseResponse> PhraseResponses = new List<PhraseResponse>();

        public readonly PhraseResponse SessionResetResponse;
        
        public readonly PhraseResponse LookResponse;
        public readonly PhraseResponse MoveResponse;
        
        public readonly PhraseResponse MeganSpeak;
        public readonly PhraseResponse MeganSpeakTo;
        public readonly PhraseResponse MeganTags;
        public readonly PhraseResponse MeganTarp;
        public readonly PhraseResponse MeganFridge;
        public readonly PhraseResponse MeganFlags;
        public readonly PhraseResponse MeganSlide;
        public readonly PhraseResponse MeganBot;
        
        // public readonly PhraseResponse MemeNotUnderstood;
        // public readonly PhraseResponse NeedCat;
        public readonly PhraseResponse BotSpeak;
        public readonly PhraseResponse BotSpeakTo;
        public readonly PhraseResponse FarmTravel;
        
        public readonly PhraseResponse FarmNoRoom;
        
        
        public PhraseResponseTable(RoomManager roomManager)
        {
            #region phrase responses
            
            SessionResetResponse = new PhraseResponse( new PhraseBlueprint(VerbTable.SessionReset, null, null, null, null), e =>
            {
                e.Session.SessionReset.Invoke(e.Session);
                e.Session.DissonanceBot.MessageReceived -= e.Session.OnMessageReceived;
                e.Session.DissonanceBot.ReactionAdded   -= e.Session.OnReactionAdded;
                e.Session.DissonanceBot.ReactionRemoved -= e.Session.OnReactionRemoved;

            }, null);
            
            MoveResponse = new PhraseResponse( new PhraseBlueprint(VerbTable.Inspect, null, null, null, null), LookResponseAction, null);
            LookResponse = new PhraseResponse( new PhraseBlueprint(VerbTable.Move, NounTable.Legs, null, null, null), MoveResponseAction, null);
            
            FarmNoRoom = new PhraseResponse(new PhraseBlueprint(null, NounTable.TheFarm, null, null, null), e =>
            {
                if (e.RoomOfPhrase == e.Session.RoomManager.Office)
                    return;
                
                if (!e.RoomOfPhrase.IsDMChannel)
                {
                    e.Session.RoomManager.DissonanceDM.RoomOwnerChannel.SendMessageAsync(
                        "I've seen you've noticed our custom ***\"the_farm\"***, I'm glad! We're very proud of The (server) Farm at Dissonance, it's how we manage to run all our tech and store all our user data!");
                }
            }, null);
            
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

            string cannedCatResponse =
                "From your throat comes a lot of dramatic meowing and whining which Megan sympathetically listens to, but unfortunately can't understand";
            
            MeganSpeakTo = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Speak, NounTable.Megan, null, null, new[] {roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync(
                        "I can small talk all day, but if I were you i'd some time to collect my thoughts and ask about something specific!");
                }, null);
                
                MeganSpeak = new PhraseResponse(new PhraseBlueprint(VerbTable.Speak, null, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync("So you must be the lucky contest winner! The names, Megan, I’m the senior and chief managing, administrative and creative director in training at *Dissonance*. We really value our customers time, is there anything you want to ask me about?");
                }, null);
           
            MeganSlide = new PhraseResponse(new PhraseBlueprint(null, NounTable.Slide, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The slide... yeah, well we used to own the entire upstairs floor before everything moved to the ***server farm***. So a slide was a fun, innovative way to encourage interdisciplinary communication.");
                }, null);
            
            
            MeganTags = new PhraseResponse(new PhraseBlueprint(null, NounTable.NameTags, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }
                    
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The name tags were from our former team of engineers. We used to own more of this space and have hundreds of workers, but when everything moved to the cloud, we were able to operate with a fraction of the staff. Now all the brains of the Disonance are housed in the ***Server Farm***, or just ***Farm*** as we call it around the office. This place’s main purpose is PR now, but we have a couple of Engineers still working on the ***Meme Machine***.");
                }, null);
            
            MeganBot = new PhraseResponse(new PhraseBlueprint(null, NounTable.MemeBot, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }
                    
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"I’m glad you asked! The ***Meme-Machine*** is a project we’re developing to reimagine advertising in a new digital, interconnected age. Memes spread across international and economic borders, we’re trying to harness that potential to communicate to, and expand our client base. The only problem is that it’s hard to transform products into original, viral memes. So, we’re developing a bot that will be able to intelligently do that for us! The only problem is right now it can only understand and communicate in emojis, and it only currently works on cats and cat-related products. If only Dissonance had access to kitty litter supply chains!");
                }, null);
            
            
            MeganTarp = new PhraseResponse(new PhraseBlueprint(null, NounTable.Tarp, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The tarp is to protect what’s underneath it from dust and coffee spills.");
                }, null);
            
            MeganFridge = new PhraseResponse(new PhraseBlueprint(null, NounTable.Fridge, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The transparent fridge is to keep people accountable to drink and eat organic and healthy!");
                }, null);
            
            MeganFlags = new PhraseResponse(new PhraseBlueprint(null, NounTable.Flags, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"We aspire to bring persons' from all walks of life together for the common goal of increasing *Dissonance's* market value.");
                }, null);
            #endregion


            #region bot
            BotSpeak = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Speak, null, null, null, new[] {roomManager.Office}),
                BotSpeakAction, null);
            
            BotSpeakTo = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Speak, NounTable.MemeBot, null, null, new[] {roomManager.Office}),
                BotSpeakAction, null);

            void BotSpeakAction(PhraseResponseEventArgs e)
            {
                if (e.RoomOfPhrase.TryFindFirstObject(NounTable.MemeBot) == null)
                    return;
                // var farm = e.Session.ReactionTable.FarmEmote;
                 e.RoomOfPhrase.MemeChannel.SendMessageAsync($"❓");
                // e.RoomOfPhrase.MemeChannel.SendMessageAsync($"<:{farm.Name}:{farm.Id}");
                e.Session.RoomManager.MemeDM.RoomOwnerChannel.SendMessageAsync(
                    "🤫🤫🤫🤫🤫🤫🤫🤫🤫\n > 🧍 ➡ 🐈 ➡ 🌐 ➡ 🖥️ ➡ ⛏ ➡ 🚫 ➡ 😄");
         
                // e.Session.RoomManager.MemeDM.RoomOwnerChannel.SendMessageAsync($"<:{farm.Name}:{farm.Id}");
            }
            FarmTravel= new PhraseResponse(
                new PhraseBlueprint(null, NounTable.TheFarm, null, null, new[] {roomManager.Office}),
                (e) =>
                {
                    if (e.RoomOfPhrase.TryFindFirstObject(NounTable.MemeBot) != null)
                    {
                        if (e.Session.Player.Role == Player.RoleTypes.Cat)
                        {
                            e.RoomOfPhrase.MemeChannel.SendMessageAsync("https://tenor.com/view/basher756-gif-20147055");
                            e.RoomOfPhrase.BodyChannel.SendMessageAsync("The feline body has disappeared, but its spirit is not lost. Transcending the physical world, the machine has turned us into a cat meme itself.");
                        }
                        else 
                            e.RoomOfPhrase.MemeChannel.SendMessageAsync("🧍 ➡ 🐱");
                    }

                    else
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("Who are you talking to?");
                    }
                }, null);
            #endregion
            
            PhraseResponses.AddRange(ReflectionHelpers.ClassMembersToArray<PhraseResponse>(typeof(PhraseResponseTable), this));
           
        }

    }
}