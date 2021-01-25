using System;
using Discord.WebSocket;
using DiscordTextAdventure.Mechanics.Rooms;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing.DataStructures;

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
        public Player? Player;
        public SocketMessage Message;
        public Room RoomOfPhrase;
    }
}