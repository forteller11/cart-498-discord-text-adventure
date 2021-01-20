using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using chext.Discord;
using Discord;
using Discord.WebSocket;

#nullable enable
namespace chext
{
    class Program
    {
        private DiscordSocketClient _client;
        private GamesManager _gamesManager;
        
        private static string? _binPath;
        private static string? _projectPath;
        
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        

        public async Task MainAsync()
        {
            Console.WriteLine("\n\n");
            _binPath = Directory.GetCurrentDirectory();
            _projectPath =  Directory.GetParent(_binPath).Parent.Parent.FullName;

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += () =>
            {
                _gamesManager = new GamesManager(_client);
                return Task.CompletedTask;
            };

            
            var token = File.ReadAllText(_projectPath + @"Sensitive\token.txt");
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            
            await Task.Delay(-1);
        }
        
        
        #region debug
        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        public static Task DebugLog(object message, string src="Debug")
        {
            Log(new LogMessage(LogSeverity.Debug, src, message.ToString()));
            return Task.CompletedTask;
        }
        
        public static Task WarningLog(object message, string src="Warning")
        {
            Log(new LogMessage(LogSeverity.Debug, src, message.ToString()));
            return Task.CompletedTask;
        }
        #endregion
    }
}