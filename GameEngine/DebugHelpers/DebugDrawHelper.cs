using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.DebugHelpers
{
    public class DebugDrawHelper
    {
        private readonly SimpleGame game;
        private readonly FPSCounter fpsCounter;

        Texture2D pixel;

        public DebugDrawHelper(SimpleGame game)
        {
            this.game = game;

            fpsCounter = new FPSCounter();
        }

        public void LoadContent()
        {
            pixel = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it 
        }

        public void Update(GameTime gameTime)
        {
            fpsCounter.Update(gameTime);
        }

        public void DrawFps()
        {
            fpsCounter.IncreaseFrames();

            if(CoreConfig.DebugModeEnabled)
            {
                var fps = string.Format("fps: {0} mem : {1}", fpsCounter.frameRate, GC.GetTotalMemory(false));

                game.SpriteBatch.DrawString(game.DefaultFont, fps, new Vector2(40, 10), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, -999);                
            }            
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        public void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            game.SpriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            game.SpriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            game.SpriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw bottom line
            game.SpriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }

        public void DrawDebugString(string s)
        {
            game.SpriteBatch.DrawString(game.DefaultFont, s, new Vector2(440, 30), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, -999);   
        }
    }
}