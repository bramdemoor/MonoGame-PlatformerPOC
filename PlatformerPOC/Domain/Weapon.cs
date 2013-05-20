using Microsoft.Xna.Framework;
using PlatformerPOC.Messages;

namespace PlatformerPOC.Domain
{
    public class Weapon : BaseGameObject
    {
        private readonly Player owner;

        private int intervalCounter = 0;
        private const int interval = 15;

        public Weapon(PlatformGame game, Player owner) : base(game)
        {
            this.owner = owner;            
        }

        public void Update(GameTime gameTime)
        {
            if (intervalCounter > 0)
            {
                intervalCounter--;
            }

            Position = owner.Position;
            HorizontalDirection = owner.HorizontalDirection;
        }

        public void Shoot()
        {
            if (intervalCounter > 0) return;

            var shotPosition = Position + new Vector2(30 * HorizontalDirection, 12);

            PlatformGame.eventAggregationManager.SendMessage(new ShootMessage(owner.Name, shotPosition, HorizontalDirection));
            
            intervalCounter = interval;
        }        
    }
}