using DiscordTextAdventure.Mechanics.Rooms;

namespace DiscordTextAdventure.Mechanics.Responses
{
    public interface IResponse
    {
        Room[] RoomFilter { get; set; }
        
        
    }
}