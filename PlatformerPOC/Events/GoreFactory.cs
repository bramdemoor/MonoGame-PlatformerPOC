using PlatformerPOC.Helpers;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Events
{
    public class GoreFactory : IListener<PlayerHitMessage>
    {
        private readonly PlatformGame _game;

        public GoreFactory(PlatformGame game)
        {
            _game = game;
        }

        public void Handle(PlayerHitMessage message)
        {        
            // TODO BDM: Temp test for cam shake
            CamShaker.StartShaking(10f, 1f);
            //game.AddObject(new Particle(game, new Vector2(Position.X + (HorizontalDirection * 40), Position.Y), HorizontalDirection));
        }
    }
}