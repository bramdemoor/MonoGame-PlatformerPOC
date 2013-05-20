using Microsoft.Xna.Framework;
using PlatformerPOC.Drawing;
using PlatformerPOC.Helpers;

namespace PlatformerPOC.Domain
{
    public class Particle : BaseGameObject
    {
        private readonly CustomSpriteSheetInstance spriteSheetInstance;

        public Particle(PlatformGame game, Vector2 position, int horizontalDirection) : base(game)
        {
            spriteSheetInstance = new CustomSpriteSheetInstance(game, game.ResourcePreloader.BulletImpactSpriteSheet, 2);

            Position = position;
            HorizontalDirection = horizontalDirection;

            BoundingBox = new CustomBoundingBox();
            UpdateBoundingBox();
        }

        public override void Update(GameTime gameTime)
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
            BoundingBox.SetFullRectangle(Position, game.ResourcePreloader.BulletImpactSpriteSheet.SpriteDimensions, Velocity);
        }

        public override void Draw()
        {
            spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.FOREGROUND_PARTICLE);               
        }
    }
}