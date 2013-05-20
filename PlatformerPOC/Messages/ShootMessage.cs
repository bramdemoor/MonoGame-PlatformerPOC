using Microsoft.Xna.Framework;
using PlatformerPOC.Domain;

namespace PlatformerPOC.Messages
{
    public class ShootMessage
    {
        public string ShooterName { get; set; }
        public Vector2 ShotPosition { get; set; }
        public HorizontalDirection HorizontalDirection { get; set; }

        public ShootMessage(string shooterName, Vector2 shotPosition, HorizontalDirection horizontalDirection)
        {
            ShooterName = shooterName;
            ShotPosition = shotPosition;
            HorizontalDirection = horizontalDirection;
        }
    }
}