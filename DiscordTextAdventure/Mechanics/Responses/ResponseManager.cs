using System.Collections.Generic;
using DiscordTextAdventure.Mechanics.User;
using DiscordTextAdventure.Parsing.DataStructures;

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class ResponseManager
    {
        List<Response> _responses = new List<Response>();

        public void CallResponseFromPhrase(Phrase phrase, Player player)
        {
            for (int i = 0; i < _responses.Count; i++)
            {
                if (_responses[i].PhraseBlueprint.MatchesPhrase(phrase))
                {
                    var responseArgs = new ResponseEventArg();
                    responseArgs.Phrase = phrase;
                    responseArgs.Player = player;
                    _responses[i].Action.Invoke(responseArgs);
                }
            }
        }
    }
}