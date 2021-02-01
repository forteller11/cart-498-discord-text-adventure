using System;
using System.Net.Sockets;
using Discord;
using Discord.WebSocket;

namespace DiscordTextAdventure.Mechanics.User
{
    public class Player
    {
        public SocketUser SocketUser;
        public IUser User;
        public readonly DateTime AcceptUserAgreement;
        public RoleTypes Role;

        public enum RoleTypes
        {
            Human=default,
            Dwarf,
            Magikarp,
            Koffing,
            Cat
        }
        //public SocketGuildUser GuildUser;
        //public DiscordSocketClient Client;



        public Player (IUser user)
        {
            SocketUser = (SocketUser) user;
            User = user;
            AcceptUserAgreement = DateTime.Now;
        }
        public void OnLogOut()
        {
            
        }
    }
}