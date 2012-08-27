using GameEngine.Helpers;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    public class Particle : PlatformGameObject
    {
        private readonly CustomSpriteSheetInstance spriteSheetInstance;

        public Particle(Vector2 position, int horizontalDirection)
        {
            spriteSheetInstance = new CustomSpriteSheetInstance(ResourcesHelper.BulletImpactSpriteSheet, 2);

            Position = position;
            HorizontalDirection = horizontalDirection;
        }

        public override void Update(GameTime gameTime)
        {
            spriteSheetInstance.LoopUntilEnd();

            if (spriteSheetInstance.IsOnLastFrame)
            {
                DestroyEntity();
            }
        }

        public override void Draw()
        {
            spriteSheetInstance.Draw(Position, DrawEffect);
        }
    }
}