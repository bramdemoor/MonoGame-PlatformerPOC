using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Helpers
{
    public class CustomSpriteSheetInstance
    {
        public CustomSpriteSheetDefinition SpriteSheetDefinition { get; private set; }
        public int FramesToSkip { get; private set; }

        private int skipCounter;

        private int animationFrame;

        private Rectangle CurrentDrawRectangle
        {
            get { return SpriteSheetDefinition.Sprites.ElementAt(animationFrame); }
        }

        public bool IsOnLastFrame
        {
            get { return animationFrame == SpriteSheetDefinition.SpriteCount - 1; }
        }

        public CustomSpriteSheetInstance(CustomSpriteSheetDefinition spriteSheetDefinition, int framesToSkip)
        {
            this.SpriteSheetDefinition = spriteSheetDefinition;
            FramesToSkip = framesToSkip;
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
                if(skipCounter >= FramesToSkip)
                {
                    skipCounter = 0;
                    animationFrame++;
                }
                else
                {
                    skipCounter++;
                }                
            }
        }

        public void LoopWithReverse()
        {
            if (IsOnLastFrame)
            {
                animationFrame = 0;
                skipCounter = 0;
            }
            else
            {
                if (skipCounter >= FramesToSkip)
                {
                    skipCounter = 0;
                    animationFrame++;
                }
                else
                {
                    skipCounter++;
                }      
            }
        }
    }
}