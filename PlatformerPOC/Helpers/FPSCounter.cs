using System;
using Microsoft.Xna.Framework;

namespace PlatformerPOC.Helpers
{
    public class FPSCounter
    {
        public int frameRate { get; private set; }
        int frameCounter;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime <= TimeSpan.FromSeconds(1)) return;

            elapsedTime -= TimeSpan.FromSeconds(1);
            frameRate = frameCounter;
            frameCounter = 0;
        }

        public void IncreaseFrames()
        {
            frameCounter++;
        }
    }
}