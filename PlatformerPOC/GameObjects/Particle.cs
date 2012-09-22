using GameEngine;
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
            BoundingBox.SetFullRectangle(Position, ResourcesHelper.BulletImpactSpriteSheet.SpriteDimensions, Velocity);
        }

        public override void Draw()
        {
            // TODO BDM: Make check + coord conversion generic properties on (platform)gameobject. E.g.: ShouldDraw property and ScreenPosition readonly properties

            if (ViewPort.IsObjectInArea(BoundingBox.FullRectangle))
            {
                var relativePos = ViewPort.GetRelativeCoords(Position);

                spriteSheetInstance.Draw(relativePos, DrawEffect, LayerDepths.FOREGROUND_PARTICLE);
            }    
        }
    }
}