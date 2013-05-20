using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC
{
    /// <summary>
    /// Attemp to centralize drawing
    /// </summary>
    public class Renderer
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly PlatformGame game;

        // TODO BDM: Add "Inview" checks for all game objects!

        public Renderer(PlatformGame game)
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

        /// <summary>
        /// Single public method
        /// </summary>
        public void Draw()
        {
            game.GraphicsDevice.Clear(Color.Black);

            var screenscale = graphics.GraphicsDevice.Viewport.Width / (float)Config.ScreenResolutionWidth;
            var SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);

            game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullNone, null, SpriteScale);

            DrawAll();

            DrawHud();

            if (Config.DebugModeEnabled)
            {
                DrawDebug();                
            }

            game.SpriteBatch.End();
        }

        private void DrawAll()
        {
            var level = game.LevelManager.CurrentLevel;
            
            foreach(var layer in level.bgLayers)
            {
                game.SpriteBatch.Draw(layer.texture, layer.Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            // foreach "tile"
            //TileDefinition.DrawTile(PositionRelativeToView, 0);   

            // foreach particle:
            //spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.FOREGROUND_PARTICLE);               

            // foreach player
            //spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.GAMEOBJECTS);
            //var displayText = string.Format("{0}", Name);
            //game.SpriteBatch.DrawString(game.ResourcePreloader.DefaultFont, displayText, PositionRelativeToView, TextColor, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, LayerDepths.TEXT);

            // foreach coin
            //game.SpriteBatch.Draw(game.ResourcePreloader.ObjectTiles.TilesetTexture, PositionRelativeToView, game.ResourcePreloader.ObjectTiles.GetGraphicsRectangle(0, 4), Color.White);            

            // foreach bullet
            //SpriteBatch.Draw(game.ResourcePreloader.BulletTexture, PositionRelativeToView, null, Color.White, 0, Vector2.Zero, 1, DrawEffect, 1f);                    
        }

        public void DrawHud()
        {
            var str = string.Format("Round: {0}", game.RoundCounter);
            string modeAndLevelText = string.Format("{0} in {1}", "Elimination", game.LevelManager.CurrentLevel.Name);
            string limitsText = "Score limit: 10 | Time limit: 10:00";

            const int leftTextStart = 1020;
            const int vTextSpace = 18;

            // Msg (center)
            game.SpriteBatch.Draw(game.ResourcePreloader.HudMsg, new Vector2(280, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            game.SpriteBatch.DrawString(game.DefaultFont, "This is just a test message", new Vector2(300, 20), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);


            game.SpriteBatch.Draw(game.ResourcePreloader.HudText, new Vector2(1000, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            const int topSTart = 60;

            game.SpriteBatch.DrawString(game.DefaultFont, str, new Vector2(leftTextStart, topSTart), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            game.SpriteBatch.DrawString(game.DefaultFont, modeAndLevelText, new Vector2(leftTextStart, topSTart + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            game.SpriteBatch.DrawString(game.DefaultFont, limitsText, new Vector2(leftTextStart, topSTart + vTextSpace + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

            const int playersStart = 220;
            // Players section
            for (int index = 0; index < game.Players.Count; index++)
            {
                var player = game.Players[index];
                game.SpriteBatch.DrawString(game.DefaultFont, player.Name, new Vector2(leftTextStart, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                game.SpriteBatch.DrawString(game.DefaultFont, player.Score.ToString(), new Vector2(1170, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            // player info section
            game.SpriteBatch.DrawString(game.DefaultFont, "Player 1", new Vector2(leftTextStart, 550), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            game.SpriteBatch.Draw(game.ResourcePreloader.Pistol, new Vector2(1068, 662), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);            
        }

        public void DrawDebug()
        {
            DrawBorder(new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), 1, Color.Purple);

            DrawBorder(new Rectangle(0, 0, game.ViewPort.ViewArea.Width, game.ViewPort.ViewArea.Height), 5, Color.Yellow);

            // foreach tile/solidwall
            //game.DebugDrawHelper.DrawBorder(ViewPort.GetRelativeCoords(BoundingBox.FullRectangle), 2, Color.DarkRed);

            // foreach player:
            //game.DebugDrawHelper.DrawBorder(ViewPort.GetRelativeCoords(BoundingBox.FullRectangle), 1, Color.Pink);

            // foreach bullet:
            //game.DebugDrawHelper.DrawBorder(ViewPort.GetRelativeCoords(BoundingBox.FullRectangle), 1, Color.Lime);
        }



        public void DrawFps()
        {
            game.fpsCounter.IncreaseFrames();

            if (Config.DebugModeEnabled)
            {
                var fps = string.Format("fps: {0} mem : {1}", game.fpsCounter.frameRate, GC.GetTotalMemory(false));

                game.SpriteBatch.DrawString(game.DefaultFont, fps, new Vector2(40, 10), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, -999);                
            }            
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        /// Order: Top > Left > Right > Bottom
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        public void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {            
            game.SpriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            game.SpriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            game.SpriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            game.SpriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }
    }


}