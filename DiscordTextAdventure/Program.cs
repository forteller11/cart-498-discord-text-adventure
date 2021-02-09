using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using chext.Discord;
using Discord;
using Discord.WebSocket;
using DiscordTextAdventure.Parsing;

#nullable enable
namespace DiscordTextAdventure
{
    class Program
    {
        private DiscordSocketClient _dissonanceBot;
        private DiscordSocketClient _bodyBot;
        private DiscordSocketClient _dankMemeBot;
        
        private GamesManager? _gamesManager;

        public static readonly HttpClient HttpClient = new HttpClient();
        
        public static readonly string BinPath;
        public static readonly string ProjectPath;
        public static readonly string AssetsPath;

        
        static Program()
        {
            BinPath = Directory.GetCurrentDirectory();
            ProjectPath = Directory.GetParent(BinPath).Parent.Parent.FullName;
            AssetsPath = Directory.GetParent(ProjectPath).FullName + "/assets";
            DebugLog(AssetsPath);

        }

        public Program()
        {
            _dissonanceBot = new DiscordSocketClient();
            _dankMemeBot   = new DiscordSocketClient();
            _bodyBot       = new DiscordSocketClient();

        }
        
        static void Main(string[] args) => new Program().MainAsync().Wait();
        
        public async Task MainAsync()
        {
            _dissonanceBot.Log += Log;
            _dankMemeBot.Log   += Log;
            _bodyBot.Log       += Log;

            bool dissoannceIsReady = false;
            bool memeIsReady       = false;
            bool bodyIsReady       = false;
            
            _dissonanceBot.Ready += () => CreateSessionIfAllBotsReady(ref dissoannceIsReady);
            _dankMemeBot.Ready   += () => CreateSessionIfAllBotsReady(ref memeIsReady);
            _bodyBot.Ready       += () => CreateSessionIfAllBotsReady(ref bodyIsReady);

            var dissonance = File.ReadAllText(ProjectPath + @"\Sensitive\dissonance-token.txt");
            var memeToken        = File.ReadAllText(ProjectPath + @"\Sensitive\meme-token.txt");
            var bodyToken        = File.ReadAllText(ProjectPath + @"\Sensitive\body-token.txt");

            Task[] loginTasks = new Task[3];
            loginTasks[0] =  _dissonanceBot.LoginAsync(TokenType.Bot, dissonance);
            loginTasks[1] =  _dankMemeBot.  LoginAsync(TokenType.Bot, memeToken);
            loginTasks[2] =  _bodyBot.      LoginAsync(TokenType.Bot, bodyToken);

            Task.WaitAll(loginTasks);
            
            Task[] startTasks = new Task[3];
            startTasks[0] =  _dissonanceBot.StartAsync();
            startTasks[1] =  _dankMemeBot.  StartAsync();
            startTasks[2] =  _bodyBot.      StartAsync();
            
            
            Task.WaitAll(startTasks);

            await Task.Delay(-1);

            Task CreateSessionIfAllBotsReady(ref bool isReady)
            {
                isReady = true;
                
                if (dissoannceIsReady && memeIsReady && bodyIsReady)
                {
                    _gamesManager = new GamesManager(_dissonanceBot, _dankMemeBot, _bodyBot);
                    Program.DebugLog("Created game maanger");
                }
                return Task.CompletedTask;
            }
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