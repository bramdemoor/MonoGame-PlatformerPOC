using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.Drawing
{
    public class GraphicsHelper
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly PlatformGame game;

        public GraphicsHelper(PlatformGame game)
        {
            this.game = game;

            graphics = new GraphicsDeviceManager(game);
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            
            graphics.PreferredBackBufferWidth = Config.ScreenResolutionWidth;
            graphics.PreferredBackBufferHeight = Config.ScreenResolutionHeight;

            // WHY: Debugger sucks when in fullscreen
            graphics.IsFullScreen = !Config.DebugModeEnabled;

            graphics.ApplyChanges();
        }

        public void StartDrawing()
        {
            game.GraphicsDevice.Clear(Color.Black);

            var screenscale = graphics.GraphicsDevice.Viewport.Width / (float)Config.ScreenResolutionWidth;
            var SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);

            game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, RasterizerState.CullNone, null, SpriteScale);            
        }

        public void EndDrawing()
        {
            if (Config.DebugModeEnabled)
            {
                game.DebugDrawHelper.DrawBorder(new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), 1, Color.Purple);
            }

            game.SpriteBatch.End();
        }
    }
}