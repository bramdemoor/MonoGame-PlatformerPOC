using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Domain;
using PlatformerPOC.Domain.Level;
using PlatformerPOC.Drawing;

namespace PlatformerPOC
{
    /// <summary>
    /// Centralized rendering and viewport implementation.
    /// </summary>
    public class Renderer
    {
        public SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;
        private readonly PlatformGame game;

        private Rectangle ViewArea { get; set; }
        public Rectangle LevelArea { get; set; }

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

            ViewArea = new Rectangle(0, 0, Config.ScreenResolutionWidth - 280, Config.ScreenResolutionHeight);
        }

        /// <summary>
        /// Converts view coordinates into world coordinates
        /// </summary>        
        public Vector2 GetWorldCoords(Vector2 viewCoords)
        {
            return new Vector2(viewCoords.X + ViewArea.X, viewCoords.Y + ViewArea.Y);
        }

        /// <summary>
        /// Single public method
        /// </summary>
        public void Draw()
        {
            game.GraphicsDevice.Clear(Color.Black);

            var screenscale = graphics.GraphicsDevice.Viewport.Width / (float)Config.ScreenResolutionWidth;
            var SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, RasterizerState.CullNone, null, SpriteScale);

            DrawAll();

            DrawHud();

            if (Config.DebugModeEnabled)
            {
                DrawDebug();                
            }

