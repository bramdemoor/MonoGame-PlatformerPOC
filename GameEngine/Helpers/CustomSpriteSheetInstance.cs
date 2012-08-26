using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Helpers
{
    public class CustomSpriteSheetInstance
    {
        public CustomSpriteSheetDefinition SpriteSheetDefinition { get; private set; }

        private int animationFrame = 0;

        private Rectangle CurrentDrawRectangle
        {
            get { return SpriteSheetDefinition.Sprites.ElementAt(animationFrame); }
        }

        public bool IsOnLastFrame
        {
            get { return animationFrame == SpriteSheetDefinition.SpriteCount - 1; }
        }

        public CustomSpriteSheetInstance(CustomSpriteSheetDefinition spriteSheetDefinition)
        {
            this.SpriteSheetDefinition = spriteSheetDefinition;
        }

        public virtual void Draw(Vector2 position, SpriteEffects drawEffect)
        {
            SimpleGameEngine.Instance.spriteBatch.Draw(SpriteSheetDefinition.SpriteSheetTexture, position, CurrentDrawRectangle, Color.White, 0f, Vector2.Zero, 1f, drawEffect, 0f);
        }

        public void LoopUntilEnd()
        {
            if (IsOnLastFrame)
            {
                // Do nothing!
            }
            else
            {
                animationFrame++;
            }
        }

        public void LoopWithReverse()
        {
            if (IsOnLastFrame)
            {
                animationFrame = 0;
            }
            else
            {
                animationFrame++;
            }
        }
    }
}