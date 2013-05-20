using System;
using PlatformerPOC.Events;
using PlatformerPOC.Messages;

namespace PlatformerPOC
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
            //throw new NotImplementedException("Implement me plz!");

            //game.AddObject(new Particle(game, new Vector2(Position.X + (HorizontalDirection * 40), Position.Y), HorizontalDirection));
        }
    }
}