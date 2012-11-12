using GameEngine.Collision;
using GameEngine.Spritesheet;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.GameObjects
{
    public class Particle : PlatformGameObject
    {
        private readonly CustomSpriteSheetInstance spriteSheetInstance;

        public Particle(PlatformGame game, Vector2 position, int horizontalDirection) : base(game)
        {
            spriteSheetInstance = new CustomSpriteSheetInstance(game, game.ResourcesHelper.BulletImpactSpriteSheet, 2);

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
            BoundingBox.SetFullRectangle(Position, game.ResourcesHelper.BulletImpactSpriteSheet.SpriteDimensions, Velocity);
        }

        public override void Draw()
        {
            if (!InView) return;

            spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.FOREGROUND_PARTICLE);               
        }
    }
}