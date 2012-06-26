using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Helpers
{
    public class FPSCounterComponent : DrawableGameComponent
    {
        readonly SpriteBatch spriteBatch;
        readonly SpriteFont spriteFont;

        int frameRate ;
        int frameCounter;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FPSCounterComponent(Microsoft.Xna.Framework.Game game, SpriteBatch Batch, SpriteFont Font) : base(game)
        {
            spriteFont = Font;
            spriteBatch = Batch;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime <= TimeSpan.FromSeconds(1)) return;

            elapsedTime -= TimeSpan.FromSeconds(1);
            frameRate = frameCounter;
            frameCounter = 0;
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            var fps = string.Format("fps: {0} mem : {1}", frameRate, GC.GetTotalMemory(false));

            spriteBatch.DrawString(spriteFont, fps, new Vector2(40, 10), Color.Red);
        }
    }
}