using PlatformerPOC.GameObjects;

namespace PlatformerPOC.EventArgs
{
    public class PlayerStateChangedArgs : System.EventArgs
    {
        public Player Player { get; private set; }

        public PlayerStateChangedArgs(Player player)
        {
            Player = player;
        }        
    }
}