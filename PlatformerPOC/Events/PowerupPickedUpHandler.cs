using System.Linq;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Events
{
    public class PowerupPickedUpHandler : IListener<PowerupPickedUpMessage>
    {
        private readonly PlatformGame _game;

        public PowerupPickedUpHandler(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(PowerupPickedUpMessage message)
        {
            _game.gameWorld.Coins = _game.gameWorld.Coins.Where(c => c.Id != message.PowerUpObjectId).ToList();
        }
    }
}