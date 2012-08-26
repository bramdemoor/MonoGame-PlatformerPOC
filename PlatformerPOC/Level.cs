using System.Collections.Generic;
using System.Linq;
using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerPOC.GameObjects
{
    public class Level
    {
        public const int SquareSize = 32;

        // Hardcoded spawnpoint locations
        private readonly List<Vector2> spawnPointTiles = new List<Vector2> { new Vector2(3, 10), new Vector2(15, 10) };

        public void Draw()
        {
            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.BgLayer1Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.BgLayer2Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Hardcoded map tiles!

            for (int y = 0; y < 14; y++) DrawTile(new Vector2(0, y), 0, 1);        // Left border
            for (int y = 0; y < 14; y++) DrawTile(new Vector2(19, y), 0, 1);        // Right border
            for (int x = 0; x < 20; x++) DrawTile(new Vector2(x, 14), 0, 3);        // Floor

        }

        public int TilesToPixels(int tiles)
        {
            return tiles*SquareSize;
        }

        public Vector2 TilesToPixels(Vector2 tiles)
        {
            return tiles * SquareSize;
        }

        private void DrawTile(Vector2 pos, int xtile, int ytile)
        {
            var sourceRectangle = new Rectangle(xtile * SquareSize, ytile * SquareSize, SquareSize, SquareSize);

            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.TilesetTexture, TilesToPixels(pos), sourceRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public bool IsInBoundsLeft(Vector2 position)
        {
            return position.X > TilesToPixels(1);
        }

        public bool IsInBoundsRight(Vector2 position)
        {
            return position.X < TilesToPixels(18);
        }

        public bool IsGroundBelow(Vector2 position)
        {
            return position.Y >= TilesToPixels(13);
        }

        public bool IsInBounds(Vector2 position)
        {
            return IsInBoundsLeft(position) && IsInBoundsRight(position);
        }

        public Vector2 GetNextFreeSpawnPoint()
        {
            return TilesToPixels(spawnPointTiles.First());
        }
    }
}