using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    public class Pistol : PlatformGameObject
    {
        private readonly Player owner;

        private int intervalCounter = 0;
        private const int interval = 15;

        public Pistol(PlatformGame game, Player owner) : base(game)
        {
            this.owner = owner;
            game.AddObject(this);
        }

        public override void Update(GameTime gameTime)
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

            var bullet = new Bullet(game, owner, Position + new Vector2(30 * HorizontalDirection, 12), HorizontalDirection);
            game.AddObject(bullet);

            intervalCounter = interval;
        }

        public override void Draw()
        {
            
        }
    }
}