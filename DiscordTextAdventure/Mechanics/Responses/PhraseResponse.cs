using System;
using chext.Mechanics;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    //to respond to phrase with, phrase
    public class PhraseResponse
    {
        public readonly PhraseBlueprint PhraseBlueprint;
        public readonly Action<PhraseResponseEventArgs> Action;


        public PhraseResponse(PhraseBlueprint phraseBlueprint, Action<PhraseResponseEventArgs> action)
        {
            PhraseBlueprint = phraseBlueprint;
            Action = action;
        }
    }
    
    public class PhraseResponseEventArgs
    {
        public Phrase Phrase;
        public SocketMessage Message;
        public Room RoomOfPhrase;
        public Session Session;

        public PhraseResponseEventArgs(Phrase phrase, SocketMessage message, Room roomOfPhrase, Session session)
        {
            Phrase = phrase;
            Message = message;
            RoomOfPhrase = roomOfPhrase;
            Session = session;
        }
    }
}