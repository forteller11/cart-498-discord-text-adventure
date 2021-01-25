using Discord;
using Discord.WebSocket;

namespace DiscordTextAdventure.Mechanics.User
{
    public class Player
    {
        public SocketUser SocketUser;
        public IUser User;
        //public SocketGuildUser GuildUser;
        //public DiscordSocketClient Client;



        public Player (IUser user)
        {
            SocketUser = (SocketUser) user;
            User = user;
  
            
            //user.
        }
        public void OnLogOut()
        {
            
        }
    }
}