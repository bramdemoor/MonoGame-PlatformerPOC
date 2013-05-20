using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerPOC.Domain;
using PlatformerPOC.Helpers;

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
            foreach(var layer in game.gameWorld.bgLayers)
            {
                var myPos = new Vector2(-ViewArea.X*layer.Speed, 0);
                var tex = game.GameDataLoader.GetTextureByKey(layer.BgKey);
                spriteBatch.Draw(tex, myPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            foreach (var tile in game.gameWorld.Walls)
            {
                if(IsObjectInView(tile))
                {
                    var tex = game.GameDataLoader.GetTextureByTileKey(tile.TileKey);
                    var rect = game.GameDataLoader.GetRectangleByTileKey(tile.TileKey);

                    spriteBatch.Draw(tex, GetRelativeCoords(tile.Position), rect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }                
            }

            foreach (var coin in game.gameWorld.Powerups)
            {
                if(IsObjectInView(coin))
                {
                    var tex = game.GameDataLoader.GetTextureByTileKey("Bonus");
                    var rect = game.GameDataLoader.GetRectangleByTileKey("Bonus");

                    spriteBatch.Draw(tex, GetRelativeCoords(coin.Position), rect, Color.White);                
                }                
            }

            // TODO BDM: foreach particle:
            //spriteSheetInstance.Draw(PositionRelativeToView, DrawEffect, LayerDepths.FOREGROUND_PARTICLE);               

            foreach (var player in game.gameWorld.Players)
            {
                if(IsObjectInView(player))
                {
                    var dir = GetSpriteEffects(player.HorizontalDirection);
                    var playerPos = GetRelativeCoords(player.Position);
                    spriteBatch.Draw(player.spriteSheetInstance.SpriteSheetDefinition.SpriteSheetTexture, playerPos, player.spriteSheetInstance.CurrentDrawRectangle, Color.White, 0f, Vector2.Zero, 1f, dir, 0f);
                    
                    var displayText = string.Format("{0}", player.Name);
                    spriteBatch.DrawString(game.GameDataLoader.DefaultFont, displayText, playerPos, player.TextColor, 0, new Vector2(0, 30), 0.65f, SpriteEffects.None, 0f);                                    
                }                
            }

            foreach (var bullet in game.gameWorld.Bullets)
            {
                if(IsObjectInView(bullet))
                {
                    var tex = game.GameDataLoader.GetTextureByTileKey("PistolBullet");
                    var rect = game.GameDataLoader.GetRectangleByTileKey("PistolBullet");

                    var dir = GetSpriteEffects(bullet.HorizontalDirection);
                    spriteBatch.Draw(tex, GetRelativeCoords(bullet.Position), rect, Color.White, 0, Vector2.Zero, 1, dir, 0f);        
                }                
            }
        }

        private static SpriteEffects GetSpriteEffects(HorizontalDirection horizontalDirection)
        {
            return horizontalDirection == HorizontalDirection.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        private void DrawHud()
        {
            var str = string.Format("Round: {0}", game.RoundCounter);
            string modeAndLevelText = string.Format("{0} in {1}", "Elimination", game.gameWorld.CurrentLevelData.Name);
            string limitsText = "Score limit: 10 | Time limit: 10:00";

            const int leftTextStart = 1020;
            const int vTextSpace = 18;

            // Msg (center)
            spriteBatch.Draw(game.GameDataLoader.HudMsg, new Vector2(280, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            spriteBatch.DrawString(game.DefaultFont, "This is just a test message", new Vector2(300, 20), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);


            spriteBatch.Draw(game.GameDataLoader.HudText, new Vector2(1000, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            const int topSTart = 60;

            spriteBatch.DrawString(game.DefaultFont, str, new Vector2(leftTextStart, topSTart), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(game.DefaultFont, modeAndLevelText, new Vector2(leftTextStart, topSTart + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(game.DefaultFont, limitsText, new Vector2(leftTextStart, topSTart + vTextSpace + vTextSpace), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

            const int playersStart = 220;
            // Players section
            for (int index = 0; index < game.gameWorld.Players.Count; index++)
            {
                var player = game.gameWorld.Players[index];
                spriteBatch.DrawString(game.DefaultFont, player.Name, new Vector2(leftTextStart, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(game.DefaultFont, player.Score.ToString(), new Vector2(1170, playersStart + (index * vTextSpace)), player.TextColor, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            // player info section
            spriteBatch.DrawString(game.DefaultFont, "Player 1", new Vector2(leftTextStart, 550), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);            
        }

        private void DrawDebug()
        {
            DrawBorder(new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), 1, Color.Purple);

            DrawBorder(new Rectangle(0, 0, ViewArea.Width, ViewArea.Height), 5, Color.Yellow);

            foreach(var tile in game.gameWorld.Walls)
            {
                DrawBorder(GetRelativeCoords(tile.BoundingBox.FullRectangle), 2, Color.DarkRed);
            }

            foreach(var player in game.gameWorld.Players)
            {
                DrawBorder(GetRelativeCoords(player.BoundingBox.FullRectangle), 1, Color.Pink);    
            }

            foreach(var bullet in game.gameWorld.Bullets)
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
            spriteBatch.Draw(game.GameDataLoader.pixelForLines, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            spriteBatch.Draw(game.GameDataLoader.pixelForLines, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(game.GameDataLoader.pixelForLines, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(game.GameDataLoader.pixelForLines, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        /// <summary>
        /// Scroll the view to a specified horizontal coordinate.
        /// Optionally, apply camera shake effect
        /// FYI V-scrolling is blocked
        /// </summary>
        public void UpdateCameraPosition(float x, GameTime gameTime)
        {
            var position = new Vector2(x, 0);

            position = CamShaker.ShakeIfShaking(position, gameTime);

            var potentialX = (int)position.X - (ViewArea.Width / 2);
            var potentialY = (int)position.Y - (ViewArea.Height / 2);

            if (potentialX < LevelArea.X) potentialX = LevelArea.X;
            if (potentialX > LevelArea.Right - ViewArea.Width) potentialX = LevelArea.Right - ViewArea.Width;

            if (potentialY < LevelArea.Y) potentialY = LevelArea.Y;
            if (potentialY > LevelArea.Bottom - ViewArea.Height) potentialY = LevelArea.Bottom - ViewArea.Height;

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