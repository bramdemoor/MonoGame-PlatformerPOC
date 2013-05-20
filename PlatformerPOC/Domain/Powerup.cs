using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    public enum PowerupType
    {
        SmallBonus,
        HealthRestoreFull
    }

    public class Powerup : BaseGameObject
    {
        private static int powerUpIdCounter = 1;

        public int Id { get; private set; } 

        public Powerup(Vector2 pos)
        {
            Id = powerUpIdCounter++;
            Position = pos;
            BoundingBox = new CustomBoundingBox();
            BoundingBox.SetFullRectangle(Position, new Rectangle(0, 0, 16, 16), Velocity);
        }        
    }
}