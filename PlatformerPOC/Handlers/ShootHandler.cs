using PlatformerPOC.Domain.Weapon;
using PlatformerPOC.Events;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Handlers
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
            var bullet = new Bullet(_game, message.ShooterName, message.ShotPosition, message.HorizontalDirection);
            _game.Bullets.Add(bullet);
        }
    }
}