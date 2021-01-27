using DiscordTextAdventure.Mechanics.Rooms;

namespace DiscordTextAdventure.Mechanics.Responses
{
    #nullable enable
    public class LinkResponseTable
    {
        public readonly LinkResponse DnD;
        public readonly LinkResponse Pokemon;
        public readonly LinkResponse Animals;
        public readonly LinkResponse [] LinkResponses;

        static LinkResponseTable()
        {
            DnD = new LinkResponse();
            
            
            
            void RelevantDND (LinkResponseEventArgs e)
            {
                if (e.PostedRoom == RoomManager.
            }
                
                
        }
    }
}