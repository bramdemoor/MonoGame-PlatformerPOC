using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    public class Pistol : PlatformGameObject
    {
        private readonly Player owner;

        public Pistol(PlatformGame game, Player owner) : base(game)
        {
            this.owner = owner;
            game.AddObject(this);
        }

        public override void Update(GameTime gameTime)
        {
            Position = owner.Position;
            HorizontalDirection = owner.HorizontalDirection;
        }

        public void Shoot()
        {
            var bullet = new Bullet(game, owner, Position + new Vector2(30 * HorizontalDirection, 12), HorizontalDirection);
            game.AddObject(bullet);
        }

        public override void Draw()
        {
            
        }
    }
}