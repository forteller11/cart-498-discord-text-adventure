
using System;
using System.Collections.Generic;
using System.Net;
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

        public PhraseResponse SessionResetResponse;
        
        public PhraseResponse LookResponse;
        public PhraseResponse DestroyResponse;
        public PhraseResponse MoveResponse;
        
        public PhraseResponse DnDResponse;
        public PhraseResponse AnimalReponse;
        public PhraseResponse PokemonResponse;
        
        //publly PhraseResponse BodyDMResponse;
        public PhraseResponse DissonanceDMResponse;
        public PhraseResponse MemeGeneratorResponse;
        
        public PhraseResponse MeganSpeak;
        public PhraseResponse MeganDissonance;
        public PhraseResponse MeganHi01;
        public PhraseResponse MeganHi02;
        public PhraseResponse MeganSpeakTo;
        public PhraseResponse MeganTags;
        public PhraseResponse MeganTarp;
        public PhraseResponse MeganFridge;
        public PhraseResponse MeganFlags;
        public PhraseResponse MeganSlide;
        public PhraseResponse MeganPepe;
        public PhraseResponse MeganBot;
        
        public PhraseResponse BotResponse;
        public PhraseResponse BotFarmTravel;

        public PhraseResponse FarmNoRoom;
        public PhraseResponse UploadPassword;
        
        
        public void Init(RoomManager roomManager)
        {
            #region general responses
            DestroyResponse = new PhraseResponse( new PhraseBlueprint(VerbTable.Destroy, null, null, null, null), e =>
            {
                e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync($"{e.Message.Content} what?");

            }, null);
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
            

            void LookResponseAction(PhraseResponseEventArgs e)
            {
                if (e.RoomOfPhrase.Objects.Count == 0)
                    e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("This room doesn't contain anything to look at!");
                else
                    e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("Look at what?");
            
            }

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
            #endregion
            
            #region the screens
            string boringResponse = "the message seems to dissolve in the sea of moving images.";
            DnDResponse     = new PhraseResponse(new PhraseBlueprint(new [] {roomManager.DnD}),     e => e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(boringResponse), null);
            AnimalReponse   = new PhraseResponse(new PhraseBlueprint(new [] {roomManager.Animals}), e => e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(boringResponse), null);
            PokemonResponse = new PhraseResponse(new PhraseBlueprint(new [] {roomManager.Pokemon}), e => e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(boringResponse), null);
            #endregion
            
            #region dms
            DissonanceDMResponse = new PhraseResponse(new PhraseBlueprint(new [] {roomManager.DissonanceDM}),     e => 
                e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("This is a No-Reply Channel."), 
                null);
           // BodyDMResponse   = new PhraseResponse(new PhraseBlueprint(new [] {roomManager.BodyDM}), e => e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(boringResponse), null);
            MemeGeneratorResponse = new PhraseResponse(new PhraseBlueprint(new [] {roomManager.MemeDM}), e => e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("🔇"), null);
            #endregion
            
            #region megan speak
            string cannedCatResponse =
                "From your throat comes a lot of dramatic meowing and whining which Megan sympathetically listens to, but unfortunately can't understand";

            string hintResponse =
                "I can small talk all day, but if I were you i'd take some time to collect my thoughts and ask about something around the office while you have this opportunity!";

            string welcomeResponse = "So you must be the lucky contest winner! The names, Megan, I’m the senior and chief managing, administrative and creative director in training at *Dissonance*. We really value our customers time, if there's anything around the office you want to ask me about, I'm here!";
            
            MeganHi01 = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Salutation, null, null, null, new[] {roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync(welcomeResponse);
                }, null);
            
            MeganHi02 = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Salutation, NounTable.Megan, null, null, new[] {roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync(welcomeResponse);
                }, null);
            
            MeganSpeakTo = new PhraseResponse(
                new PhraseBlueprint(VerbTable.Speak, NounTable.Megan, null, null, new[] {roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync( welcomeResponse);
                }, null);
                
                MeganSpeak = new PhraseResponse(new PhraseBlueprint(VerbTable.Speak, null, null, null, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync(welcomeResponse);
                }, null);
           
            MeganSlide = new PhraseResponse(new PhraseBlueprint(NounTable.Slide, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The slide... yeah, well we used to own the entire upstairs floor before everything moved to the ***Server Farm***, which is where we now store all our user data and process it, we're so excited that we even developed a custom emote for it! So a slide was a fun, innovative way to encourage interdisciplinary communication, but now is more of a set piece.");
                }, null);
            
            MeganPepe = new PhraseResponse(new PhraseBlueprint(NounTable.Pepe, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync(
                        $"At Dissonance, we acknowledge the recent controversies sourronding Pepe the Frog and the alt right. Dissonance is a strictly a-political organization, we believe in the freedom of thought and rights of all individuals, epitomized by our laissez faire user-contract. We just love the Pepe Meme so much around the office and don't want to have all that fun sanitized just because a couple of bad actors. ");
                }, null);
            
            
            MeganTags = new PhraseResponse(new PhraseBlueprint(NounTable.NameTags, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }
                    
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The name tags were from our former team of engineers. We used to own more of this space and have hundreds of workers, but when everything moved to the cloud, we were able to operate with a fraction of the staff. Now all the brains of the Dissonance are housed in the ***Server Farm***, or just ***The Farm*** as we call it around the office. This place’s main purpose is PR now, but we have a couple of Engineers still working on the ***Meme Machine***. But all our hardware is now stored at ***The Farm*** now, we're so excited that we even developed a custom emote for the ***The Farm*** so people can join the conversation!");
                }, null);
            
            MeganBot = new PhraseResponse(new PhraseBlueprint(NounTable.MemeBot, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }
                    
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"I’m glad you asked! The ***Meme-Machine*** is a project we’re developing to reimagine advertising in a new digital, interconnected age. Memes spread across international and economic borders, we’re trying to harness that potential to communicate to, and expand our client base. The only problem is that it’s hard to transform products into original, viral memes. So, we’re developing a bot that will be able to intelligently do that for us! The only problem is right now it can only understand and communicate in emojis, and it only currently works on cats and cat-related products. If only Dissonance had access to kitty litter supply chains, we could turn them into viral memes and dominate the markets!");
                }, null);
            
            
            MeganTarp = new PhraseResponse(new PhraseBlueprint( NounTable.Tarp, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The tarp is to protect what’s underneath it from dust and coffee spills.");
                }, null);
            
            MeganFridge = new PhraseResponse(new PhraseBlueprint(NounTable.Fridge, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"The transparent fridge is to keep people accountable to drink and eat organic and healthy!");
                }, null);
            
            MeganFlags = new PhraseResponse(new PhraseBlueprint( NounTable.Flags, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"We aspire to bring persons' from all walks of life together for the common goal of increasing *Dissonance's* market value. We've recently moved our hardware to ***The (server) Farm*** so we can better support communities all across the globe!");
                }, null);
            
            MeganDissonance = new PhraseResponse(new PhraseBlueprint( NounTable.Dissonance, new []{roomManager.Megan}),
                e =>
                {
                    if (e.Session.Player.Role == Player.RoleTypes.Cat)
                    {
                        e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync(cannedCatResponse);
                        return;
                    }

                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"We're a cutting edge, tech forward, social media platform. We're excited to have recently moved all our hardware to ***The (server) Farm***, we even have a custom emote which users can now use to share their excitement!");
                }, null);
            
            MeganDissonance = new PhraseResponse(new PhraseBlueprint( NounTable.TheFarm, new []{roomManager.Megan}),
                e =>
                {
                    e.RoomOfPhrase.DissoanceChannel.SendMessageAsync($"I'm so glad to see you inspired by ***The Farm*** Emoji! We've been really excited to move all of our computing power to the cloud.");
                }, null);
            
            
            #endregion
            
            #region bot
            BotResponse = new PhraseResponse(
                new PhraseBlueprint(new[] {roomManager.Office}),
                e =>
                {
                    var bot = e.RoomOfPhrase.TryFindFirstObject(NounTable.MemeBot);
                    
                    if (bot != null)
                    {
                        if (e.Session.timesMessagedMemeBotNoDM == 1)
                        {
                            e.Session.RoomManager.MemeDM.RoomOwnerChannel.SendMessageAsync(
                                "\n🤫🤫🤫🤫🤫🤫🤫🤫🤫\n > 🧍 ➡ 🐈 ➡ 🌐 ➡ 🖥️ ➡ ⛏ ➡ 🚫 ➡ 😄\n. . .");
                        }
                        
                        if (e.Session.timesMessagedMemeBotNoDM % 3 == 1)
                        {
                            e.RoomOfPhrase.MemeChannel.SendMessageAsync("https://tenor.com/view/con-gif-4384521");
                            e.RoomOfPhrase.MemeChannel.SendMessageAsync("❓");
                        }

                        e.Session.timesMessagedMemeBotNoDM++;
                        
                    } 
                }, null);
            
            BotFarmTravel = new PhraseResponse(
                new PhraseBlueprint(NounTable.TheFarm, new[] {roomManager.Office}),
                e =>
                {
                    var bot = e.RoomOfPhrase.TryFindFirstObject(NounTable.MemeBot);
                    if (bot != null)
                    {
                        if (e.Session.Player?.Role != Player.RoleTypes.Cat)
                        {
                            e.Session.RoomManager.MemeDM.RoomOwnerChannel.SendMessageAsync("🐈");
                            return;
                        }
                        e.Session.RoomManager.Screen.ChangeRoomVisibilityAsync(e.Session, RoomCategory.NothingPermission);
                        e.Session.RoomManager.TheCloud.ChangeRoomVisibilityAsync(e.Session, RoomCategory.NothingPermission);
                        e.Session.RoomManager.Intro.ChangeRoomVisibilityAsync(e.Session, RoomCategory.NothingPermission);
                        e.Session.RoomManager.TheFarm.ChangeRoomVisibilityAsync(e.Session, RoomCategory.ViewAndSendPermission);

                        e.Session.CanSeeOffice = false;
                        e.Session.CanSeeServer = true;
                        e.Session.CanSeeIntro  = false;
                        
                        e.Session.RoomManager.BodyDM.RoomOwnerChannel.SendMessageAsync("https://tenor.com/view/basher756-gif-20147055");
                        e.Session.RoomManager.MemeDM.RoomOwnerChannel.SendMessageAsync("🏌️ 😼 ↗➡↘ 💥");
                        e.Session.RoomManager.BodyDM.RoomOwnerChannel.SendMessageAsync("The feline body has disappeared, but its spirit is not lost. Transcending the organic realm, the machine has turned us into a cat meme itself. An idea, a comedic moment -- compressed and serialized into a stream of bytes, able to reach the most remote areas of internet.");
                    } 
                }, null);
            
            #endregion
            
            #region the farm 
            UploadPassword = new PhraseResponse(
                new PhraseBlueprint( new[] {roomManager.ControlRoom}),
                e =>
                {
                    if (e.Message.Attachments.Count > 0)
                    {
                        foreach (var txt in e.Message.Attachments)
                        { 
                            Uri uri = new Uri(txt.Url);
                           e.Session.HttpClient.GetStringAsync(uri).ContinueWith(task =>
                           {
                               Program.DebugLog(task.Result);
                               if (task.Result.ToLower().Trim() == "password")
                               {
                                   e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("Correct Password.\nShutting Down *The Farm's* Firewall.");
                                   e.Session.RoomManager.Screen.ChangeRoomVisibilityAsync(e.Session, RoomCategory.ViewAndSendPermission);
                               }
                               else
                               {
                                   e.RoomOfPhrase.RoomOwnerChannel.SendMessageAsync("Incorrect Password. Please Try Again.");
                               }
                           });
                        }
                    }
                }, null);

            #endregion
            
            PhraseResponses.AddRange(ReflectionHelpers.ClassMembersToArray<PhraseResponse>(typeof(PhraseResponseTable), this));
           
        }

    }
}