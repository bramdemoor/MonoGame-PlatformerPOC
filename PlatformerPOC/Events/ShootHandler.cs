using PlatformerPOC.Domain;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Events
{
    public class ShootHandler : IListener<ShootMessage>
    {
        private readonly PlatformGame _game;

        public ShootHandler(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(ShootMessage message)
        {
            var bullet = new Projectile(_game, message.ShooterName, message.ShotPosition, message.HorizontalDirection);
            _game.gameWorld.Bullets.Add(bullet);
        }
    }
}