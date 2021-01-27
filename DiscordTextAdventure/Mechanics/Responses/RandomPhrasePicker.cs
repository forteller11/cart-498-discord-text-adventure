using System;
using System.Threading;
#nullable enable

namespace DiscordTextAdventure.Mechanics.Responses
{
    public class RandomPhrasePicker
    {
        private int _counter;
        private readonly string[] _phrases;
        private readonly object _lock = new object();

        public RandomPhrasePicker(params string[] phrases)
        {
            _phrases = phrases;
        }

        public string GetNextPhrase()
        {
            _counter++;

            int index = _counter % _phrases.Length;
            Program.DebugLog(index);
            return _phrases[index];
        }

      
    }
}