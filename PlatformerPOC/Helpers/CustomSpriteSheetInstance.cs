using System.Linq;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.Helpers
{
    public class CustomSpriteSheetInstance
    {
        public CustomSpriteSheetDefinition SpriteSheetDefinition { get; private set; }
        public int FramesToSkip { get; private set; }

        private int skipCounter;

        private int animationFrame;

        public Rectangle CurrentDrawRectangle
        {
            get { return SpriteSheetDefinition.Sprites.ElementAt(animationFrame); }
        }

        public bool IsOnLastFrame
        {
            get { return animationFrame == SpriteSheetDefinition.SpriteCount - 1; }
        }

        public CustomSpriteSheetInstance(CustomSpriteSheetDefinition spriteSheetDefinition, int framesToSkip)
        {
            SpriteSheetDefinition = spriteSheetDefinition;
            FramesToSkip = framesToSkip;
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