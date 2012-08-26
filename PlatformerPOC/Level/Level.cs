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
        // TODO BDM: Make spawnpoints objects!
        // Hardcoded spawnpoint locations
        private readonly List<Vector2> spawnPointTiles = new List<Vector2> { new Vector2(3, 10), new Vector2(15, 10) };

        public Level()
        {
            BuildHardcodedLevel();
        }

        private void BuildHardcodedLevel()
        {
            for (int y = 0; y < 14; y++)
            {
                // Left border
                AddTile(0, y, ResourcesHelper.TileWall);
            }        

            for (int y = 0; y < 14; y++)
            {
                // Right border
                AddTile(19, y, ResourcesHelper.TileWall);                
            }   
     
            for (int x = 0; x < 20; x++)
            {
                // Floor
                AddTile(x, 14, ResourcesHelper.TileGround);
            }

            AddTile(7, 12, ResourcesHelper.TileWall);
        }

        private static void AddTile(int x, int y, TileDefinition tileDefinition)
        {
            PlatformGame.Instance.MarkGameObjectForAdd(new SolidWall(LevelTileConcept.TilesToPixels(x, y), tileDefinition));
        }

        public void Draw()
        {
            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.BgLayer1Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            SimpleGameEngine.Instance.spriteBatch.Draw(ResourcesHelper.BgLayer2Texture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public bool IsInBoundsLeft(Vector2 position)
        {
            return position.X > LevelTileConcept.TilesToPixels(1);
        }

        public bool IsInBoundsRight(Vector2 position)
        {
            return position.X < LevelTileConcept.TilesToPixels(18);
        }

        public bool IsGroundBelow(Vector2 position)
        {
            return position.Y >= LevelTileConcept.TilesToPixels(13);
        }

        public bool IsInBounds(Vector2 position)
        {
            return IsInBoundsLeft(position) && IsInBoundsRight(position);
        }

        public Vector2 GetNextFreeSpawnPoint()
        {
            return LevelTileConcept.TilesToPixels(spawnPointTiles.First());
        }
    }
}