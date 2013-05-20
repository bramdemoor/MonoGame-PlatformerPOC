using Microsoft.Xna.Framework;

namespace PlatformerPOC.Messages
{
    public class ShootMessage
    {
        public string ShooterName { get; set; }
        public Vector2 ShotPosition { get; set; }
        public int HorizontalDirection { get; set; }

        public ShootMessage(string shooterName, Vector2 shotPosition, int horizontalDirection)
        {
            ShooterName = shooterName;
            ShotPosition = shotPosition;
            HorizontalDirection = horizontalDirection;
        }
    }
}