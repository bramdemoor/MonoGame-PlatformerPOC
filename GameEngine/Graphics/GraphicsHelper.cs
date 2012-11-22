using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics
{
    public class GraphicsHelper
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly SimpleGame game;

        public GraphicsHelper(SimpleGame game)
        {
            this.game = game;

            graphics = new GraphicsDeviceManager(game);
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            
            graphics.PreferredBackBufferWidth = CoreConfig.ScreenResolutionWidth;
            graphics.PreferredBackBufferHeight = CoreConfig.ScreenResolutionHeight;

            // WHY: Debugger sucks when in fullscreen
            graphics.IsFullScreen = !CoreConfig.DebugModeEnabled;

            graphics.ApplyChanges();
        }

        public void StartDrawing()
        {
            game.GraphicsDevice.Clear(Color.Black);

            var screenscale = graphics.GraphicsDevice.Viewport.Width / (float)CoreConfig.ScreenResolutionWidth;
            var SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);

            game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, RasterizerState.CullNone, null, SpriteScale);
            //game.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        }

        public void EndDrawing()
        {
            if (CoreConfig.DebugModeEnabled)
            {
                game.DebugDrawHelper.DrawBorder(new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), 1, Color.Purple);
            }

            game.SpriteBatch.End();
        }
    }
}