            spriteBatch.End();
        }

        private void DrawAll()
        {
            var level = game.LevelManager.CurrentLevel;
            
            foreach(var layer in level.bgLayers)
            {
                // TODO BDM: Use speed per layer
                var myPos = new Vector2(-ViewArea.X*BgLayer.PARALLAX_LAYER1_SPEED);
                spriteBatch.Draw(layer.texture, myPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            foreach(var tile in level.Walls)
            {
                if(IsObjectInView(tile))
                {                                                
                    spriteBatch.Draw(tile.TileDefinition.TileSet.TilesetTexture, GetRelativeCoords(tile.Position),tile.TileDefinition.graphicsRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }                
            }

            foreach(var coin in level.Coins)
            {
                if(IsObjectInView(coin))
                {
                    spriteBatch.Draw(game.ResourcePreloader.ObjectTiles.TilesetTexture, GetRelativeCoords(coin.Position), game.ResourcePreloader.ObjectTiles.GetGraphicsRectangle(0, 4), Color.White);                
                }                
            }

            // TODO BDM: foreach particle:
            //spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.FOREGROUND_PARTICLE);               

            foreach (var player in game.Players)
            {
                if(IsObjectInView(player))
                {
                    var dir = player.HorizontalDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    var playerPos = GetRelativeCoords(player.Position);
                    spriteBatch.Draw(player.spriteSheetInstance.SpriteSheetDefinition.SpriteSheetTexture, playerPos, player.spriteSheetInstance.CurrentDrawRectangle, Color.White, 0f, Vector2.Zero, 1f, dir, 0f);
                    
                    var displayText = string.Format("{0}", player.Name);
                    spriteBatch.DrawString(game.ResourcePreloader.DefaultFont, displayText, playerPos, player.TextColor, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, 0f);                                    
                }                
            }
            
            foreach( var bullet in game.Bullets)
            {
                if(IsObjectInView(bullet))
                {
                    var dir = bullet.HorizontalDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    spriteBatch.Draw(game.ResourcePreloader.BulletTexture, GetRelativeCoords(bullet.Position), null, Color.White, 0, Vector2.Zero, 1, dir, 0f);        
                }                
            }
        }

        private void DrawHud()
        {
            var str = string.Format("Round: {0}", game.RoundCounter);
            string modeAndLevelText = string.Format("{0} in {1}", "Elimination", game.LevelManager.CurrentLevel.Name);
            string limitsText = "Score limit: 10 | Time limit: 10:00";

            const int leftTextStart = 1020;
            const int vTextSpace = 18;

            // Msg (center)
            spriteBatch.Draw(game.ResourcePreloader.HudMsg, new Vector2(280, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            spriteBatch.DrawString(game.DefaultFont, "This is just a test message", new Vector2(300, 20), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);


            spriteBatch.Draw(game.ResourcePreloader.HudText, new Vector2(1000, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            const int topSTart = 60;

            spriteBatch.DrawString(game.DefaultFont, str, new Vector2(leftTextStart, topSTart), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(game.DefaultFont, modeAndLevelText, new Vector2(leftTextStart, topSTart + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(game.DefaultFont, limitsText, new Vector2(leftTextStart, topSTart + vTextSpace + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

            const int playersStart = 220;
            // Players section
            for (int index = 0; index < game.Players.Count; index++)
            {
                var player = game.Players[index];
                spriteBatch.DrawString(game.DefaultFont, player.Name, new Vector2(leftTextStart, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(game.DefaultFont, player.Score.ToString(), new Vector2(1170, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            // player info section
            spriteBatch.DrawString(game.DefaultFont, "Player 1", new Vector2(leftTextStart, 550), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            spriteBatch.Draw(game.ResourcePreloader.Pistol, new Vector2(1068, 662), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);            
        }

        private void DrawDebug()
        {
            DrawBorder(new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), 1, Color.Purple);

            DrawBorder(new Rectangle(0, 0, ViewArea.Width, ViewArea.Height), 5, Color.Yellow);

            foreach(var tile in game.LevelManager.CurrentLevel.Walls)
            {
                DrawBorder(GetRelativeCoords(tile.BoundingBox.FullRectangle), 2, Color.DarkRed);
            }

            foreach(var player in game.Players)
            {
                DrawBorder(GetRelativeCoords(player.BoundingBox.FullRectangle), 1, Color.Pink);    
            }

            foreach(var bullet in game.Bullets)
            {
                DrawBorder(GetRelativeCoords(bullet.BoundingBox.FullRectangle), 1, Color.Lime);    
            }

            if (Config.DebugModeEnabled)
            {
                var fps = string.Format("fps: {0} mem : {1}", game.fpsCounter.frameRate, GC.GetTotalMemory(false));

                spriteBatch.DrawString(game.DefaultFont, fps, new Vector2(40, 10), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, -999);
            }    
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        /// Order: Top > Left > Right > Bottom
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {            
            spriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            spriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(game.ResourcePreloader.pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        /// <summary>
        /// Block V scrolling
        /// </summary>
        public void ScrollToHorizontal(float x)
        {
            Vector2 position = new Vector2(x, 0);
            var potentialX = (int)position.X - (ViewArea.Width / 2);

            var potentialY = (int)position.Y - (ViewArea.Height / 2);

            if (potentialX < LevelArea.X) potentialX = LevelArea.X;
            if (potentialX > LevelArea.Right - ViewArea.Width) potentialX = LevelArea.Right - ViewArea.Width;

            if (potentialY < LevelArea.Y) potentialY = LevelArea.Y;
            if (potentialY > LevelArea.Bottom - ViewArea.Height) potentialY = LevelArea.Bottom - ViewArea.Height;

            // TODO BDM: Checks on 'level out of bounds'

            ViewArea = new Rectangle(potentialX, potentialY, ViewArea.Width, ViewArea.Height);
        }

        private bool IsObjectInView(BaseGameObject obj)
        {
            if (obj.BoundingBox == null) return true;   // Otherwise, objects without bounding box can never be drawn!
            return IsObjectInArea(obj.BoundingBox.FullRectangle);
        }

        private bool IsObjectInArea(Rectangle objectRectangle)
        {
            return ViewArea.Intersects(objectRectangle);
        }

        /// <summary>
        /// Converts world coordinates into view coordinates
        /// </summary>        
        private Vector2 GetRelativeCoords(Vector2 realWorldCoords)
        {
            return new Vector2(realWorldCoords.X - ViewArea.X, realWorldCoords.Y - ViewArea.Y);
        }

        /// <summary>
        /// Converts world rectangle into view rectangle
        /// </summary>        
        private Rectangle GetRelativeCoords(Rectangle realWorldRectangle)
        {
            return new Rectangle(realWorldRectangle.X - ViewArea.X, realWorldRectangle.Y - ViewArea.Y, realWorldRectangle.Width, realWorldRectangle.Height);
        }
    }
}