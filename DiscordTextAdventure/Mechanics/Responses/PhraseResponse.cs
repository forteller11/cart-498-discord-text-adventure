using System;
using System.Threading.Tasks;
using chext.Mechanics;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Parsing.DataStructures;

#nullable enable
namespace DiscordTextAdventure.Mechanics.Responses
{
    //to respond to phrase with, phrase
    public class PhraseResponse
    {
        public readonly PhraseBlueprint PhraseBlueprint;
        private readonly Action<PhraseResponseEventArgs> Action;
        private readonly Func<PhraseResponseEventArgs, Task> ActionAsync;

        public PhraseResponse(PhraseBlueprint phraseBlueprint, Action<PhraseResponseEventArgs> action)
        {
            PhraseBlueprint = phraseBlueprint;
            Action = action;
        }
        
        public PhraseResponse(PhraseBlueprint phraseBlueprint, Func<PhraseResponseEventArgs, Task> actionAsync)
        {
            PhraseBlueprint = phraseBlueprint;
            ActionAsync = actionAsync;
        }

        public void CallResponses(PhraseResponseEventArgs args)
        {
            Action?.Invoke(args);
            ActionAsync?.Invoke(args);
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