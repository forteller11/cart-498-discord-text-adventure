using Discord.WebSocket;

namespace DiscordTextAdventure.Mechanics.Player
{
    public class Player
    {
        public SocketUser User;
        public SocketGuildUser GuildUser;
        public DiscordSocketClient Client;

        public Player (SocketGuildUser user, DiscordSocketClient client)
        {
            User = (SocketUser) user;
            GuildUser = user;
        }

        public void OnLogOut()
        {
            
        }
    }
}