using Microsoft.Xna.Framework;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    public class Particle : BaseGameObject
    {
        private readonly CustomSpriteSheetInstance spriteSheetInstance;

        public Particle(PlatformGame game, Vector2 position, HorizontalDirection horizontalDirection)
        {
            //spriteSheetInstance = new CustomSpriteSheetInstance(game.GameDataLoader.BulletImpactSpriteSheet, 2);

            Position = position;
            HorizontalDirection = horizontalDirection;

            BoundingBox = new CustomBoundingBox();
            UpdateBoundingBox();
        }

        public void Update(GameTime gameTime)
        {
            spriteSheetInstance.LoopUntilEnd();

            if (spriteSheetInstance.IsOnLastFrame)
            {
                DestroyEntity();
            }

            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            BoundingBox.SetFullRectangle(Position, spriteSheetInstance.SpriteSheetDefinition.SpriteDimensions, Velocity);
        }
    }
}